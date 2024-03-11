using GFS.Common.Extensions;
using GFS.GrailCommon.Enums;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Internal.Trash.Gann.Tendention;

internal class TendentionBuilder
{
    private readonly List<QuoteModel> _quotes;
    private GrailCommon.Models.Tendention? _tendention;

    public TendentionBuilder(IEnumerable<QuoteModel> quotes)
    {
        quotes.RequiredNotNull();
        _quotes = quotes.OrderBy(q => q.Date).ToList();
        ValidateQuotes(_quotes);
    }

    public GrailCommon.Models.Tendention BuildThreePointsTendention()
    {
        _tendention = new GrailCommon.Models.Tendention();

        if (!TryGetFirstMove(out var firstMoveResult))
            return _tendention;

        var (firstPointDate, firstMoveDirection) = firstMoveResult;
        Build(firstPointDate, firstMoveDirection);

        return _tendention;
    }

    private void Build(DateTime firstPointDate, PriceMoveDirectionTypeEnum direction)
    {
        /*
         * moveLastQuote - последняя котировка, которая участвует в текущем движении
         * pivotCandidate - точка-кандидат на разворотную точку тенденции
         */

        PriceTimePoint? pivotCandidate = null;
        QuoteModel? contrTrendMoveLastQuote = null;
        var inTrendMoveLastQuote = _quotes.Single(q => q.Date == firstPointDate);
        var contrTrendMovesCount = 0;

        foreach (var quote in _quotes.Skip(_quotes.IndexOf(inTrendMoveLastQuote) + 1))
        {
            var currentRelation = GetQuotesRelationStatus(inTrendMoveLastQuote, quote);

            if (currentRelation == QuotesRelationStatus.InnerBar)
                continue;

            if (currentRelation == QuotesRelationStatus.OuterBar)
                throw new NotImplementedException();

            if (InOneDirection(currentRelation, direction))
            {
                inTrendMoveLastQuote = quote;
                contrTrendMoveLastQuote = null;
                pivotCandidate = null;
                contrTrendMovesCount = 0;
            }
            else if (pivotCandidate == null)
            {
                contrTrendMoveLastQuote = quote;
                pivotCandidate = GetPivotCandidate(inTrendMoveLastQuote, direction);
                contrTrendMovesCount++;
            }
            else if (InOneDirection(GetQuotesRelationStatus(contrTrendMoveLastQuote!, quote), ContrDirection(direction)))
            {
                contrTrendMoveLastQuote = quote;
                contrTrendMovesCount++;
            }

            if (contrTrendMovesCount >= 3)
                break;
        }

        if (pivotCandidate is null || contrTrendMovesCount < 3)
            return;

        _tendention!.AddPoint(pivotCandidate);
        Build(pivotCandidate.Date, ContrDirection(direction));
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

    private static PriceTimePoint GetPivotCandidate(QuoteModel quote, PriceMoveDirectionTypeEnum direction)
        => direction == PriceMoveDirectionTypeEnum.Up ? new PriceTimePoint { Date = quote.Date, Price = quote.High } : new PriceTimePoint { Date = quote.Date, Price = quote.Low };

    private bool TryGetFirstMove(out (DateTime, PriceMoveDirectionTypeEnum) result)
    {
        var moveUp = new List<QuoteModel>();
        var moveDown = new List<QuoteModel>();

        for (int i = 0; i < _quotes.Count - 1; i++)
        {
            switch (GetQuotesRelationStatus(_quotes[i], _quotes[i + 1]))
            {
                case QuotesRelationStatus.MoveUp:
                    if (!moveUp.Any())
                        moveUp.AddRange(new[] { _quotes[i], _quotes[i + 1] });
                    else if (GetQuotesRelationStatus(moveUp.Last(), _quotes[i + 1]) == QuotesRelationStatus.MoveUp)
                        moveUp.Add(_quotes[i + 1]);
                    break;
                case QuotesRelationStatus.MoveDown:
                    if (!moveDown.Any())
                        moveDown.AddRange(new[] { _quotes[i], _quotes[i + 1] });
                    else if (GetQuotesRelationStatus(moveDown.Last(), _quotes[i + 1]) == QuotesRelationStatus.MoveDown)
                        moveDown.Add(_quotes[i + 1]);
                    break;
                case QuotesRelationStatus.InnerBar:
                    break;
                case QuotesRelationStatus.OuterBar:
                    moveDown.Clear();
                    moveUp.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

        result = (DateTime.MinValue, PriceMoveDirectionTypeEnum.Flat);
        return false;
    }

    private static QuotesRelationStatus GetQuotesRelationStatus(QuoteModel quote1, QuoteModel quote2)
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

    private static void ValidateQuotes(IEnumerable<QuoteModel> quotes)
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