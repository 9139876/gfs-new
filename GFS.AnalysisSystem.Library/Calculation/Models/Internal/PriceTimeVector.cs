using System.Drawing;
using GFS.Common.Extensions;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Extensions;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Calculation.Models.Internal;

public class PriceTimeVector
{
    public PriceTimeVector(
        Point sourcePointInCell,
        Point targetPointInCell,
        PriceTimePoint sourcePoint,
        PriceTimePoint targetPoint,
        TimeFrameEnum timeFrame)
    {
        SourcePointInCell = sourcePointInCell;
        TargetPointInCell = targetPointInCell;
        SourcePoint = sourcePoint;
        TargetPoint = targetPoint;
        TimeFrame = timeFrame;
    }

    public PriceTimeVector(
        Point sourcePointInCell,
        Point targetPointInCell,
        CalculationContext context)
    {
        SourcePointInCell = sourcePointInCell;
        TargetPointInCell = targetPointInCell;
        SourcePoint = context.GetPriceTimePosition(sourcePointInCell);
        TargetPoint = context.GetPriceTimePosition(targetPointInCell);
        TimeFrame = context.TimeFrame;
    }

    public Point SourcePointInCell { get; }

    public Point TargetPointInCell { get; }

    public PriceTimePoint SourcePoint { get; }

    public PriceTimePoint TargetPoint { get; }

    public TimeFrameEnum TimeFrame { get; }

    public TimeSpan TimeSpan => TargetPoint.Date - SourcePoint.Date;

    public decimal DeltaPrice => TargetPoint.Price - SourcePoint.Price;

    public decimal AbsDeltaPrice => Math.Abs(TargetPoint.Price - SourcePoint.Price);

    public string Name => $"[{GetText(SourcePoint)} - {GetText(TargetPoint)}]";

    private string GetText(PriceTimePoint point)
        => $"{point.Price.ToHumanReadableNumber()} {point.Date.GetDateStringByTimeFrame(TimeFrame)}";
}