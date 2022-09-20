using GFS.QuotesService.BL.Models;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IBcsExpressAdapter : IQuotesProviderAdapter
{
}

public class BcsExpressAdapter : IBcsExpressAdapter
{
    public Task<List<InitialModel>> GetInitialData()
    {
        throw new NotImplementedException("This is not the Main adapter");
    }
}