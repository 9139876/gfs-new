using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IBcsExpressAdapter : IQuotesProviderAdapter
{
}

internal class BcsExpressAdapter : QuotesProviderAbstractAdapter, IBcsExpressAdapter
{
    protected override Task<GetQuotesBatchResponseModel> GetQuotesBatchInternal(GetQuotesBatchRequestModel request)
    {
        throw new NotImplementedException();
    }

    public override TimeFrameEnum[] NativeSupportedTimeFrames => new[] { TimeFrameEnum.min1, TimeFrameEnum.H1, TimeFrameEnum.D1 };

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.BcsExpress;
}