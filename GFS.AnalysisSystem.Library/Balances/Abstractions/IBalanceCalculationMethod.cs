using GFS.AnalysisSystem.Library.Balances.Models;

namespace GFS.AnalysisSystem.Library.Balances.Abstractions
{
    public interface IBalanceCalculationMethod : IGroupedItem
    {
        Guid Identifier { get; }
        void Calculate(CalculationContext context);
    }
}