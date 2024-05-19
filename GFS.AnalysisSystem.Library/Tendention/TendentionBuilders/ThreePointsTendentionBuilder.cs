using System.Diagnostics.CodeAnalysis;
using GFS.AnalysisSystem.Library.Tendention.Models.BuildTendentionContexts;
using GFS.Common.Extensions;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Tendention.TendentionBuilders;

public class ThreePointsTendentionBuilder : FromCandlesTendentionBuilder<BuildThreePointsTendentionContext>
{
    private readonly GrailCommon.Models.Tendention _tendention;

    public ThreePointsTendentionBuilder(BuildThreePointsTendentionContext context, IList<CandleInCells> candles) : base(context, candles)
    {
        _tendention = new GrailCommon.Models.Tendention();
    }

    public override GrailCommon.Models.Tendention BuildThreePointsTendention()
    {
        _tendention.Clear();

        if (!TryGetFirstMove(Candles.ToList(), out var firstMoveDirection))
            return _tendention;

        BuildThreePointsTendentionInternal(Candles.ToList(), firstMoveDirection);

        return _tendention;
    }

    private bool TryGetFirstMove(IList<CandleInCells> candles, out PriceMoveDirectionTypeEnum direction)
    {
        var ifUpMovePivot = GetNextPivot(candles, PriceMoveDirectionTypeEnum.Up);
        var ifDownMovePivot = GetNextPivot(candles, PriceMoveDirectionTypeEnum.Up);

        direction = PriceMoveDirectionTypeEnum.Unknown;

        if (ifUpMovePivot is null && ifDownMovePivot is null)
            return false;

        direction = true switch
        {
            _ when ifUpMovePivot is null => PriceMoveDirectionTypeEnum.Down,
            _ when ifDownMovePivot is null => PriceMoveDirectionTypeEnum.Up,
            _ => candles.IndexOf(ifUpMovePivot) < candles.IndexOf(ifDownMovePivot) ? PriceMoveDirectionTypeEnum.Up : PriceMoveDirectionTypeEnum.Down
        };

        return true;
    }

    [SuppressMessage("ReSharper", "LoopVariableIsNeverChangedInsideLoop")]
    private void BuildThreePointsTendentionInternal(IList<CandleInCells> candles, PriceMoveDirectionTypeEnum direction)
    {
        while (true)
        {
            var pivotCandidate = GetNextPivot(candles, direction);

            if (pivotCandidate is null)
                return;

            _tendention.AddPoint(GetPivotValue(pivotCandidate, direction));
            candles = candles.Skip(candles.IndexOf(pivotCandidate)).ToList();
            direction = GetContrDirection(direction);
        }
    }

    private static CandleInCells? GetNextPivot(IList<CandleInCells> candles, PriceMoveDirectionTypeEnum direction)
    {
        /*
        *  lastNormalCandle - последняя "нормальная" свеча - не внешний и не внутренний бар
        *  pivotCandidate - точка-кандидат на разворотную точку тенденции
        */

        var lastNormalCandle = candles.FirstOrDefault();
        if (lastNormalCandle is null)
            return null;

        CandleInCells? pivotCandidate = null;
        var contrTrendMovesCount = 0;

        foreach (var candle in candles.Skip(1))
        {
            var currentRelation = GetQuotesRelationStatus(lastNormalCandle, candle);

            if (currentRelation == QuotesRelationStatus.OuterBar)
            {
                pivotCandidate = GetPivotCandidate(pivotCandidate, candle, direction);
                continue;
            }

            lastNormalCandle = candle;

            if (currentRelation == QuotesRelationStatus.InnerBar)
                continue;
            
            if (InOneDirection(currentRelation, direction))
            {
                pivotCandidate = GetPivotCandidate(pivotCandidate, candle, direction);
                contrTrendMovesCount = 0;
            }
            else
            {
                contrTrendMovesCount++;
            }

            if (contrTrendMovesCount >= 3)
                break;
        }

        return contrTrendMovesCount < 3 ? null : pivotCandidate;
    }

    private static PriceMoveDirectionTypeEnum GetContrDirection(PriceMoveDirectionTypeEnum direction)
        => direction switch
        {
            PriceMoveDirectionTypeEnum.Up => PriceMoveDirectionTypeEnum.Down,
            PriceMoveDirectionTypeEnum.Down => PriceMoveDirectionTypeEnum.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static bool InOneDirection(QuotesRelationStatus relation, PriceMoveDirectionTypeEnum direction)
        => relation == QuotesRelationStatus.MoveUp && direction == PriceMoveDirectionTypeEnum.Up ||
           relation == QuotesRelationStatus.MoveDown && direction == PriceMoveDirectionTypeEnum.Down;

    private static CandleInCells GetPivotCandidate(CandleInCells? currentPivotCandidate, CandleInCells quote, PriceMoveDirectionTypeEnum direction)
        => direction == PriceMoveDirectionTypeEnum.Up
            ? currentPivotCandidate == null || currentPivotCandidate.High <= quote.High
                ? quote
                : currentPivotCandidate
            : currentPivotCandidate == null || currentPivotCandidate.Low >= quote.Low
                ? quote
                : currentPivotCandidate;

    private static QuotesRelationStatus GetQuotesRelationStatus(CandleInCells quote1, CandleInCells quote2)
    {
        return true switch
        {
            true when quote2.High > quote1.High && quote2.Low >= quote1.Low => QuotesRelationStatus.MoveUp,
            true when quote2.High <= quote1.High && quote2.Low < quote1.Low => QuotesRelationStatus.MoveDown,
            true when quote2.High <= quote1.High && quote2.Low >= quote1.Low => QuotesRelationStatus.InnerBar,
            true when quote2.High > quote1.High && quote2.Low < quote1.Low => QuotesRelationStatus.OuterBar,
            _ => throw new InvalidOperationException($"Неопределенное состояние - quote1={quote1.Serialize()}, quote2={quote2.Serialize()}")
        };
    }

    private static PriceTimePointInCells GetPivotValue(CandleInCells candle, PriceMoveDirectionTypeEnum direction)
    {
        return direction switch
        {
            PriceMoveDirectionTypeEnum.Up => new PriceTimePointInCells { X = candle.Date, Y = candle.High },
            PriceMoveDirectionTypeEnum.Down => new PriceTimePointInCells { X = candle.Date, Y = candle.Low },
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private enum QuotesRelationStatus
    {
        MoveUp = 1,
        MoveDown = 2,
        InnerBar = 3,
        OuterBar = 4
    }
}