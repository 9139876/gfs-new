using GFS.FakeDealer.Api.Enums;

namespace GFS.FakeDealer.Api.Models;

public class MakeDealRequest
{
    /// <summary>
    /// Financial Instrument Global Identifier инструмента сделки
    /// </summary>
    public string FIGI { get; init; }
    
    /// <summary>
    /// Количество единиц инструмента, участвующих в сделке
    /// </summary>
    public int AssetUnitsCount { get; init; }
 
    /// <summary>
    /// Дата совершения сделки
    /// </summary>
    public DateTime DealDateUtc { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// Вид операции
    /// </summary>
    public DealerOperationTypeEnum OperationType { get; init; }
}