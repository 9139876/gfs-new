using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.DAL.Entities;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IBcsExpressAdapter : IQuotesProviderAdapter
{
}

public class BcsExpressAdapter : QuotesProviderAbstractAdapter, IBcsExpressAdapter
{

    protected override Task<IEnumerable<QuoteModel>> GetQuotesBatchInternal(AssetEntity asset, TimeFrameEnum timeFrame,  DateTime lastQuoteDate)
    {
        throw new NotImplementedException();
    }

    public override TimeFrameEnum[] NativeSupportedTimeFrames => new[] { TimeFrameEnum.min1, TimeFrameEnum.H1, TimeFrameEnum.D1 };
}