using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;
using GFS.QuotesService.DAL.Entities;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IFinamAdapter : IQuotesProviderAdapter
{
}

public class FinamAdapter : QuotesProviderAbstractAdapter, IFinamAdapter
{
    protected override Task<IEnumerable<QuoteModel>> GetQuotesBatchInternal(AssetEntity asset, TimeFrameEnum timeFrame,  DateTime lastQuoteDate)
    {
        throw new NotImplementedException();
    }

    protected override TimeFrameEnum[] NativeSupportedTimeFrames => throw new NotImplementedException();
}