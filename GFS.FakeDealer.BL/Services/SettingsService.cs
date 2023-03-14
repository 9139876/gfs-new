using GFS.Common.Attributes;
using GFS.FakeDealer.Api.Enums;
using GFS.FakeDealer.Api.Models;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.FakeDealer.BL.Services;

[SingletonRegistration]
public interface ISettingsService
{
    void SetDealerSettings(DealerSettings request);
    DealerSettings GetDealerSettings();

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

    private static DealerSettings GetDefaultSettings()
        => new()
        {
            QuotesProviderType = QuotesProviderTypeEnum.Tinkoff,
            DealPriceCalcBehavior = DealPriceCalcBehaviorEnum.Close,
            TimeFrame = TimeFrameEnum.D1
        };
}