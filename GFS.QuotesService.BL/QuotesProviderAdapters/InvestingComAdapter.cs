using GFS.Common.Exceptions;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.BL.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IInvestingComAdapter : IQuotesProviderAdapter
{
}

internal class InvestingComAdapter : QuotesProviderAbstractAdapter, IInvestingComAdapter
{
    protected override Task<GetQuotesBatchResponseModel> GetQuotesBatchInternal(GetQuotesBatchRequestModel request)
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