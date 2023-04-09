namespace GFS.Broker.Api.Models.TestDealer;

public class TryPerformPendingOrdersRequest
{
    /// <summary>
    /// Идентификатор портфеля, для которого проверяются отложенные ордеры
    /// </summary>
    public Guid PortfolioId { get; init; }

    public List<AssetWithQuote> AssetsWithQuotes { get; init; } = new();
}