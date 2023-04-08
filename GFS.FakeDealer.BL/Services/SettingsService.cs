using GFS.Common.Attributes;
using GFS.FakeDealer.Api.Enums;
using GFS.FakeDealer.Api.Models;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Common.Enum;

namespace GFS.FakeDealer.BL.Services;

[SingletonRegistration]
public interface ISettingsService
{
    void SetDealerSettings(DealerSettings request);
    DealerSettings GetDealerSettings();
    decimal GetDealerCommission(decimal cashAmount);

    Func<QuoteModel, decimal> GetDealPriceCalculator(DealerOperationTypeEnum operationType);
}

internal class SettingsService : ISettingsService
{
    private DealerSettings _settings;

    public SettingsService()
    {
        _settings = GetDefaultSettings();
    }

    public void SetDealerSettings(DealerSettings request)
        => _settings = request;

    public DealerSettings GetDealerSettings()
        => _settings;

    public Func<QuoteModel, decimal> GetDealPriceCalculator(DealerOperationTypeEnum operationType)
        => _settings.DealPriceCalcBehavior switch
        {
            DealPriceCalcBehaviorEnum.MedianHiLow => quote => (quote.High + quote.Low) / 2,
            DealPriceCalcBehaviorEnum.Worst => quote => operationType == DealerOperationTypeEnum.Buy ? quote.High : quote.Low,
            DealPriceCalcBehaviorEnum.Close => quote => quote.Close,
            _ => throw new ArgumentOutOfRangeException(nameof(_settings.DealPriceCalcBehavior), $"Value = {_settings.DealPriceCalcBehavior}")
        };

    public decimal GetDealerCommission(decimal cashAmount)
        => _settings.DealerCommission.CommissionType switch
        {
            DealerCommissionType.Fix => _settings.DealerCommission.Value,
            DealerCommissionType.PercentOfDealSum => Math.Abs(cashAmount) * _settings.DealerCommission.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(_settings.DealerCommission.CommissionType), $"Value = {_settings.DealerCommission.CommissionType}")
        };


    private static DealerSettings GetDefaultSettings()
        => new()
        {
            QuotesProviderType = QuotesProviderTypeEnum.Tinkoff,
            DealPriceCalcBehavior = DealPriceCalcBehaviorEnum.Close,
            TimeFrame = TimeFrameEnum.D1,
            DealerCommission = new DealerCommission
            {
                CommissionType = DealerCommissionType.Fix,
                Value = 0
            }
        };
}