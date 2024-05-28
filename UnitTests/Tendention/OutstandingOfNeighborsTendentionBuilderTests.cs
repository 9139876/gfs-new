using System;
using GFS.AnalysisSystem.Library.Tendention.Models.BuildTendentionContexts;
using GFS.AnalysisSystem.Library.Tendention.TendentionBuilders;
using Xunit;

namespace UnitTests.Tendention;

public class OutstandingOfNeighborsTendentionBuilderTests
{
    [Fact]
    public void ValidationTests()
    {
        Assert.Throws<InvalidOperationException>(() => new OutstandingOfNeighborsTendentionBuilder(null!, null!));

        // var duplicateQuotes = TendentionTestsData.SimpleQuotesCollection_FirstUp.Append(TendentionTestsData.SimpleQuotesCollection_FirstUp.First()); 
        // Assert.Throws<InvalidOperationException>(() => new TendentionBuilder(duplicateQuotes));
    }

    [Fact]
    public void TopAndBottom_ByOneEach_Test()
    {
        var builder = new OutstandingOfNeighborsTendentionBuilder(new BuildOutstandingOfNeighborsTendentionContext { ByNeighborsType = ByNeighborsType.ByOneEach },
            TendentionTestsData.TopAndBottom);

        CheckInternal(builder.BuildTendention());
    }
    
    [Fact]
    public void TopAndBottom_ByTwoEach_Test()
    {
        var builder = new OutstandingOfNeighborsTendentionBuilder(new BuildOutstandingOfNeighborsTendentionContext { ByNeighborsType = ByNeighborsType.ByTwoEach },
            TendentionTestsData.TopAndBottom);

        CheckInternal(builder.BuildTendention());
    }
    
    [Fact]
    public void TopAndBottom_ByThreeEach_Test()
    {
        var builder = new OutstandingOfNeighborsTendentionBuilder(new BuildOutstandingOfNeighborsTendentionContext { ByNeighborsType = ByNeighborsType.ByThreeEach },
            TendentionTestsData.TopAndBottom);

        CheckInternal(builder.BuildTendention());
    }

    private static void CheckInternal(GFS.GrailCommon.Models.Tendention? tendention)
    {
        Assert.NotNull(tendention);
        Assert.True(tendention!.IsCorrect);

        var getPointsSuccess = tendention.TryGetPoints(out var points);
        
        Assert.True(getPointsSuccess);
        Assert.Equal(2, points.Count);
        Assert.Equal(6, points[0].Point.X);
        Assert.Equal(35, points[0].Point.Y);
        Assert.Equal(9, points[1].Point.X);
        Assert.Equal(10, points[1].Point.Y);
    }
}