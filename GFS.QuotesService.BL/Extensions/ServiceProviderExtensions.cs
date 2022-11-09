using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.QuotesProviderAdapters;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.QuotesService.BL.Extensions;

internal static class ServiceProviderExtensions
{
    public static IQuotesProviderAdapter GetQuotesProviderAdapter(this IServiceProvider serviceProvider, QuotesProviderTypeEnum type)
        => type switch
        {
            QuotesProviderTypeEnum.Tinkoff => serviceProvider.GetRequiredService<ITinkoffAdapter>(),
            QuotesProviderTypeEnum.Finam => serviceProvider.GetRequiredService<IFinamAdapter>(),
            QuotesProviderTypeEnum.BcsExpress => serviceProvider.GetRequiredService<IBcsExpressAdapter>(),
            QuotesProviderTypeEnum.InvestingCom => serviceProvider.GetRequiredService<IInvestingComAdapter>(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    /// <summary> Возвращает адаптер основного поставщика котировок - остальные используются для контроля качества котировок и ориентируются на данные основного </summary>
    public static IQuotesProviderAdapter GetMainQuotesProviderAdapter(this IServiceProvider serviceProvider)
        => serviceProvider.GetRequiredService<ITinkoffAdapter>();
}