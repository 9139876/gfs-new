namespace GFS.FakeDealer.Api.Models;

public class MakeDealResponse
{
    public MakeDealRequest OperationRequest { get; init; }
    
    /// <summary>
    /// Сумма сделки. Принимает положительное значение, если дилер даёт деньги (счет пополняется), и отрицательное, если дилер берет деньги (счет уменьшается)
    /// </summary>
    public decimal CashAmount { get; init; }
}