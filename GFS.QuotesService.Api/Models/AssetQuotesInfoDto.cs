namespace GFS.QuotesService.Api.Models;

public class AssetQuotesInfoDto
{
    public List<AssetTimeFrameQuotesInfoDto> AssetTimeFrameQuotesInfos { get; init; } = new();
    
    public decimal MinPrice { get; init; }
    public decimal MaxPrice { get; init; }
}