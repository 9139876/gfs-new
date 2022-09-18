using GFS.BalanceCalculation.BL.Abstractions;
using GFS.BalanceCalculation.BL.Groups;
using GFS.BalanceCalculation.BL.Models;
using GFS.Common.Extensions;

namespace GFS.BalanceCalculation.BL
{
    public static class MethodsTree
    {
        private static IBalanceCalculationMethodsGroup? _treeRoot;

        public static IBalanceCalculationMethodsGroup TreeRoot => _treeRoot ??= BuildAllFromAssembly();

        public static void Calculate(IEnumerable<Guid> methodsIds, CalculationContext context)
        {
            var methods = methodsIds.Select(id => TreeRoot.TryGetMethod(id)).ToHashSet();
            methods.ThrowIfAny(m => m == null, new InvalidOperationException());
            
            Parallel.ForEach(methods, method => method!.Calculate(context));
        }

        private static IBalanceCalculationMethodsGroup Build(HashSet<Type> groupsTypes, HashSet<Type> methodsTypes)
        {
            groupsTypes = groupsTypes.Where(t => t != typeof(RootGroup)).ToHashSet();

            var root = new RootGroup();
            var inTreeGroups = new Queue<IBalanceCalculationMethodsGroup>();
            inTreeGroups.Enqueue(root);

            while (inTreeGroups.TryDequeue(out var currentGroup))
            {
                groupsTypes
                    .Where(groupType => groupType.GetBaseClass().GenericTypeArguments.FirstOrDefault() == currentGroup.GetType())
                    .ToList()
                    .ForEach(groupType =>
                    {
                        var group = Activator.CreateInstance(groupType, currentGroup) as IBalanceCalculationMethodsGroup
                                    ?? throw new InvalidOperationException($"Type of group '{groupType.Name}' is not assignable {nameof(IBalanceCalculationMethodsGroup)}");
                        groupsTypes.Remove(groupType);
                        inTreeGroups.Enqueue(group);
                    });

                methodsTypes
                    .Where(methodType => methodType.GetBaseClass().GenericTypeArguments.FirstOrDefault() == currentGroup.GetType())
                    .ToList()
                    .ForEach(methodType =>
                    {
                        if (Activator.CreateInstance(methodType, currentGroup) is not IBalanceCalculationMethod)
                            throw new InvalidOperationException($"Type of method '{methodType.Name}' is not assignable {nameof(IBalanceCalculationMethod)}");
                        methodsTypes.Remove(methodType);
                    });
            }

            if (groupsTypes.Any())
                throw new InvalidOperationException($"Group '{groupsTypes.First().Name}' not have parent");

            if (methodsTypes.Any())
                throw new InvalidOperationException($"Method '{methodsTypes.First().Name}' not have parent");

            return root;
        }

        private static IBalanceCalculationMethodsGroup BuildAllFromAssembly()
        {
            var assembly = typeof(MethodsTree).Assembly;

            var groups = assembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.IsAssignableTo(typeof(IBalanceCalculationMethodsGroup)))
                .ToHashSet();
            var methods = assembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.IsAssignableTo(typeof(IBalanceCalculationMethod)))
                .ToHashSet();

            return Build(groups, methods);
        }
    }
}