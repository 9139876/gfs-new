using GFS.GrailCommon.Models;

namespace UnitTests.Tendention;

public static class TendentionTestsData
{
    public static PriceTimePointInCells[] CorrectTendentionFourPoints => new[]
    {
        new PriceTimePointInCells { X = ZeroDateTime + 0, Y = 10 },
        new PriceTimePointInCells { X = ZeroDateTime + 1, Y = 40 },
        new PriceTimePointInCells { X = ZeroDateTime + 2, Y = 20 },
        new PriceTimePointInCells { X = ZeroDateTime + 3, Y = 50 }
    };

    public static CandleInCells[] TopAndBottom => new[]
    {
        new CandleInCells { Date = ZeroDateTime + 3, Low = 10, High = 20 },
        new CandleInCells { Date = ZeroDateTime + 4, Low = 15, High = 25 },
        new CandleInCells { Date = ZeroDateTime + 5, Low = 20, High = 30 },
        new CandleInCells { Date = ZeroDateTime + 6, Low = 25, High = 35 },
        new CandleInCells { Date = ZeroDateTime + 7, Low = 20, High = 30 },
        new CandleInCells { Date = ZeroDateTime + 8, Low = 15, High = 25 },
        new CandleInCells { Date = ZeroDateTime + 9, Low = 10, High = 20 },
        new CandleInCells { Date = ZeroDateTime + 10, Low = 15, High = 25 },
        new CandleInCells { Date = ZeroDateTime + 11, Low = 20, High = 30 },
        new CandleInCells { Date = ZeroDateTime + 12, Low = 25, High = 35 },
    };
    
    // ReSharper disable once InconsistentNaming
    public static CandleInCells[] SimpleQuotesCollection_FirstUp => new[]
    {
        new CandleInCells { Date = ZeroDateTime + 0, Low = 10, High = 20 },
        new CandleInCells { Date = ZeroDateTime + 1, Low = 15, High = 25 },
        new CandleInCells { Date = ZeroDateTime + 2, Low = 20, High = 30 },
        new CandleInCells { Date = ZeroDateTime + 3, Low = 25, High = 35 },
        new CandleInCells { Date = ZeroDateTime + 4, Low = 20, High = 30 },
        new CandleInCells { Date = ZeroDateTime + 5, Low = 15, High = 25 },
        new CandleInCells { Date = ZeroDateTime + 6, Low = 10, High = 20 },
        new CandleInCells { Date = ZeroDateTime + 7, Low = 15, High = 25 },
        new CandleInCells { Date = ZeroDateTime + 8, Low = 20, High = 30 },
        new CandleInCells { Date = ZeroDateTime + 9, Low = 25, High = 35 },
    };

    // ReSharper disable once InconsistentNaming
    public static PriceTimePointInCells[] SimpleQuotesCollectionResult_FirstUp => new[]
    {
        new PriceTimePointInCells { X = ZeroDateTime + 0, Y = 10 },
        new PriceTimePointInCells { X = ZeroDateTime + 3, Y = 35 },
        new PriceTimePointInCells { X = ZeroDateTime + 6, Y = 10 }
    };

    // ReSharper disable once InconsistentNaming
    public static CandleInCells[] SimpleQuotesCollection_FirstDown => new[]
    {
        new CandleInCells { Date = ZeroDateTime + 0, Low = 25, High = 35 },
        new CandleInCells { Date = ZeroDateTime + 1, Low = 20, High = 30 },
        new CandleInCells { Date = ZeroDateTime + 2, Low = 15, High = 25 },
        new CandleInCells { Date = ZeroDateTime + 3, Low = 10, High = 20 },
        new CandleInCells { Date = ZeroDateTime + 4, Low = 15, High = 25 },
        new CandleInCells { Date = ZeroDateTime + 5, Low = 20, High = 30 },
        new CandleInCells { Date = ZeroDateTime + 6, Low = 25, High = 35 },
        new CandleInCells { Date = ZeroDateTime + 7, Low = 20, High = 30 },
        new CandleInCells { Date = ZeroDateTime + 8, Low = 15, High = 25 },
        new CandleInCells { Date = ZeroDateTime + 9, Low = 10, High = 20 },
    };

    // ReSharper disable once InconsistentNaming
    public static PriceTimePointInCells[] SimpleQuotesCollectionResult_FirstDown => new[]
    {
        new PriceTimePointInCells { X = ZeroDateTime + 0, Y = 35 },
        new PriceTimePointInCells { X = ZeroDateTime + 3, Y = 10 },
        new PriceTimePointInCells { X = ZeroDateTime + 6, Y = 35 }
    };

    private static int ZeroDateTime => 0;
}