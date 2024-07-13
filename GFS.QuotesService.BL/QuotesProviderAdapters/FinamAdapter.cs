using GFS.Common.Exceptions;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.Models.Adapters;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

internal interface IFinamAdapter : IQuotesProviderAdapter
{
}

internal class FinamAdapter : QuotesProviderAbstractAdapter, IFinamAdapter
{
    protected override Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatchInternal(GetQuotesDateBatchAdapterRequestModel request)
    {
        throw new NotImplementedYetException();
    }

    public override ICollection<TimeFrameEnum> NativeSupportedTimeFrames => throw new NotImplementedYetException();

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.Finam;
}