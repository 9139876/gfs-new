using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using GFS.QuotesService.BL.QuotesProviderAdapters.Abstraction;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IInvestingComAdapter : IQuotesProviderAdapter
{
}

public class InvestingComAdapter : QuotesProviderAbstractAdapter, IInvestingComAdapter
{
    protected override Task<QuoteModel> GetFirstQuote(string getQuotesRequest, TimeFrameEnum timeFrame)
    {
        throw new NotImplementedException();
    }

    protected override Task<IEnumerable<QuoteModel>> GetQuotesBatchInternal(string getQuotesRequest, TimeFrameEnum timeFrame, QuoteModel? lastQuote)
    {
        throw new NotImplementedException();
    }

    protected override TimeFrameEnum[] NativeSupportedTimeFrames => throw new NotImplementedException();
    
    public override string GenerateCommonGetQuotesRequest()
    {
        throw new NotImplementedException();
    }
}