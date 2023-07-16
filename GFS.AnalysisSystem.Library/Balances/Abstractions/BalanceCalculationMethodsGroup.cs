using System.Data;
using GFS.Common.Helpers;

namespace GFS.AnalysisSystem.Library.Balances.Abstractions
{
    public abstract class BalanceCalculationMethodsGroup<TParentGroup> : IBalanceCalculationMethodsGroup
        where TParentGroup : IBalanceCalculationMethodsGroup
    {
        private readonly List<IBalanceCalculationMethod> _methods = new();
        private readonly List<IBalanceCalculationMethodsGroup> _groups = new();
        private string? _name;

        protected BalanceCalculationMethodsGroup(TParentGroup? parentGroup)
        {
            ParentGroup = parentGroup;
            parentGroup?.AddChildGroup(this);
        }

        public virtual string? FullName => _name ??= StringHelper.JoinString('.', new[] {ParentGroup?.FullName, OwnName});
        public List<IBalanceCalculationMethod> Methods => _methods.ToList();
        public List<IBalanceCalculationMethodsGroup> Groups => _groups.ToList();
        public IBalanceCalculationMethodsGroup? ParentGroup { get; }

        public void AddChildMethod(IBalanceCalculationMethod method)
        {
            if (method.ParentGroup?.GetType() != GetType())
                throw new InvalidOperationException();

            if (_methods.Select(m => m.GetType().Name).Contains(method.GetType().Name))
                throw new DuplicateNameException();

            _methods.Add(method);
        }

        public void AddChildGroup(IBalanceCalculationMethodsGroup group)
        {
            if (group.ParentGroup?.GetType() != GetType())
                throw new InvalidOperationException();

            if (_groups.Select(g => g.GetType().Name).Contains(group.GetType().Name))
                throw new DuplicateNameException();

            _groups.Add(group);
        }

        public IBalanceCalculationMethod? TryGetMethod(Guid id)
            => _methods.FirstOrDefault(m => m.Identifier == id)
               ?? _groups.Select(g => g.TryGetMethod(id)).FirstOrDefault(m => m != null);

        // public IBalanceCalculationMethodsGroup? TryGetGroup<TGroup>() where TGroup : IBalanceCalculationMethodsGroup
        //     => TryGetGroup(group => group.GetType() == typeof(TGroup));
        //
        // public IBalanceCalculationMethodsGroup? TryGetGroup(Predicate<IBalanceCalculationMethodsGroup> predicate)
        //     => _groups.FirstOrDefault(new Func<IBalanceCalculationMethodsGroup, bool>(predicate))
        //        ?? _groups.Select(g => g.TryGetGroup(predicate)).FirstOrDefault(g => g != null);
        //
        // public IBalanceCalculationMethod? TryGetMethod<TMethod>() where TMethod : IBalanceCalculationMethod
        //     => TryGetMethod(method => method.GetType() == typeof(TMethod));
        //
        // public IBalanceCalculationMethod? TryGetMethod(Predicate<IBalanceCalculationMethod> predicate)
        //     => _methods.FirstOrDefault(new Func<IBalanceCalculationMethod, bool>(predicate))
        //        ?? _groups.Select(g => g.TryGetMethod(predicate)).FirstOrDefault(m => m != null);

        public abstract string? OwnName { get; }
    }
}