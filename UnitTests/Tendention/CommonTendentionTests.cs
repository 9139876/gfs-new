using System.Linq;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;
using Xunit;

namespace UnitTests.Tendention;

public class CommonTendentionTests
{
    [Fact]
    public void NonCorrect_LessTwoPoint()
    {
        var tendention = new GFS.GrailCommon.Models.Tendention();

        Assert.False(tendention.IsCorrect);

        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[0]);
        Assert.False(tendention.IsCorrect);
    }

    [Fact]
    public void Correct_TwoPoint()
    {
        var tendention = new GFS.GrailCommon.Models.Tendention();

        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[0]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[1]);
        Assert.True(tendention.IsCorrect);
    }
    
    [Fact]
    public void NonCorrect_TwoSameMoves()
    {
        var tendention = new GFS.GrailCommon.Models.Tendention();

        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[0]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[1]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[3]);
        Assert.False(tendention.IsCorrect);
    }

    [Fact]
    public void Correct_WrongOrder()
    {
        var tendention = new GFS.GrailCommon.Models.Tendention();

        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[3]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[0]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[2]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[1]);

        Assert.True(tendention.IsCorrect);
    }

    [Fact]
    public void Correct_Move()
    {
        var tendention = new GFS.GrailCommon.Models.Tendention();

        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[0]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[1]);

        var isCorrect = tendention.TryGetMoves(out var moves);

        Assert.True(isCorrect);
        Assert.NotEmpty(moves);
        Assert.Equal(TendentionMoveDirectionTypeEnum.Up, moves.Single().MoveDirectionType);
    }

    [Fact]
    public void Correct_NextMoveDirection()
    {
        var tendention = new GFS.GrailCommon.Models.Tendention();

        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[0]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[1]);

        var isCorrect = tendention.TryGetNextDirection(out var nextDirection);

        Assert.True(isCorrect);
        Assert.Equal(TendentionMoveDirectionTypeEnum.Down, nextDirection);
    }

    [Fact]
    public void Correct_PointsSort()
    {
        var tendention = new GFS.GrailCommon.Models.Tendention();

        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[1]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[0]);

        var isCorrect = tendention.TryGetPoints(out var points);

        Assert.True(isCorrect);
        Assert.NotEmpty(points);
        Assert.Equal(2, points.Count);
        Assert.Equal(TendentionTestsStubs.CorrectTendentionFourPoints[0], points[0].Point);
        Assert.Equal(TendentionTestsStubs.CorrectTendentionFourPoints[1], points[1].Point);
    }

    [Fact]
    public void Correct_PointChange()
    {
        var tendention = new GFS.GrailCommon.Models.Tendention();

        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[1]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[0]);

        var isCorrect = tendention.TryGetNextDirection(out var nextDirection);

        Assert.True(isCorrect);
        Assert.Equal(TendentionMoveDirectionTypeEnum.Down, nextDirection);

        var newPoint = new PriceTimePoint { Date = TendentionTestsStubs.CorrectTendentionFourPoints[0].Date, Price = decimal.MaxValue };
        tendention.AddPoint(newPoint);

        isCorrect = tendention.TryGetNextDirection(out nextDirection);

        Assert.True(isCorrect);
        Assert.Equal(TendentionMoveDirectionTypeEnum.Up, nextDirection);

        isCorrect = tendention.TryGetPoints(out var points);

        Assert.True(isCorrect);
        Assert.NotEmpty(points);
        Assert.Equal(2, points.Count);
        Assert.NotEqual(TendentionTestsStubs.CorrectTendentionFourPoints[0], points[0].Point);
        Assert.Equal(newPoint, points[0].Point);
        Assert.Equal(TendentionTestsStubs.CorrectTendentionFourPoints[1], points[1].Point);
    }
    
    [Fact]
    public void Correct_RemovePoint()
    {
        var tendention = new GFS.GrailCommon.Models.Tendention();

        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[0]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[1]);
        tendention.AddPoint(TendentionTestsStubs.CorrectTendentionFourPoints[2]);

        var isCorrect = tendention.TryGetPoints(out var points);

        Assert.True(isCorrect);
        Assert.NotEmpty(points);
        Assert.Equal(3, points.Count);

        tendention.RemovePoint(TendentionTestsStubs.CorrectTendentionFourPoints[2].Date);
        
        isCorrect = tendention.TryGetPoints(out points);

        Assert.True(isCorrect);
        Assert.NotEmpty(points);
        Assert.Equal(2, points.Count);
        
        Assert.Equal(TendentionTestsStubs.CorrectTendentionFourPoints[0], points[0].Point);
        Assert.Equal(TendentionTestsStubs.CorrectTendentionFourPoints[1], points[1].Point);
    }
}