using GFS.QuotesService.Api.Enum;
#pragma warning disable CS8618

namespace GFS.QuotesService.Api.Models;

public class AssetsInfoDto
{
    public Guid AssetId { get; init; }
    
    public MarketTypeEnum MarketType { get; init; }
    public AssetTypeEnum AssetType { get; init; }

    /// <summary> Торговая площадка </summary>
    public string Exchange { get; init; }

    /// <summary> Человекочитаемое имя </summary>
    public string Name { get; init; }

    /// <summary> Financial Instrument Global Identifier </summary>
    public string FIGI { get; init; }

    /// <summary> Короткий код инструмента </summary>
    public string Ticker { get; init; }

    /// <summary> Класс-код инструмента </summary>
    public string ClassCode { get; init; }

    /// <summary> International Securities Identification Number - Международный идентификационный код ценной бумаги </summary>
    public string ISIN { get; init; }
    
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