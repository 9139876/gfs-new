using GFS.QuotesService.BL.Models;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IInvestingComAdapter : IQuotesProviderAdapter
{
}

public class InvestingComAdapter : IInvestingComAdapter
{
    public Task<List<InitialModel>> GetInitialData()
    {
        throw new NotImplementedException("This is not the Main adapter");
    }
}