using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IFinamAdapter : IQuotesProviderAdapter
{
}

internal class FinamAdapter : QuotesProviderAbstractAdapter, IFinamAdapter
{
    protected override Task<GetQuotesBatchResponseModel> GetQuotesBatchInternal(GetQuotesBatchRequestModel request)
    {
        throw new NotImplementedException();
    }

    public override TimeFrameEnum[] NativeSupportedTimeFrames => throw new NotImplementedException();

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.Finam;
}