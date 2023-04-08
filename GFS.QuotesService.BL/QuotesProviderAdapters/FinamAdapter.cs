using GFS.Common.Exceptions;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IFinamAdapter : IQuotesProviderAdapter
{
}

internal class FinamAdapter : QuotesProviderAbstractAdapter, IFinamAdapter
{
    protected override Task<GetQuotesBatchResponseModel> GetQuotesBatchInternal(GetQuotesBatchRequestModel request)
    {
        throw new NotImplementedYetException();
    }

    public override TimeFrameEnum[] NativeSupportedTimeFrames => throw new NotImplementedYetException();

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.Finam;
}