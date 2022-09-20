using GFS.QuotesService.Api.Enum;

namespace GFS.QuotesService.BL.Models;

public class InitialModel
{
    public MarketTypeEnum MarketType { get; set; }
    public AssetTypeEnum AssetType { get; set; }
    
    /// <summary> Торговая площадка </summary>
    public string Exchange { get; set; }

    /// <summary> Человекочитаемое имя </summary>
    public string Name { get; set; }

    /// <summary> Financial Instrument Global Identifier </summary>
    public string Figi { get; set; }

    /// <summary> Короткий код инструмента </summary>
    public string Ticker { get; set; }

    /// <summary> Класс-код инструмента </summary>
    public string ClassCode { get; set; }

    /// <summary> International Securities Identification Number - Международный идентификационный код ценной бумаги </summary>
    public string Isin { get; set; }
    
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
}