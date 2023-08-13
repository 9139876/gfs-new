using System;
using GFS.GrailCommon.Models;

namespace UnitTests.Tendention;

public static class TendentionTestsStubs
{
    public static PriceTimePoint[] CorrectTendentionFourPoints => new[]
    {
        new PriceTimePoint { Date = new DateTime(1990, 1, 1), Price = 10 },
        new PriceTimePoint { Date = new DateTime(1990, 1, 2), Price = 40 },
        new PriceTimePoint { Date = new DateTime(1990, 1, 3), Price = 20 },
        new PriceTimePoint { Date = new DateTime(1990, 1, 4), Price = 50 }
    };
}