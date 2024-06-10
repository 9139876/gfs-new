using System.ComponentModel.DataAnnotations;
using GFS.QuotesService.Api.Enum;
#pragma warning disable CS8618

namespace GFS.QuotesService.BL.Models;

public class AssetModel
{
    public MarketTypeEnum MarketType { get; set; }
    public AssetTypeEnum AssetType { get; set; }
    
    /// <summary> Торговая площадка </summary>
    [Required]
    public string Exchange { get; set; }

    /// <summary> Человекочитаемое имя </summary>
    [Required]
    public string Name { get; set; }

    /// <summary> Financial Instrument Global Identifier </summary>
    [Required]
    public string Figi { get; set; }

    /// <summary> Короткий код инструмента </summary>
    [Required]
    public string Ticker { get; set; }

    /// <summary> Класс-код инструмента </summary>
    [Required]
    public string ClassCode { get; set; }

    /// <summary> International Securities Identification Number - Международный идентификационный код ценной бумаги </summary>
    public string? ISIN { get; set; }
    
    /// <summary> Валюта расчётов </summary>
    public string? Currency { get; set; }

    /// <summary> Шаг цены </summary>
    public decimal? MinPriceIncrement { get; set; }

    /// <summary> Лотность инструмента. Возможно совершение операций только на количества ценной бумаги, кратные параметру lot </summary>
    public int? Lot { get; set; }

    /// <summary> Дата IPO акции в часовом поясе UTC </summary>
    public DateTime? IpoDate { get; set; }

    /// <summary> Сектор экономики </summary>
    public string? Sector { get; set; }
    
    /// <summary> Дата первой 1-минутной свечи у Тинькофф </summary>
    public DateTime? First1MinCandleDate { get; set; }
    
    /// <summary> Дата первой 1-дневной свечи у Тинькофф </summary>
    public DateTime? First1DayCandleDate { get; set; }
}