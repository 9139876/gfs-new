using System.ComponentModel.DataAnnotations;
using GFS.Common.Attributes.Validation;

#pragma warning disable CS8618

namespace GFS.Broker.Api.Models;

public class MakeDealResponse
{
    /// <summary>
    /// Financial Instrument Global Identifier инструмента сделки
    /// </summary>
    [Required]
    public string FIGI { get; init; }
    
    /// <summary>
    /// Дата совершения сделки
    /// </summary>
    [UtcDate]
    public DateTime DealDateUtc { get; init; }

    /// <summary>
    /// Цена, по которой была совершена сделка 
    /// </summary>
    public decimal DealPrice { get; init; }

    /// <summary>
    /// Изменение суммы на счёте после сделки
    /// </summary>
    public decimal DeltaCash { get; init; }
    
    /// <summary>
    /// Изменение количества единиц инструмента в портфеле после сделки
    /// </summary>
    public int DeltaAsset { get; init; }
}