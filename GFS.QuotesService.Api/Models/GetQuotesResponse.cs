using GFS.GrailCommon.Models;

namespace GFS.QuotesService.Api.Models;

public class GetQuotesResponse
{
    public List<QuoteModel> Quotes { get; init; } = new();
}