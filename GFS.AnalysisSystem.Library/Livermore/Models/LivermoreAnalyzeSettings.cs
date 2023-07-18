using GFS.AnalysisSystem.Library.Livermore.Enum;

namespace GFS.AnalysisSystem.Library.Livermore.Models;

public class LivermoreAnalyzeSettings
{
    public QuoteToPriceConverterTypeEnum QuoteToPriceConverterType { get; init; } = QuoteToPriceConverterTypeEnum.Close;
    public decimal KPrice { get; init; } = 1;
}