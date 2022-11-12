using GFS.Common.Attributes;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.BL.Models;

namespace GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

public interface IQuotesProviderAdapter
{
    Task<List<InitialModel>> GetInitialData();
    bool IsNativeSupportedTimeframe(TimeFrameEnum timeFrame);
    string GenerateCommonGetQuotesRequest();
    Task<IEnumerable<QuoteModel>> GetQuotesBatch(string getQuotesRequest, TimeFrameEnum timeFrame, QuoteModel? lastQuote);
}

[IgnoreRegistration]
public abstract class QuotesProviderAbstractAdapter : IQuotesProviderAdapter
{
    public virtual Task<List<InitialModel>> GetInitialData()
    {
        throw new NotImplementedException("This is not the Main adapter");
    }

    public bool IsNativeSupportedTimeframe(TimeFrameEnum timeFrame)
        => NativeSupportedTimeFrames.Contains(timeFrame);

    public async Task<IEnumerable<QuoteModel>> GetQuotesBatch(string getQuotesRequest, TimeFrameEnum timeFrame, QuoteModel? lastQuote)
    {
        if (!IsNativeSupportedTimeframe(timeFrame))
            throw new InvalidOperationException($"Timeframe {timeFrame} is not supported {GetType().Name}");

        lastQuote ??= await GetFirstQuote(getQuotesRequest, timeFrame);
        
        return await GetQuotesBatchInternal(getQuotesRequest, timeFrame, lastQuote);
    }

    protected abstract Task<QuoteModel> GetFirstQuote(string getQuotesRequest, TimeFrameEnum timeFrame);
    
    protected abstract Task<IEnumerable<QuoteModel>> GetQuotesBatchInternal(string getQuotesRequest, TimeFrameEnum timeFrame, QuoteModel? lastQuote);
    
    protected abstract TimeFrameEnum[] NativeSupportedTimeFrames { get; }
    
    public abstract string GenerateCommonGetQuotesRequest();
    
}