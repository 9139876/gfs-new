using GFS.AnalysisSystem.Library.Livermore.Enum;
using GFS.AnalysisSystem.Library.Livermore.Models;
using GFS.GrailCommon.Models;

namespace GFS.AnalysisSystem.Library.Livermore;

public class LivermoreRecordHandler
{
    private readonly AssetTable _table;

    private readonly Func<decimal, decimal, bool> _isSixPointDifference;

    public LivermoreRecordHandler(
        AssetTable table,
        Func<decimal, decimal, bool> isSixPointDifference)
    {
        _table = table;
        _isSixPointDifference = isSixPointDifference;
    }

    public void Handle(PriceTimePoint point)
    {
        var column = _table.CurrentColumn switch
        {
            TableColumnTypeEnum.UpwardTrend => OnUpwardTrend(point),
            TableColumnTypeEnum.NaturalRally => OnNaturalRally(point),
            TableColumnTypeEnum.SecondaryRally => OnSecondaryRally(point),
            TableColumnTypeEnum.SecondaryReaction => OnSecondaryReaction(point),
            TableColumnTypeEnum.NaturalReaction => OnNaturalReaction(point),
            TableColumnTypeEnum.DownwardTrend => OnDownwardTrend(point),
            _ => throw new ArgumentOutOfRangeException(nameof(_table.CurrentColumn))
        };

        if (column != null)
            _table.AddRecord(new AssetTableRecord(point, column.Value));
    }

    private TableColumnTypeEnum? OnUpwardTrend(PriceTimePoint point)
    {
        if (PriceIsUp(point))
            return TableColumnTypeEnum.UpwardTrend;

        if (PriceIsDown(point) && _isSixPointDifference(_table.LastValue(), point.Price))
        {
            var (needUnderlined, column) = TurnToDown(point.Price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.UpwardTrend);

            return column;
        }

        return null;
    }

    private TableColumnTypeEnum? OnNaturalRally(PriceTimePoint point)
    {
        if (PriceIsUp(point))
        {
            return point.Price > _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.UpwardTrend)
                ? TableColumnTypeEnum.UpwardTrend
                : TableColumnTypeEnum.NaturalRally;
        }
        
        if (PriceIsDown(point) && _isSixPointDifference(_table.LastValue(), point.Price))
        {
            var (needUnderlined, column) = TurnToDown(point.Price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.NaturalRally);

            return column;
        }

        return null;
    }

    private TableColumnTypeEnum? OnSecondaryRally(PriceTimePoint point)
    {
        if (PriceIsUp(point))
        {
            if (point.Price > _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.UpwardTrend))
                return TableColumnTypeEnum.UpwardTrend;
            
            return point.Price > _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalRally) 
                ? TableColumnTypeEnum.NaturalRally 
                : TableColumnTypeEnum.SecondaryRally;
        }

        return PriceIsDown(point) && _isSixPointDifference(_table.LastValue(), point.Price)
            ? TurnToDown(point.Price).Item2
            : null;
    }

    private TableColumnTypeEnum? OnSecondaryReaction(PriceTimePoint point)
    {
        if (PriceIsDown(point))
        {
            if (point.Price < _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.DownwardTrend))
                return TableColumnTypeEnum.DownwardTrend;
            
            return point.Price < _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.NaturalReaction) 
                ? TableColumnTypeEnum.NaturalReaction 
                : TableColumnTypeEnum.SecondaryReaction;
        }

        return PriceIsUp(point) && _isSixPointDifference(_table.LastValue(), point.Price)
            ? TurnToUp(point.Price).Item2
            : null;
    }

    private TableColumnTypeEnum? OnNaturalReaction(PriceTimePoint point)
    {
        if (PriceIsDown(point))
        {
            return point.Price < _table.ColumnLastUnderlinedValue(TableColumnTypeEnum.DownwardTrend)
                ? TableColumnTypeEnum.DownwardTrend
                : TableColumnTypeEnum.NaturalReaction;
        }

        if (PriceIsUp(point) && _isSixPointDifference(_table.LastValue(), point.Price))
        {
            var (needUnderlined, column) = TurnToUp(point.Price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.NaturalReaction);

            return column;
        }

        return null;
    }

    private TableColumnTypeEnum? OnDownwardTrend(PriceTimePoint point)
    {
        if (PriceIsDown(point))
            return TableColumnTypeEnum.DownwardTrend;


        if (PriceIsUp(point) && _isSixPointDifference(_table.LastValue(), point.Price))
        {
            var (needUnderlined, column) = TurnToUp(point.Price);

            if (needUnderlined)
                _table.UnderlineLastRecord(TableColumnTypeEnum.DownwardTrend);

            return column;
        }

        return null;
    }

    private bool PriceIsUp(PriceTimePoint point) => point.Price > _table.LastValue();

    private bool PriceIsDown(PriceTimePoint point) => point.Price < _table.LastValue();

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