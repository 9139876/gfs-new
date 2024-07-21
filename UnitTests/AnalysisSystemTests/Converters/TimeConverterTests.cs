using System;
using System.Linq;
using System.Threading.Tasks;
using GFS.AnalysisSystem.Library.Internal.TimeConverter;
using GFS.GrailCommon.Enums;
using Xunit;

namespace UnitTests.AnalysisSystemTests.Converters;

public class TimeConverterTests
{
    [Fact]
    public void Smoke_Test()
    {
        void AssertAction(TimeSpan ts) => Assert.True(ts.TotalMilliseconds >= 0);

        foreach (var unitOfTime in Enum.GetValues<UnitOfTime>())
        {
            AssertAction(TimeConverter.GetTimeSpan(unitOfTime, decimal.Zero));
            AssertAction(TimeConverter.GetTimeSpan(unitOfTime, decimal.MaxValue));
            AssertAction(TimeConverter.GetTimeSpan(unitOfTime, decimal.MinValue));
            AssertAction(TimeConverter.GetTimeSpan(unitOfTime, int.MaxValue));
        }
    }

    [Fact]
    public void ConvertNumber_Test()
    {
        var timeRange = new TimeRange(TimeSpan.FromDays(1), TimeSpan.FromDays(1));

        Assert.Empty(TimeConverter.ConvertNumber(10m, timeRange));
    }

    [Fact]
    public void ConvertTimeSpan_Test()
    {
        var timeRange = new TimeRange(TimeSpan.FromDays(1), TimeSpan.FromDays(100));

        Assert.Throws<InvalidOperationException>(() => TimeConverter.ConvertTimeSpan(TimeSpan.MinValue, timeRange));
        Assert.Empty(TimeConverter.ConvertTimeSpan(TimeSpan.Zero, timeRange));
        Assert.Empty(TimeConverter.ConvertTimeSpan(TimeSpan.FromMinutes(4), timeRange));
        Assert.NotEmpty(TimeConverter.ConvertTimeSpan(TimeSpan.FromMinutes(5), timeRange));

        Parallel.ForEach(Enumerable.Range(1, 1000), item =>
        {
            TimeConverter.ConvertTimeSpan(TimeSpan.FromDays(item), timeRange)
                .Select(x => x.Item2.TimeSpanValue)
                .ToList()
                .ForEach(x => Assert.True(x >= timeRange.MinValue && x <= timeRange.MaxValue));
        });
    }

    [Fact]
    public void TimeRange_Test()
    {
        Assert.Throws<InvalidOperationException>(() => new TimeRange(TimeSpan.MinValue, TimeSpan.MaxValue));
        Assert.Throws<InvalidOperationException>(() => new TimeRange(TimeSpan.Zero, TimeSpan.MaxValue));

        Assert.True(new TimeRange(TimeSpan.FromDays(1), TimeSpan.FromDays(1)).IsZeroLength);

        var timeRange1 = new TimeRange(TimeSpan.FromDays(1), TimeSpan.MaxValue);
        Assert.True(timeRange1.MaxValue >= timeRange1.MinValue);

        var timeRange2 = new TimeRange(TimeSpan.FromDays(1), TimeSpan.FromDays(1));
        Assert.True(timeRange1.MaxValue >= timeRange2.MinValue);

        var timeRange3 = new TimeRange(TimeSpan.MaxValue, TimeSpan.FromDays(1));
        Assert.True(timeRange1.MaxValue >= timeRange3.MinValue);
    }

    [Fact]
    public void TimeSpanToUnitOfTimeByTimeFrame_Test()
    {
        foreach (var timeFrame in Enum.GetValues<TimeFrameEnum>())
        {
            Assert.Throws<InvalidOperationException>(() => TimeConverter.TimeSpanToUnitOfTimeByTimeFrame(TimeSpan.MinValue, timeFrame));
            Assert.Equal(0, TimeConverter.TimeSpanToUnitOfTimeByTimeFrame(TimeSpan.Zero, timeFrame));
            TimeConverter.TimeSpanToUnitOfTimeByTimeFrame(TimeSpan.MaxValue, timeFrame);
        }

        TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan.FromSeconds(1), TimeFrameEnum.tick);
        TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan.FromMinutes(1), TimeFrameEnum.min1);
        TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan.FromMinutes(4), TimeFrameEnum.min4);
        TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan.FromHours(1), TimeFrameEnum.H1);
        TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan.FromDays(1), TimeFrameEnum.D1);
        TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan.FromDays(7), TimeFrameEnum.W1);
        TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan.FromDays(30), TimeFrameEnum.M1);
        TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan.FromDays(30 * 3), TimeFrameEnum.Seasonly);
        TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan.FromDays(365), TimeFrameEnum.Y1);
    }

    private void TimeSpanToUnitOfTimeByTimeFrame_InnerTest(TimeSpan timeSpan, TimeFrameEnum timeFrame)
    {
        Func<TimeFrameEnum, bool> IsMore = tf => TimeConverter.TimeSpanToUnitOfTimeByTimeFrame(timeSpan, tf) > 0;
        Func<TimeFrameEnum, bool> IsZero = tf => TimeConverter.TimeSpanToUnitOfTimeByTimeFrame(timeSpan, tf) == 0;

        foreach (var tf in Enum.GetValues<TimeFrameEnum>())
            Assert.True(timeFrame >= tf ? IsMore(tf) : IsZero(tf));
    }
}