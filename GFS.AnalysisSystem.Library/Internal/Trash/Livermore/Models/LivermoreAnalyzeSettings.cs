using GFS.AnalysisSystem.Library.Internal.Trash.Livermore.Enum;

namespace GFS.AnalysisSystem.Library.Internal.Trash.Livermore.Models;

internal class LivermoreAnalyzeSettings
{
    public QuoteToPriceConverterTypeEnum QuoteToPriceConverterType { get; init; } = QuoteToPriceConverterTypeEnum.Close;
    public decimal KPrice { get; init; } = 1;

    public PriceComparisonTypeEnum PriceComparisonType { get; init; } = PriceComparisonTypeEnum.Percentage;
}