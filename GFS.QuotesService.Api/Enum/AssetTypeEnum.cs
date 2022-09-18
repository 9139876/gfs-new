namespace GFS.QuotesService.Api.Enum;

/// <summary> Тип инструмента </summary>
public enum AssetTypeEnum
{
    /// <summary> Облигации </summary>
    Bonds = 1,

    /// <summary> Валюты </summary>
    Currencies = 2,

    /// <summary> Инвестиционные фонды </summary>
    Etfs = 3,

    /// <summary> Фьючерсы </summary>
    Futures = 4,

    /// <summary> Опционы </summary>
    Options = 5,

    /// <summary> Акции </summary>
    Shares = 6
}