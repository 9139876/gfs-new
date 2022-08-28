using GFS.BalanceCalculation.BL.Abstractions;

namespace GFS.BalanceCalculation.BL.Groups
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