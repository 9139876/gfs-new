using System.Collections.Concurrent;
using System.Drawing;
using GFS.AnalysisSystem.Library.Calculation.Abstraction;
using GFS.AnalysisSystem.Library.Calculation.Models;
using GFS.Common.Extensions;

namespace GFS.AnalysisSystem.Library.Calculation;

public static class ForecastCalculator
{
    private static readonly IForecastTreeMethod[] Methods;
    private static readonly IForecastTreeGroup[] Groups;
    public static readonly ForecastTreeRoot ForecastTree;

    static ForecastCalculator()
    {
        Methods = GetMethods();
        Groups = GetGroups();
        ForecastTree = BuildForecastTree();
    }

    public static ForecastCalculationResult Calculate(IEnumerable<string> methodsIds, CalculationContext context)
    {
        var methods = methodsIds
            .Select(id => Methods.SingleOrDefault(m => m.Identifier == id))
            .Where(ftm => ftm != null);

        var bag = new ConcurrentBag<ForecastCalculationResult>();

        Parallel.ForEach(methods, method => bag.Add(method!.Calculate(context)));

        return CombineForecastCalculationResults(bag);
    }

    private static ForecastCalculationResult CombineForecastCalculationResults(IEnumerable<ForecastCalculationResult> results)
    {
        var dict = new Dictionary<Point, ForecastCalculationResultItem>();

        foreach (var methodResult in results)
        {
            foreach (var item in methodResult.GetItems)
            {
                if (!dict.ContainsKey(item.Position))
                    dict.Add(item.Position, new ForecastCalculationResultItem(position: item.Position));

                dict[item.Position].AddDescriptionList(item.Descriptions);
            }
        }

        var result = new ForecastCalculationResult();
        result.AddForecastCalculationResultItemList(dict.Values);

        return result;
    }

    private static IForecastTreeMethod[] GetMethods()
    {
        return typeof(ForecastCalculator).Assembly
            .GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.IsAssignableTo(typeof(IForecastTreeMethod)))
            .Distinct()
            .Select(type =>
            {
                if (Activator.CreateInstance(type) is not IForecastTreeMethod method)
                    throw new InvalidOperationException($"Type of method '{type.Name}' is not assignable {nameof(IForecastTreeMethod)}");

                return method;
            })
            .ToArray();
    }

    private static IForecastTreeGroup[] GetGroups()
    {
        return typeof(ForecastCalculator).Assembly
            .GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.IsAssignableTo(typeof(IForecastTreeGroup)) &&
                t != typeof(ForecastTreeRoot)
            )
            .Distinct()
            .Select(type =>
            {
                if (Activator.CreateInstance(type) is not IForecastTreeGroup group)
                    throw new InvalidOperationException($"Type of method '{type.Name}' is not assignable {nameof(IForecastTreeGroup)}");

                return group;
            })
            .ToArray();
    }

    private static ForecastTreeRoot BuildForecastTree()
    {
        var root = new ForecastTreeRoot();

        foreach (var group in Groups)
        {
            group.Children.AddRange(Methods
                .Where(method => method.GetType().GetBaseClass().GenericTypeArguments.FirstOrDefault() == group.GetType())
                .ToList());

            group.Children.AddRange(Groups
                .Where(treeGroup => treeGroup.GetType().GetBaseClass().GenericTypeArguments.FirstOrDefault() == group.GetType())
                .ToList());

            if (group.GetType().GetBaseClass().GenericTypeArguments.FirstOrDefault() == root.GetType() && group is IForecastTreeItem item)
                root.Children.Add(item);
        }

        return root;
    }
}