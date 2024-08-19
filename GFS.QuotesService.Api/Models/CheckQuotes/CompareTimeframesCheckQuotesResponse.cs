using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Enum;

namespace GFS.QuotesService.Api.Models.CheckQuotes;

public class CompareTimeframesCheckQuotesResponse
{
    public CompareTimeframesCheckQuotesResponse(AssetProviderTimeFrame one, AssetProviderTimeFrame two, TimeFrameEnum targetTimeFrame)
    {
        One = one;
        Two = two;
        TargetTimeFrame = targetTimeFrame;
    }

    public AssetProviderTimeFrame One { get; }
    public AssetProviderTimeFrame Two { get; }

    public TimeFrameEnum TargetTimeFrame { get; }

    public DateTime FirstDate { get; set; }

    public DateTime LastDate { get; set; }

    public string? Summary { get; set; }

    public List<CompareQuotesDifferent> Differences { get; } = new();
}

public record AssetProviderTimeFrame(string AssetName, QuotesProviderTypeEnum ProviderType, TimeFrameEnum TimeFrame);

public record CompareQuotesDifferent(DateTime Date, List<CompareQuotesDifferentItem> DifferentItems);

public record CompareQuotesDifferentItem(string PropertyName, string ValueOne, string ValueTwo);