using System;
using GFS.GrailCommon.Models;

namespace UnitTests.Tendention;

public static class TendentionTestsData
{
    public static PriceTimePoint[] CorrectTendentionFourPoints => new[]
    {
        new PriceTimePoint { Date = ZeroDateTime.AddDays(0), Price = 10 },
        new PriceTimePoint { Date = ZeroDateTime.AddDays(1), Price = 40 },
        new PriceTimePoint { Date = ZeroDateTime.AddDays(2), Price = 20 },
        new PriceTimePoint { Date = ZeroDateTime.AddDays(3), Price = 50 }
    };

    // ReSharper disable once InconsistentNaming
    public static QuoteModel[] SimpleQuotesCollection_FirstUp => new[]
    {
        new QuoteModel{Date = ZeroDateTime.AddDays(0), Low = 10, High = 20},
        new QuoteModel{Date = ZeroDateTime.AddDays(1), Low = 15, High = 25},
        new QuoteModel{Date = ZeroDateTime.AddDays(2), Low = 20, High = 30},
        new QuoteModel{Date = ZeroDateTime.AddDays(3), Low = 25, High = 35},
        new QuoteModel{Date = ZeroDateTime.AddDays(4), Low = 20, High = 30},
        new QuoteModel{Date = ZeroDateTime.AddDays(5), Low = 15, High = 25},
        new QuoteModel{Date = ZeroDateTime.AddDays(6), Low = 10, High = 20},
        new QuoteModel{Date = ZeroDateTime.AddDays(7), Low = 15, High = 25},
        new QuoteModel{Date = ZeroDateTime.AddDays(8), Low = 20, High = 30},
        new QuoteModel{Date = ZeroDateTime.AddDays(9), Low = 25, High = 35},
    };

    // ReSharper disable once InconsistentNaming
    public static PriceTimePoint[] SimpleQuotesCollectionResult_FirstUp => new[]
    {
        new PriceTimePoint { Date = ZeroDateTime.AddDays(0), Price = 10 },
        new PriceTimePoint { Date = ZeroDateTime.AddDays(3), Price = 35 },
        new PriceTimePoint { Date = ZeroDateTime.AddDays(6), Price = 10 }
    };
    
    // ReSharper disable once InconsistentNaming
    public static QuoteModel[] SimpleQuotesCollection_FirstDown => new[]
    {
        new QuoteModel{Date = ZeroDateTime.AddDays(0), Low = 25, High = 35},
        new QuoteModel{Date = ZeroDateTime.AddDays(1), Low = 20, High = 30},
        new QuoteModel{Date = ZeroDateTime.AddDays(2), Low = 15, High = 25},
        new QuoteModel{Date = ZeroDateTime.AddDays(3), Low = 10, High = 20},
        new QuoteModel{Date = ZeroDateTime.AddDays(4), Low = 15, High = 25},
        new QuoteModel{Date = ZeroDateTime.AddDays(5), Low = 20, High = 30},
        new QuoteModel{Date = ZeroDateTime.AddDays(6), Low = 25, High = 35},
        new QuoteModel{Date = ZeroDateTime.AddDays(7), Low = 20, High = 30},
        new QuoteModel{Date = ZeroDateTime.AddDays(8), Low = 15, High = 25},
        new QuoteModel{Date = ZeroDateTime.AddDays(9), Low = 10, High = 20},
    };

    // ReSharper disable once InconsistentNaming
    public static PriceTimePoint[] SimpleQuotesCollectionResult_FirstDown => new[]
    {
        new PriceTimePoint { Date = ZeroDateTime.AddDays(0), Price = 35 },
        new PriceTimePoint { Date = ZeroDateTime.AddDays(3), Price = 10 },
        new PriceTimePoint { Date = ZeroDateTime.AddDays(6), Price = 35 }
    };
    
    private static DateTime ZeroDateTime => new DateTime(1990, 1, 1);
}