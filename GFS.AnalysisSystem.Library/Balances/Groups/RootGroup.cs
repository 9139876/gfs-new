using GFS.AnalysisSystem.Library.Balances.Abstractions;

namespace GFS.AnalysisSystem.Library.Balances.Groups
{
    public sealed class RootGroup : BalanceCalculationMethodsGroup<RootGroup>
    {
        public RootGroup() : base(null)
        {
        }

        public override string? OwnName => null;
        public override string? FullName => null;
    }
}