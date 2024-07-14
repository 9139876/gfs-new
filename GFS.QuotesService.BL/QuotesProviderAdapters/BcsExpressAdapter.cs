using GFS.Common.Exceptions;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.Models.Adapters;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

internal interface IBcsExpressAdapter : IQuotesProviderAdapter
{
}

internal class BcsExpressAdapter : QuotesProviderAbstractAdapter, IBcsExpressAdapter
{
    protected override Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatchInternal(GetQuotesDateBatchAdapterRequestModel request)
    {
        throw new NotImplementedYetException();
    }

    public override ICollection<TimeFrameEnum> NativeSupportedTimeFrames => new[] { TimeFrameEnum.min1, TimeFrameEnum.H1, TimeFrameEnum.D1 };

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.BcsExpress;
}