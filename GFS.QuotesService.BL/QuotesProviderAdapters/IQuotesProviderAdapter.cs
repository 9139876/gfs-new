using GFS.QuotesService.Api.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IQuotesProviderAdapter
{
}

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
}