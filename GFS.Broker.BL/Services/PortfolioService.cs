using AutoMapper;
using GFS.Broker.Api.Enums;
using GFS.Broker.Api.Models.Portfolio;
using GFS.Broker.BL.Models;
using GFS.Broker.DAL.Entities;
using GFS.Common.Extensions;
using GFS.EF.Extensions;
using GFS.EF.Repository;
using Microsoft.EntityFrameworkCore;

namespace GFS.Broker.BL.Services
{
    public interface IPortfolioService
    {
        Task<PortfolioInfoDto> CreatePortfolio(CreatePortfolioRequestDto request);
        Task DeletePortfolio(DeletePortfolioRequestDto request);
        Task<PortfolioInfoDto> GetPortfolioInfo(GetPortfolioInfoRequestDto request);
        Task<List<PortfolioInfoDto>> GetAllPortfoliosInfo();
        Task<PortfolioInfoWithHistoryDto> GetPortfolioInfoWithHistory(GetPortfolioInfoRequestDto request);
        Task<PortfolioOperationResponseDto> PerformDepositOperation(PortfolioDepositOperationRequestDto request);
        Task<PortfolioOperationResponseDto> PerformDealOperation(PerformDealOperationRequestDto request);
    }

    internal class PortfolioService : IPortfolioService
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public PortfolioService(
            IDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PortfolioInfoDto> CreatePortfolio(CreatePortfolioRequestDto request)
        {
            var portfolio = _dbContext.GetRepository<PortfolioEntity>().Insert(_mapper.Map<PortfolioEntity>(request));
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<PortfolioInfoDto>(portfolio);
        }

        public async Task DeletePortfolio(DeletePortfolioRequestDto request)
        {
            await _dbContext.GetRepository<PortfolioEntity>().DeleteById(request.PortfolioId);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PortfolioInfoDto> GetPortfolioInfo(GetPortfolioInfoRequestDto request)
        {
            var portfolio = await PortfolioWithOperations()
                .Where(p => p.Id == request.PortfolioId)
                .AsNoTracking()
                .SingleOrFailAsync();

            return _mapper.Map<PortfolioInfoDto>(portfolio);
        }

        public async Task<List<PortfolioInfoDto>> GetAllPortfoliosInfo()
        {
            var portfolios = await PortfolioWithOperations()
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<PortfolioInfoDto>>(portfolios);
        }

        public Task<PortfolioInfoWithHistoryDto> GetPortfolioInfoWithHistory(GetPortfolioInfoRequestDto request)
        {
            throw new NotImplementedException();
            
            // var portfolio = await PortfolioWithOperations()
            //     .Where(p => p.Id == request.PortfolioId)
            //     .Include(p => p.DealOperations)
            //     .AsNoTracking()
            //     .SingleOrFailAsync();
            //
            // return _mapper.Map<PortfolioInfoWithHistoryDto>(portfolio);
        }

        public async Task<PortfolioOperationResponseDto> PerformDepositOperation(PortfolioDepositOperationRequestDto request)
        {
            var portfolio = await PortfolioWithOperations()
                .Where(p => p.Id == request.PortfolioId)
                .SingleOrFailAsync();

            if (request.CashAmount is null or < 0)
                return PortfolioOperationResponseDto.GetFailResponse("Сумма средств не может быть меньше 0");

            if (request.PortfolioOperationType == PortfolioDepositOperationType.Take && portfolio.GetCashAmount() < request.CashAmount)
                return PortfolioOperationResponseDto.GetFailResponse("Недостаточно наличных средств в портфеле");
                
            var cashChange = request.PortfolioOperationType == PortfolioDepositOperationType.Deposit
                ? request.CashAmount!.Value
                : -request.CashAmount!.Value;

            portfolio.DepositOperations.Add(new DepositOperationEntity
            {
                MomentUtc = request.MomentUtc,
                OperationType = PortfolioDepositOperationType.Deposit,
                CashChange = cashChange
            });

            await _dbContext.SaveChangesAsync();

            return PortfolioOperationResponseDto.GetSuccessResponse(_mapper.Map<PortfolioInfoDto>(portfolio));
        }

        public async Task<PortfolioOperationResponseDto> PerformDealOperation(PerformDealOperationRequestDto request)
        {
            request.Validate();
            
            var portfolio = await PortfolioWithOperations()
                .Where(p => p.Id == request.PortfolioId)
                .SingleOrFailAsync();

            if (portfolio.GetCashAmount() + request.DeltaCash < 0)
                return PortfolioOperationResponseDto.GetFailResponse("Недостаточно наличных средств в портфеле");

            portfolio.DealOperations.Add(new DealOperationEntity
            {
                MomentUtc = request.MomentUtc,
                OperationType = request.OperationType,
                AssetId = request.AssetId,
                AssetDealPrice = request.DealPrice,
                AssetUnitsCountChange = request.DeltaAsset,
                CashChange = request.DeltaCash
            });

            await _dbContext.SaveChangesAsync();

            return PortfolioOperationResponseDto.GetSuccessResponse(_mapper.Map<PortfolioInfoDto>(portfolio));
        }

        private IQueryable<PortfolioEntity> PortfolioWithOperations()
            => _dbContext
                .GetRepository<PortfolioEntity>()
                .Get()
                .Include(p => p.DealOperations);
    }

    internal static class PortfolioExtensions
    {
        public static decimal GetCashAmount(this PortfolioEntity portfolio)
            => portfolio.DepositOperations.Sum(o => o.CashChange) + portfolio.DealOperations.Sum(o => o.CashChange);

        // public static List<PortfolioOperation> ToPortfolioHistory(this IEnumerable<DealOperationEntity> operations)
        // {
        //     var result = new List<PortfolioOperation>();
        //     var appliedOperations = new List<DealOperationEntity>();
        //
        //     foreach (var operation in operations)
        //     {
        //         appliedOperations.Add(operation);
        //
        //         result.Add(new PortfolioOperation
        //         {
        //             MomentUtc = operation.MomentUtc,
        //             PortfolioOperationType = operation.PortfolioOperationType,
        //             AssetId = operation.AssetId,
        //             AssetsLotsChange = operation.AssetUnitsCountChange,
        //             AssetDealPrice = operation.AssetUnitsCountChange,
        //             CashChange = operation.CashChange,
        //             PortfolioStateAfterOperation = GetPortfolioState(appliedOperations)
        //         });
        //     }
        //
        //     return result;
        // }

        public static PortfolioStateDto GetPortfolioState(this PortfolioEntity portfolio)
            => new()
            {
                CashAmount = portfolio.GetCashAmount(),
                Assets = portfolio.DealOperations
                    .GroupBy(o => o.AssetId)
                    .Select(g => new AssetModel
                    {
                        AssetId = g.Key,
                        Count = g.Sum(o => o.AssetUnitsCountChange)
                    })
                    .ToList()
            };
    }
}