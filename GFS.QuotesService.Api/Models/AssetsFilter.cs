namespace GFS.QuotesService.Api.Models;

public class AssetsFilter
{
    public Guid? AssetId { get; init; }

    public string? NameFilter { get; init; }

    public string? FIGI { get; init; }

    public bool OnlyHasQuotes { get; init; }
}