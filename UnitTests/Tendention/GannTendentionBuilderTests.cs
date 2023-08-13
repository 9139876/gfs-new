using System;
using System.Linq;
using GFS.AnalysisSystem.Library.Gann.Tendention;
using Xunit;

namespace UnitTests.Tendention;

public class GannTendentionBuilderTests
{
    [Fact]
    public void ValidationTests()
    {
        Assert.Throws<InvalidOperationException>(() => new TendentionBuilder(null!));

        var duplicateQuotes = TendentionTestsData.SimpleQuotesCollection_FirstUp.Append(TendentionTestsData.SimpleQuotesCollection_FirstUp.First()); 
        Assert.Throws<InvalidOperationException>(() => new TendentionBuilder(duplicateQuotes));
    }
    
    [Fact]
    public void BuildSimpleTendention_FirstUp()
    {
        var referenceResult = new GFS.GrailCommon.Models.Tendention();
        TendentionTestsData.SimpleQuotesCollectionResult_FirstUp.ToList().ForEach(point => referenceResult.AddPoint(point));

        var builder = new TendentionBuilder(TendentionTestsData.SimpleQuotesCollection_FirstUp);
        var verifiableResult = builder.BuildThreePointsTendention();
        
        Assert.Equal(referenceResult.IsCorrect, verifiableResult.IsCorrect);
    }
    
    [Fact]
    public void BuildSimpleTendention_FirstDown()
    {
        var referenceResult = new GFS.GrailCommon.Models.Tendention();
        TendentionTestsData.SimpleQuotesCollectionResult_FirstDown.ToList().ForEach(point => referenceResult.AddPoint(point));

        var builder = new TendentionBuilder(TendentionTestsData.SimpleQuotesCollection_FirstDown);
        var verifiableResult = builder.BuildThreePointsTendention();
        
        Assert.Equal(referenceResult.IsCorrect, verifiableResult.IsCorrect);
    }
}