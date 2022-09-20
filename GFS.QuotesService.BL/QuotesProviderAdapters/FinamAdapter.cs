using GFS.QuotesService.BL.Models;

namespace GFS.QuotesService.BL.QuotesProviderAdapters;

public interface IFinamAdapter :IQuotesProviderAdapter
{
}

public class FinamAdapter : IFinamAdapter
{
    public Task<List<InitialModel>> GetInitialData()
    {
        throw new NotImplementedException("This is not the Main adapter");
    }
}