using GFS.Common.Exceptions;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Enum;
using GFS.QuotesService.BL.Models.Adapters;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

internal interface IInvestingComAdapter : IQuotesProviderAdapter
{
}

internal class InvestingComAdapter : QuotesProviderAbstractAdapter, IInvestingComAdapter
{
    protected override Task<GetQuotesBatchAdapterResponseModel> GetQuotesBatchInternal(GetQuotesDateBatchAdapterRequestModel request)
    {
        throw new NotImplementedYetException();
    }

    public override ICollection<TimeFrameEnum> NativeSupportedTimeFrames => new[]
    {
        TimeFrameEnum.H1,
        TimeFrameEnum.D1,
        TimeFrameEnum.W1,
        TimeFrameEnum.M1
    };

    public override QuotesProviderTypeEnum ProviderType => QuotesProviderTypeEnum.InvestingCom;
}