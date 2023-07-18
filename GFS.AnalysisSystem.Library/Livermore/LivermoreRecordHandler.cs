using GFS.AnalysisSystem.Library.Livermore.Enum;
using GFS.AnalysisSystem.Library.Livermore.Models;

namespace GFS.AnalysisSystem.Library.Livermore;

public class LivermoreRecordHandler
{
    private readonly AssetTable _table;

    // private readonly Func<decimal, decimal, bool> _isSixPointDifference;

    public LivermoreRecordHandler(
        AssetTable table)
    {
        _table = table;
    }

    public TableColumnTypeEnum? Handle(PriceValue price)
        => _table.CurrentColumn switch
        {
            TableColumnTypeEnum.UpwardTrend => OnUpwardTrend(price),
            TableColumnTypeEnum.NaturalRally => OnNaturalRally(price),
            TableColumnTypeEnum.SecondaryRally => OnSecondaryRally(price),
            TableColumnTypeEnum.SecondaryReaction => OnSecondaryReaction(price),
            TableColumnTypeEnum.NaturalReaction => OnNaturalReaction(price),
            TableColumnTypeEnum.DownwardTrend => OnDownwardTrend(price),
            _ => throw new ArgumentOutOfRangeException(nameof(_table.CurrentColumn))
        };

    private TableColumnTypeEnum? OnUpwardTrend(PriceValue price)
    {
        if (price.IsMoreThan(_table.LastValue()))
            return TableColumnTypeEnum.UpwardTrend;

        if (price.IsLessOnSixPointsThan(_table.LastValue()))
        {
            var (needUnderlined, column) = TurnToDown(point.Price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.UpwardTrend);

            return column;
        }

        return null;
    }

    private TableColumnTypeEnum? OnNaturalRally(PriceValue price)
    {
        if (price.IsMoreThan(_table.LastValue()))
        {
            return price.IsMoreThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.UpwardTrend)) ||
                   price.IsMoreOnThreePointsThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalRally))
                ? TableColumnTypeEnum.UpwardTrend
                : TableColumnTypeEnum.NaturalRally;
        }

        if (price.IsLessOnSixPointsThan(_table.LastValue()))
        {
            var (needUnderlined, column) = TurnToDown(point.Price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.NaturalRally);

            return column;
        }

        return null;
    }

    private TableColumnTypeEnum? OnSecondaryRally(PriceValue price)
    {
        if (price.IsMoreThan(_table.LastValue()))
            return true switch
            {
                true when price.IsMoreThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.UpwardTrend)) => TableColumnTypeEnum.UpwardTrend,
                true when price.IsMoreThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalRally)) => TableColumnTypeEnum.NaturalRally,
                _ => TableColumnTypeEnum.SecondaryRally
            };

        return price.IsLessOnSixPointsThan(_table.LastValue())
            ? TurnToDown(point.Price).Item2
            : null;
    }

    private TableColumnTypeEnum? OnSecondaryReaction(PriceValue price)
    {
        if (price.IsLessThan(_table.LastValue()))
        {
            if (point.Price < _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.DownwardTrend))
                return TableColumnTypeEnum.DownwardTrend;

            return point.Price < _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalReaction)
                ? TableColumnTypeEnum.NaturalReaction
                : TableColumnTypeEnum.SecondaryReaction;
        }

        return price.IsMoreThan(_table.LastValue()) && _isSixPointDifference(_table.LastValue(), point.Price)
            ? TurnToUp(point.Price).Item2
            : null;
    }

    private TableColumnTypeEnum? OnNaturalReaction(PriceValue price)
    {
        if (price.IsLessThan(_table.LastValue()))
        {
            return point.Price < _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.DownwardTrend) || //5a5b
                ? TableColumnTypeEnum.DownwardTrend
                : TableColumnTypeEnum.NaturalReaction;
        }

        if (price.IsMoreThan(_table.LastValue()) && _isSixPointDifference(_table.LastValue(), point.Price))
        {
            var (needUnderlined, column) = TurnToUp(point.Price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.NaturalReaction);

            return column;
        }

        return null;
    }

    private TableColumnTypeEnum? OnDownwardTrend(PriceValue price)
    {
        if (price.IsLessThan(_table.LastValue()))
            return TableColumnTypeEnum.DownwardTrend;


        if (price.IsMoreThan(_table.LastValue()) && _isSixPointDifference(_table.LastValue(), point.Price))
        {
            var (needUnderlined, column) = TurnToUp(point.Price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.DownwardTrend);

            return column;
        }

        return null;
    }

    private bool PriceIsUp(PriceValue price) => point.Price > _table.LastValue();

    private bool PriceIsDown(PriceValue price) => point.Price < _table.LastValue();

    private (bool, TableColumnTypeEnum) TurnToDown(decimal price)
        => true switch
        {
            true when _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.DownwardTrend) > price => (true, TableColumnTypeEnum.DownwardTrend),
            true when _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalReaction) > price => (true, TableColumnTypeEnum.NaturalReaction),
            _ => (false, TableColumnTypeEnum.SecondaryReaction)
        };


    private (bool, TableColumnTypeEnum) TurnToUp(decimal price)
        => true switch
        {
            true when _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.UpwardTrend) < price => (true, TableColumnTypeEnum.UpwardTrend),
            true when _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalRally) < price => (true, TableColumnTypeEnum.NaturalRally),
            _ => (false, TableColumnTypeEnum.SecondaryRally)
        };
}