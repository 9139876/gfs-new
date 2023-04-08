using GFS.Common.Exceptions;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IBcsExpressAdapter : IQuotesProviderAdapter
{
}

internal class BcsExpressAdapter : QuotesProviderAbstractAdapter, IBcsExpressAdapter
{
    protected override Task<GetQuotesBatchResponseModel> GetQuotesBatchInternal(GetQuotesBatchRequestModel request)
    {
        throw new NotImplementedYetException();
    }

    public override TimeFrameEnum[] NativeSupportedTimeFrames => new[] { TimeFrameEnum.min1, TimeFrameEnum.H1, TimeFrameEnum.D1 };

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.BcsExpress;
}