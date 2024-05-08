using GFS.AnalysisSystem.Library.Tendention.Models;
using GFS.AnalysisSystem.Library.Tendention.Models.BuildTendentionContexts;
using GFS.Common.Extensions;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Tendention;

public static class TendentionBuilder
{
    public static GrailCommon.Models.Tendention Build<TContext>(BuildTendentionRequest<TContext> request)
        where TContext : BuildTendentionContext
    {
        request.RequiredNotNull();
        request.Quotes.RequiredNotNull();

        var quotes = request.Quotes.OrderBy(q => q.Date).ToList();
        ValidateQuotes(quotes);

        return true switch
        {
            _ when request.Context is BuildThreePointsTendentionContext threePointsTendentionContext => BuildThreePointsTendention(quotes, threePointsTendentionContext),
            _ => throw new ArgumentOutOfRangeException(nameof(request.Context), request.Context, null)
        };
    }

    private static GrailCommon.Models.Tendention BuildThreePointsTendention(IList<CandleInCells> quotes, BuildThreePointsTendentionContext context)
    {
        var tendention = new GrailCommon.Models.Tendention();

        if (!TryGetFirstMove(quotes, out var firstMoveResult))
            return tendention;

        var (firstPointDate, firstMoveDirection) = firstMoveResult;
        BuildThreePointsTendentionInternal(quotes, tendention, firstPointDate, firstMoveDirection);

        return tendention;
    }

    private static void BuildThreePointsTendentionInternal(
        IList<CandleInCells> quotes,
        GrailCommon.Models.Tendention tendention,
        int firstPointDate,
        PriceMoveDirectionTypeEnum direction)
    {
        /*
         * inTrendMoveLastQuote - последняя котировка, которая участвует в текущем движении
         * pivotCandidate - точка-кандидат на разворотную точку тенденции
         */

        PriceTimePointInCells? pivotCandidate = null;
        CandleInCells? contrTrendMoveLastQuote = null;
        var inTrendMoveLastQuote = quotes.Single(q => q.Date == firstPointDate);
        var contrTrendMovesCount = 0;

        foreach (var quote in quotes.Skip(quotes.IndexOf(inTrendMoveLastQuote) + 1))
        {
            var currentRelation = GetQuotesRelationStatus(inTrendMoveLastQuote, quote);

            if (currentRelation == QuotesRelationStatus.InnerBar)
            {
                // inTrendMoveLastQuote = quote;
                continue;
            }

            if (currentRelation == QuotesRelationStatus.OuterBar)
            {
                // inTrendMoveLastQuote = quote;
                // pivotCandidate = GetPivotCandidate(pivotCandidate, inTrendMoveLastQuote, direction);
                
                continue;
            }

            // if (InOneDirection(currentRelation, direction))
            // {
            //     inTrendMoveLastQuote = quote;
            //     contrTrendMoveLastQuote = null;
            //     pivotCandidate = null;
            //     contrTrendMovesCount = 0;
            // }
            // else if (pivotCandidate == null)
            // {
            //     contrTrendMoveLastQuote = quote;
            //     pivotCandidate = GetPivotCandidate(inTrendMoveLastQuote, direction);
            //     contrTrendMovesCount++;
            // }
            // else if (InOneDirection(GetQuotesRelationStatus(contrTrendMoveLastQuote!, quote), ContrDirection(direction)))
            // {
            //     contrTrendMoveLastQuote = quote;
            //     contrTrendMovesCount++;
            // }

            if (InOneDirection(currentRelation, direction))
            {
                inTrendMoveLastQuote = quote;
                pivotCandidate = GetPivotCandidate(pivotCandidate, inTrendMoveLastQuote, direction);
                contrTrendMoveLastQuote = null;
                contrTrendMovesCount = 0;
            }
            else if (contrTrendMoveLastQuote == null || InOneDirection(GetQuotesRelationStatus(contrTrendMoveLastQuote, quote), ContrDirection(direction)))
            {
                contrTrendMoveLastQuote = quote;
                contrTrendMovesCount++;
            }
            
            if (contrTrendMovesCount >= 3)
                break;
        }

        if (pivotCandidate is null || contrTrendMovesCount < 3)
            return;

        tendention.AddPoint(pivotCandidate);
        BuildThreePointsTendentionInternal(quotes, tendention, pivotCandidate.X, ContrDirection(direction));
    }

