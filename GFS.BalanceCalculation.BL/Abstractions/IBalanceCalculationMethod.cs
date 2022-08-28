using System;
using GFS.BalanceCalculation.BL.Models;

namespace GFS.BalanceCalculation.BL.Abstractions
{
    public interface IBalanceCalculationMethod : IGroupedItem
    {
        Guid Identifier { get; }
        void Calculate(CalculationContext context);
    }
}