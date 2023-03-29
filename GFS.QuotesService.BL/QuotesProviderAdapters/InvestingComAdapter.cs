using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.DAL.Entities;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IInvestingComAdapter : IQuotesProviderAdapter
{
}

public class InvestingComAdapter : QuotesProviderAbstractAdapter, IInvestingComAdapter
{
    protected override Task<IEnumerable<QuoteModel>> GetQuotesBatchInternal(AssetEntity asset, TimeFrameEnum timeFrame,  DateTime batchEndDate)
    {
        throw new NotImplementedException();
    }

    public override TimeFrameEnum[] NativeSupportedTimeFrames => new[]
    {
        TimeFrameEnum.H1,
        TimeFrameEnum.D1,
        TimeFrameEnum.W1,
        TimeFrameEnum.M1
    };
}