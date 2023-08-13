using GFS.AnalysisSystem.Library.Livermore.Models;
using HtmlAgilityPack;

namespace GFS.AnalysisSystem.Library.Livermore.Export;

public abstract class HtmlTablesCreator
{
    protected readonly HtmlDocument Document;

    protected HtmlTablesCreator()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Document = GetTemplate();
    }

    public HtmlDocument Fill(AssetTable[] assetTables)
    {
        FillHeaders(assetTables);

        var dates = assetTables
            .SelectMany(t => t.GetAllRecordDates())
            .Distinct()
            .OrderBy(d => d)
            .ToList();

        var tableNode = GetTableNode();

        foreach (var date in dates)
        {
            var row = CreateRowNode();
            row.ChildNodes.Add(CreateDateCellNode(date));

            foreach (var assetTable in assetTables)
            {
                var record = assetTable.GetRecordByDate(date);
                FillCellsByRecord(row, record);
            }

            tableNode.ChildNodes.Add(row);
        }

        return Document;
    }

    protected abstract HtmlNode GetTableNode(); //Document.GetElementbyId("132");

    protected abstract HtmlDocument GetTemplate();

    protected abstract HtmlNode CreateRowNode(); //new HtmlNode(HtmlNodeType.Element, Document, 0);

    protected abstract HtmlNode CreateDateCellNode(DateTime date); //new HtmlNode(HtmlNodeType.Element, Document, 0);

    protected abstract void FillHeaders(AssetTable[] tables);

    protected abstract void FillCellsByRecord(HtmlNode row, AssetTableRecord? record);
}

public class HtmlTablesCreatorStandard : HtmlTablesCreator
{
    protected override HtmlDocument GetTemplate()
    {
        var path = "";//???

        var document = new HtmlDocument();
        document.Load(path);

        return document;
    }
    
    protected override HtmlNode GetTableNode()
    {
        throw new NotImplementedException();
    }

    protected override HtmlNode CreateRowNode()
    {
        throw new NotImplementedException();
    }

    protected override HtmlNode CreateDateCellNode(DateTime date)
    {
        throw new NotImplementedException();
    }

    protected override void FillHeaders(AssetTable[] tables)
    {
        throw new NotImplementedException();
    }

    protected override void FillCellsByRecord(HtmlNode row, AssetTableRecord? record)
    {
        throw new NotImplementedException();
    }
}