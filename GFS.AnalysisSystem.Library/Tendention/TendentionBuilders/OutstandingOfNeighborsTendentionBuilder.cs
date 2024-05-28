using GFS.AnalysisSystem.Library.Tendention.Models.BuildTendentionContexts;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Tendention.TendentionBuilders;

public class OutstandingOfNeighborsTendentionBuilder : FromCandlesTendentionBuilder<BuildOutstandingOfNeighborsTendentionContext>
{
    private readonly GrailCommon.Models.Tendention _tendention;

    public OutstandingOfNeighborsTendentionBuilder(BuildOutstandingOfNeighborsTendentionContext context, IList<CandleInCells> candles) : base(context, candles)
    {
        _tendention = new GrailCommon.Models.Tendention();
    }

    public override GrailCommon.Models.Tendention BuildTendention()
    {
        _tendention.Clear();
        BuildInternal();
        return _tendention;
    }

    private void BuildInternal()
    {
        var neighborsCount = (int)Context.ByNeighborsType;

        var statusItems = new List<CandleStatusItem>();

        for (var i = neighborsCount; i < Candles.Count - neighborsCount; i++)
        {
            var index = i;

            var candle = Candles[index];
            var range = Candles.Skip(index - neighborsCount).Take(2 * neighborsCount + 1).Except(new[] { candle }).ToList();

            var isTop = range.All(c => candle.High >= c.High);
            var isBottom = range.All(c => candle.Low <= c.Low);

            var candleStatusItem = true switch
            {
                _ when isTop && isBottom => new CandleStatusItem(index, CandleStatus.OuterBar),
                _ when isTop => new CandleStatusItem(index, CandleStatus.Top),
                _ when isBottom => new CandleStatusItem(index, CandleStatus.Bottom),
                _ => null
            };

            if (candleStatusItem != null)
                statusItems.Add(candleStatusItem);
        }

        Correct(statusItems, 0);

        if (statusItems.Count < 2)
            return;

        if (!statusItems.All(si => new[] { CandleStatus.Top, CandleStatus.Bottom }.Contains(si.Status)))
            return;

        statusItems.ForEach(statusItem =>
        {
            var (index, candleStatus) = statusItem;
            var candle = Candles[index];

            var point = true switch
            {
                _ when candleStatus == CandleStatus.Top => new PriceTimePointInCells { X = candle.Date, Y = candle.High },
                _ when candleStatus == CandleStatus.Bottom => new PriceTimePointInCells { X = candle.Date, Y = candle.Low },
                _ => null
            };

            if (point != null)
                _tendention.AddPoint(point);
        });
    }

    private void Correct(List<CandleStatusItem> candleStatusItems, int startIndex)
    {
        if (startIndex >= candleStatusItems.Count - 1)
            return;

        var current = candleStatusItems[startIndex];
        var next = candleStatusItems[startIndex + 1];

        if (IsCorrectSequence(current.Status, next.Status))
        {
            Correct(candleStatusItems, startIndex + 1);
            return;
        }

        if (current.Status == CandleStatus.OuterBar)
        {
            candleStatusItems.Remove(current);
        }
        else if (next.Status == CandleStatus.OuterBar)
        {
            candleStatusItems.Remove(next);
            candleStatusItems.Insert(startIndex + 1, new CandleStatusItem(next.Index, current.Status == CandleStatus.Top ? CandleStatus.Bottom : CandleStatus.Top));
        }
        else
        {
            candleStatusItems.Remove(GetItemForRemove(current, next));
        }

        Correct(candleStatusItems, startIndex);
    }

    private CandleStatusItem GetItemForRemove(CandleStatusItem current, CandleStatusItem next)
        => current.Status == CandleStatus.Top
            ? Candles[next.Index].High > Candles[current.Index].High
                ? current
                : next
            : Candles[next.Index].Low < Candles[current.Index].Low
                ? current
                : next;

    private static bool IsCorrectSequence(CandleStatus status1, CandleStatus status2)
        => status1 == CandleStatus.Top && status2 == CandleStatus.Bottom || status1 == CandleStatus.Bottom && status2 == CandleStatus.Top;

    private enum CandleStatus
    {
        Top = 1,
        Bottom = 2,
        OuterBar = 3
    }

    private record CandleStatusItem(int Index, CandleStatus Status);
}