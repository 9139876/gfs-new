using System.ComponentModel.DataAnnotations;
using GFS.Common.Attributes.Validation;
using GFS.FakeDealer.Api.Enums;
#pragma warning disable CS8618

namespace GFS.FakeDealer.Api.Models;

public class MakeDealRequest
{
    /// <summary>
    /// Financial Instrument Global Identifier инструмента сделки
    /// </summary>
    [Required]
    public string FIGI { get; init; }
    
    /// <summary>
    /// Количество единиц инструмента, участвующих в сделке
    /// </summary>
    [PositiveNumber]
    public int AssetUnitsCount { get; init; }
 
    /// <summary>
    /// Дата совершения сделки
    /// </summary>
    [UtcDate]
    public DateTime DealDateUtc { get; init; }
    
    /// <summary>
    /// Вид операции
    /// </summary>
    public DealerOperationTypeEnum OperationType { get; init; }
}