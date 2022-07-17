using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFS.EF.Extensions;
using GFS.EF.Repository;
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
        Task<OperationResponseDto> PerformOperation(OperationRequestDto request);
    }

    public class PortfolioService : IPortfolioService
    {
        private readonly IDbContext _dbContext;

        public PortfolioService(
            IDbContext dbContext)
        {
            _dbContext = dbContext;
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
                    OperationType = OperationTypeEnum.Deposit,
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

        public async Task<OperationResponseDto> PerformOperation(OperationRequestDto request)
        {
            var portfolio = await PortfolioWithOperations()
                .Where(p => p.Id == request.PortfolioId)
                .SingleOrFailAsync();

            var (allowed, error) = request.OperationType switch
            {
                OperationTypeEnum.Deposit or OperationTypeEnum.Take => await PerformCashOperation(request, portfolio),
                OperationTypeEnum.Sell or OperationTypeEnum.Buy => await PerformSellBuyOperation(request, portfolio),

                _ => throw new ArgumentOutOfRangeException(nameof(request.OperationType))
            };

            if (!allowed)
                return new OperationResponseDto
                {
                    OperationResult = OperationResultTypeEnum.Fail,
                    ErrorMessage = error
                };

            return new OperationResponseDto
            {
                OperationResult = OperationResultTypeEnum.Success,
                PortfolioInfo = portfolio.ToPortfolioInfo()
            };
        }

        private async Task<(bool allowed, string? error)> PerformCashOperation(OperationRequestDto request, PortfolioEntity portfolio)
        {
            if (request.CashAmount is null or < 0)
                return (false, "Сумма средств не может быть меньше 0");

            if (request.OperationType == OperationTypeEnum.Take && portfolio.Operations.GetCashAmount() < request.CashAmount)
                return (false, "Недостаточно наличных средств в портфеле");

            var cashChange = request.OperationType == OperationTypeEnum.Deposit
                ? request.CashAmount!.Value
                : -request.CashAmount!.Value;

            portfolio.Operations.Add(new OperationEntity
            {
                MomentUtc = request.MomentUtc,
                OperationType = OperationTypeEnum.Deposit,
                CashChange = cashChange
            });

            await _dbContext.SaveChangesAsync();

            return (true, null);
        }

        private async Task<(bool allowed, string? error)> PerformSellBuyOperation(OperationRequestDto request, PortfolioEntity portfolio)
        {
            if (request.AssetIdentifier is null)
                return (false, "Не указан идентификатор актива");

            if (request.AssetLotsCount is null or <= 0)
                return (false, "Количество лотов актива должно быть положительным числом");

            if (request.AssetDealPrice is null or <= 0)
                return (false, "Стоимость актива должна быть положительным числом");

            var cashChange = request.OperationType == OperationTypeEnum.Buy
                ? -request.AssetDealPrice!.Value * request.AssetLotsCount!.Value
                : request.AssetDealPrice!.Value * request.AssetLotsCount!.Value;

            var assetLotsChange = request.OperationType == OperationTypeEnum.Buy
                ? request.AssetLotsCount
                : -request.AssetLotsCount;

            if (portfolio.Operations.GetCashAmount() + cashChange < 0)
                return (false, "Недостаточно наличных средств в портфеле");

            portfolio.Operations.Add(new OperationEntity
            {
                MomentUtc = request.MomentUtc,
                OperationType = request.OperationType,
                AssetIdentifier = request.AssetIdentifier,
                AssetDealPrice = request.AssetDealPrice,
                AssetLotsChange = assetLotsChange,
                CashChange = cashChange
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
                    OperationType = operation.OperationType,
                    AssetIdentifier = operation.AssetIdentifier,
                    AssetsLotsChange = operation.AssetLotsChange,
                    AssetDealPrice = operation.AssetLotsChange,
                    CashChange = operation.CashChange,
                    PortfolioStateAfterOperation = GetPortfolioState(appliedOperations)
                });
            }

            return result;
        }

        private static PortfolioStateDto GetPortfolioState(List<OperationEntity> operations)
            => new PortfolioStateDto
            {
                CashAmount = operations.GetCashAmount(),
                Assets = operations
                    .Where(o => o.AssetIdentifier != null)
                    .GroupBy(o => o.AssetIdentifier)
                    .Select(g => new AssetModel
                    {
                        AssetIdentifier = g.Key!,
                        Count = g.Sum(o => o.AssetLotsChange!.Value)
                    })
                    .ToList()
            };
    }
}