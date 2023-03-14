namespace GFS.FakeDealer.Api.Models;

public class MakeDealResponse
{
    /// <summary>
    /// Financial Instrument Global Identifier инструмента сделки
    /// </summary>
    public string FIGI { get; init; }
    
    /// <summary>
    /// Дата совершения сделки
    /// </summary>
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