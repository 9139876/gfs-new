namespace GFS.Broker.Api.Models.Portfolio;

public class PendingOrderDto
{
    /// <summary>
    /// Идентификатор актива
    /// </summary>
    public Guid AssetId { get; set; }
    
    /// <summary>
    /// Цена исполнения ордера
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Количество единиц актива
    /// </summary>
    public decimal AssetCount { get; set; }
}