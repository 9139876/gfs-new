using GFS.Api.Client.Services;
using GFS.EF.Extensions;
using GFS.EF.Repository;
using GFS.FakeDealer.Api.Enums;
using GFS.FakeDealer.Api.Interfaces;
using GFS.FakeDealer.Api.Models;
using GFS.Portfolio.Api.Enums;
using GFS.Portfolio.Api.Models;
using GFS.Portfolio.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFS.Portfolio.BL.Services
{
    public interface IPortfolioService
    {
        Task<PortfolioInfoDto> CreatePortfolio(CreatePortfolioRequestDto request);
        Task DeletePortfolio(DeletePortfolioRequestDto request);
        Task<PortfolioInfoDto> GetPortfolioInfo(GetPortfolioInfoRequestDto request);
        Task<List<PortfolioInfoDto>> GetAllPortfoliosInfo();
        Task<PortfolioInfoWithHistoryDto> GetPortfolioInfoWithHistory(GetPortfolioInfoRequestDto request);
        Task<OperationResponseDto> PerformOperation(PortfolioOperationRequestDto request);
    }

    public class PortfolioService : IPortfolioService
    {
        private readonly IDbContext _dbContext;
        private readonly IRemoteApiClient _remoteApiClient;

        public PortfolioService(
            IDbContext dbContext,
            IRemoteApiClient remoteApiClient)
        {
            _dbContext = dbContext;
            _remoteApiClient = remoteApiClient;
        }

        public async Task<PortfolioInfoDto> CreatePortfolio(CreatePortfolioRequestDto request)
        {
            if (request.CashAmount < 0)
                throw new InvalidOperationException("Сумма вносимых денежных средств не может быть меньше 0");

            var portfolio = new PortfolioEntity
            {
                Name = request.Name
            };

            if (request.CashAmount > 0)
                portfolio.Operations.Add(new OperationEntity
                {
                    MomentUtc = request.MomentUtc,
                    PortfolioOperationType = PortfolioOperationTypeEnum.Deposit,
                    CashChange = request.CashAmount
                });

            _dbContext.GetRepository<PortfolioEntity>().Insert(portfolio);
            await _dbContext.SaveChangesAsync();

            return portfolio.ToPortfolioInfo();
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

            return portfolio.ToPortfolioInfo();
        }

        public async Task<List<PortfolioInfoDto>> GetAllPortfoliosInfo()
        {
            var portfolios = await PortfolioWithOperations()
                .AsNoTracking()
                .ToListAsync();

            return portfolios.Select(p => p.ToPortfolioInfo()).ToList();
        }

        public async Task<PortfolioInfoWithHistoryDto> GetPortfolioInfoWithHistory(GetPortfolioInfoRequestDto request)
        {
            var portfolio = await PortfolioWithOperations()
                .Where(p => p.Id == request.PortfolioId)
                .Include(p => p.Operations)
                .AsNoTracking()
                .SingleOrFailAsync();

            return new PortfolioInfoWithHistoryDto
            {
                PortfolioId = portfolio.Id,
                Name = portfolio.Name,
                PortfolioState = portfolio.ToPortfolioInfo().PortfolioState,
                History = portfolio.Operations.OrderBy(o => o.MomentUtc).ToPortfolioHistory()
            };
        }

        public async Task<OperationResponseDto> PerformOperation(PortfolioOperationRequestDto request)
        {
            var portfolio = await PortfolioWithOperations()
                .Where(p => p.Id == request.PortfolioId)
                .SingleOrFailAsync();

            var (allowed, error) = request.PortfolioOperationType switch
            {
                PortfolioOperationTypeEnum.Deposit or PortfolioOperationTypeEnum.Take => await PerformCashOperation(request, portfolio),
                PortfolioOperationTypeEnum.Sell or PortfolioOperationTypeEnum.Buy => await PerformSellBuyOperation(request, portfolio),

                _ => throw new ArgumentOutOfRangeException(nameof(request.PortfolioOperationType))
            };

            if (!allowed)
                return new OperationResponseDto
                {
                    PortfolioOperationResult = PortfolioOperationResultTypeEnum.Fail,
                    ErrorMessage = error
                };

            return new OperationResponseDto
            {
                PortfolioOperationResult = PortfolioOperationResultTypeEnum.Success,
                PortfolioInfo = portfolio.ToPortfolioInfo()
            };
        }

        private async Task<(bool allowed, string? error)> PerformCashOperation(PortfolioOperationRequestDto request, PortfolioEntity portfolio)
        {
            if (request.CashAmount is null or < 0)
                return (false, "Сумма средств не может быть меньше 0");

            if (request.PortfolioOperationType == PortfolioOperationTypeEnum.Take && portfolio.Operations.GetCashAmount() < request.CashAmount)
                return (false, "Недостаточно наличных средств в портфеле");

            var cashChange = request.PortfolioOperationType == PortfolioOperationTypeEnum.Deposit
                ? request.CashAmount!.Value
                : -request.CashAmount!.Value;

            portfolio.Operations.Add(new OperationEntity
            {
                MomentUtc = request.MomentUtc,
                PortfolioOperationType = PortfolioOperationTypeEnum.Deposit,
                CashChange = cashChange
            });

            await _dbContext.SaveChangesAsync();

            return (true, null);
        }

        private async Task<(bool allowed, string? error)> PerformSellBuyOperation(PortfolioOperationRequestDto request, PortfolioEntity portfolio)
        {
            if (string.IsNullOrWhiteSpace(request.FIGI) || !request.AssetId.HasValue)
                return (false, "Не указан идентификатор инструмента");

            if (request.AssetUnitsCount is null or <= 0)
                return (false, "Количество единиц инструмента должно быть положительным числом");

            var dealRequest = new MakeDealRequest
            {
                FIGI = request.FIGI,
                AssetUnitsCount = request.AssetUnitsCount!.Value,
                DealDateUtc = request.MomentUtc,
                OperationType = request.PortfolioOperationType == PortfolioOperationTypeEnum.Buy ? DealerOperationTypeEnum.Buy: DealerOperationTypeEnum.Sell
            };

            var dealDetails = await _remoteApiClient.Call<MakeDeal, MakeDealRequest, MakeDealResponse>(dealRequest);

            if (portfolio.Operations.GetCashAmount() + dealDetails.DeltaCash < 0)
                return (false, "Недостаточно наличных средств в портфеле");

            portfolio.Operations.Add(new OperationEntity
            {
                MomentUtc = request.MomentUtc,
                PortfolioOperationType = request.PortfolioOperationType,
                AssetId = request.AssetId,
                AssetDealPrice = dealDetails.DealPrice,
                AssetUnitsCountChange = dealDetails.DeltaAsset,
                CashChange = dealDetails.DeltaCash
            });

            await _dbContext.SaveChangesAsync();

            return (true, null);
        }

        private IQueryable<PortfolioEntity> PortfolioWithOperations()
            => _dbContext
                .GetRepository<PortfolioEntity>()
                .Get()
                .Include(p => p.Operations);
    }

    internal static class Extensions
    {
        public static PortfolioInfoDto ToPortfolioInfo(this PortfolioEntity portfolio)
            => new PortfolioInfoDto
            {
                PortfolioId = portfolio.Id,
                Name = portfolio.Name,
                PortfolioState = GetPortfolioState(portfolio.Operations)
            };

        public static decimal GetCashAmount(this IEnumerable<OperationEntity> operations)
            => operations.Sum(o => o.CashChange);

        public static List<PortfolioOperation> ToPortfolioHistory(this IEnumerable<OperationEntity> operations)
        {
            var result = new List<PortfolioOperation>();
            var appliedOperations = new List<OperationEntity>();

            foreach (var operation in operations)
            {
                appliedOperations.Add(operation);

                result.Add(new PortfolioOperation
                {
                    MomentUtc = operation.MomentUtc,
                    PortfolioOperationType = operation.PortfolioOperationType,
                    AssetId = operation.AssetId,
                    AssetsLotsChange = operation.AssetUnitsCountChange,
                    AssetDealPrice = operation.AssetUnitsCountChange,
                    CashChange = operation.CashChange,
                    PortfolioStateAfterOperation = GetPortfolioState(appliedOperations)
                });
            }

            return result;
        }

        private static PortfolioStateDto GetPortfolioState(List<OperationEntity> operations)
            => new()
            {
                CashAmount = operations.GetCashAmount(),
                Assets = operations
                    .Where(o => o.AssetId != null)
                    .GroupBy(o => o.AssetId)
                    .Select(g => new AssetModel
                    {
                        AssetId = g.Key!.Value,
                        Count = g.Sum(o => o.AssetUnitsCountChange!.Value)
                    })
                    .ToList()
            };
    }
}