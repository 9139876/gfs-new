using GFS.AnalysisSystem.Library.Internal.Trash.Livermore.Enum;
using GFS.AnalysisSystem.Library.Internal.Trash.Livermore.Models;

namespace GFS.AnalysisSystem.Library.Internal.Trash.Livermore;

internal class LivermoreRecordHandler
{
    private readonly AssetTable _table;

    // private readonly Func<decimal, decimal, bool> _isSixPointDifference;

    public LivermoreRecordHandler(
        AssetTable table)
    {
        _table = table;
    }

    public TableColumnTypeEnum? Handle(IPriceValue price)
        => _table.CurrentColumn() switch
        {
            TableColumnTypeEnum.UpwardTrend => OnUpwardTrend(price),
            TableColumnTypeEnum.NaturalRally => OnNaturalRally(price),
            TableColumnTypeEnum.SecondaryRally => OnSecondaryRally(price),
            TableColumnTypeEnum.SecondaryReaction => OnSecondaryReaction(price),
            TableColumnTypeEnum.NaturalReaction => OnNaturalReaction(price),
            TableColumnTypeEnum.DownwardTrend => OnDownwardTrend(price),
            _ => throw new ArgumentOutOfRangeException(nameof(_table.CurrentColumn))
        };

    private TableColumnTypeEnum? OnUpwardTrend(IPriceValue price)
    {
        if (price.IsMoreThan(_table.LastValue()))
            return TableColumnTypeEnum.UpwardTrend;

        if (price.IsLessOnSixPointsThan(_table.LastValue()))
        {
            var (needUnderlined, column) = TurnToDown(price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.UpwardTrend);

            return column;
        }

        return null;
    }

    private TableColumnTypeEnum? OnNaturalRally(IPriceValue price)
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
            var (needUnderlined, column) = TurnToDown(price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.NaturalRally);

            return column;
        }

        return null;
    }

    private TableColumnTypeEnum? OnSecondaryRally(IPriceValue price)
    {
        if (price.IsMoreThan(_table.LastValue()))
            return true switch
            {
                true when price.IsMoreThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.UpwardTrend)) => TableColumnTypeEnum.UpwardTrend,
                true when price.IsMoreThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalRally)) => TableColumnTypeEnum.NaturalRally,
                _ => TableColumnTypeEnum.SecondaryRally
            };

        return price.IsLessOnSixPointsThan(_table.LastValue())
            ? TurnToDown(price).Item2
            : null;
    }

    private TableColumnTypeEnum? OnSecondaryReaction(IPriceValue price)
    {
        if (price.IsLessThan(_table.LastValue()))
        {
            if (price.IsLessThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.DownwardTrend)))
                return TableColumnTypeEnum.DownwardTrend;

            return price.IsLessThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalReaction))
                ? TableColumnTypeEnum.NaturalReaction
                : TableColumnTypeEnum.SecondaryReaction;
        }

        return price.IsMoreOnSixPointsThan(_table.LastValue())
            ? TurnToUp(price).Item2
            : null;
    }

    private TableColumnTypeEnum? OnNaturalReaction(IPriceValue price)
    {
        if (price.IsLessThan(_table.LastValue()))
        {
            return price.IsLessThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.DownwardTrend)) ||
                   price.IsLessOnThreePointsThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalReaction))
                ? TableColumnTypeEnum.DownwardTrend
                : TableColumnTypeEnum.NaturalReaction;
        }

        if (price.IsMoreOnSixPointsThan(_table.LastValue()))
        {
            var (needUnderlined, column) = TurnToUp(price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.NaturalReaction);

            return column;
        }

        return null;
    }

    private TableColumnTypeEnum? OnDownwardTrend(IPriceValue price)
    {
        if (price.IsLessThan(_table.LastValue()))
            return TableColumnTypeEnum.DownwardTrend;

        if (price.IsMoreOnSixPointsThan(_table.LastValue()))
        {
            var (needUnderlined, column) = TurnToUp(price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.DownwardTrend);

            return column;
        }

        return null;
    }

    private (bool, TableColumnTypeEnum) TurnToDown(IPriceValue price)
        => true switch
        {
            true when price.IsLessThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.DownwardTrend)) => (true, TableColumnTypeEnum.DownwardTrend),
            true when price.IsLessThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalReaction)) => (true, TableColumnTypeEnum.NaturalReaction),
            _ => (false, TableColumnTypeEnum.SecondaryReaction)
        };


    private (bool, TableColumnTypeEnum) TurnToUp(IPriceValue price)
        => true switch
        {
            true when price.IsMoreThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.UpwardTrend)) => (true, TableColumnTypeEnum.UpwardTrend),
            true when price.IsMoreThan(_table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalRally)) => (true, TableColumnTypeEnum.NaturalRally),
            _ => (false, TableColumnTypeEnum.SecondaryRally)
        };
}