    private static PriceMoveDirectionTypeEnum ContrDirection(PriceMoveDirectionTypeEnum direction)
        => direction switch
        {
            PriceMoveDirectionTypeEnum.Up => PriceMoveDirectionTypeEnum.Down,
            PriceMoveDirectionTypeEnum.Down => PriceMoveDirectionTypeEnum.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static bool InOneDirection(QuotesRelationStatus relation, PriceMoveDirectionTypeEnum direction)
        => relation == QuotesRelationStatus.MoveUp && direction == PriceMoveDirectionTypeEnum.Up ||
           relation == QuotesRelationStatus.MoveDown && direction == PriceMoveDirectionTypeEnum.Down;

    private static PriceTimePointInCells GetPivotCandidate(PriceTimePointInCells? currentPivotCandidate, CandleInCells quote, PriceMoveDirectionTypeEnum direction)
        => direction == PriceMoveDirectionTypeEnum.Up
            ? currentPivotCandidate == null || currentPivotCandidate.Y < quote.High ? new PriceTimePointInCells { X = quote.Date, Y = quote.High } : currentPivotCandidate
            : currentPivotCandidate == null || currentPivotCandidate.Y > quote.Low ? new PriceTimePointInCells { X = quote.Date, Y = quote.Low } : currentPivotCandidate;

    private static bool TryGetFirstMove(IList<CandleInCells> quotes, out (int, PriceMoveDirectionTypeEnum) result)
    {
        var moveUp = new List<CandleInCells>();
        var moveDown = new List<CandleInCells>();

        for (var i = 0; i < quotes.Count - 1; i++)
        {
            var quotesRelationStatus = GetQuotesRelationStatus(quotes[i], quotes[i + 1]);

            switch (quotesRelationStatus)
            {
                case QuotesRelationStatus.MoveUp:
                    if (!moveUp.Any())
                        moveUp.AddRange(new[] { quotes[i], quotes[i + 1] });
                    else if (GetQuotesRelationStatus(moveUp.Last(), quotes[i + 1]) == QuotesRelationStatus.MoveUp)
                        moveUp.Add(quotes[i + 1]);
                    break;
                case QuotesRelationStatus.MoveDown:
                    if (!moveDown.Any())
                        moveDown.AddRange(new[] { quotes[i], quotes[i + 1] });
                    else if (GetQuotesRelationStatus(moveDown.Last(), quotes[i + 1]) == QuotesRelationStatus.MoveDown)
                        moveDown.Add(quotes[i + 1]);
                    break;
                case QuotesRelationStatus.InnerBar:
                    break;
                case QuotesRelationStatus.OuterBar:
                    moveDown.Clear();
                    moveUp.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(quotesRelationStatus), quotesRelationStatus, null);
            }

            if (moveUp.Count >= 3)
            {
                result = (moveUp.First().Date, PriceMoveDirectionTypeEnum.Up);
                return true;
            }

            if (moveDown.Count >= 3)
            {
                result = (moveDown.First().Date, PriceMoveDirectionTypeEnum.Down);
                return true;
            }
        }

        result = (0, PriceMoveDirectionTypeEnum.Flat);
        return false;
    }

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

    private static void ValidateQuotes(IEnumerable<CandleInCells> quotes)
    {
        var duplicateGroup = quotes.GroupBy(q => q.Date).FirstOrDefault(gr => gr.Count() > 1);

        if (duplicateGroup is not null)
            throw new InvalidOperationException($"В коллекции котировок, переданных построителю тенденции, более одной котировки на дату {duplicateGroup.Key}");
    }

    private enum QuotesRelationStatus
    {
        MoveUp = 1,
        MoveDown = 2,
        InnerBar = 3,
        OuterBar = 4
    }
}