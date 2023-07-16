using GFS.AnalysisSystem.Library.Balances.Models;
using GFS.Common.Helpers;

namespace GFS.AnalysisSystem.Library.Balances.Abstractions
{
    public abstract class BalanceCalculationMethod<TParentGroup> : IBalanceCalculationMethod
        where TParentGroup : IBalanceCalculationMethodsGroup
    {
        private string? _name;

        protected BalanceCalculationMethod(TParentGroup parentGroup)
        {
            ParentGroup = parentGroup;
            parentGroup.AddChildMethod(this);
            Identifier = Guid.NewGuid();
        }

        public Guid Identifier { get; }
        
        public string FullName => _name ??= StringHelper.JoinString('.', new[] {ParentGroup?.FullName, OwnName});
        public IBalanceCalculationMethodsGroup? ParentGroup { get; }
        public abstract string OwnName { get; }

        public abstract void Calculate(CalculationContext context);
    }
}