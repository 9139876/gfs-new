using GFS.GrailCommon.Enums;

namespace GFS.QuotesService.Api.Models;

public class AssetTimeFrameQuotesInfoDto
{
    public TimeFrameEnum TimeFrame { get; init; }
    
    public int Count { get; init; }
    
    public DateTime FirstDate { get; set; }
    
    public DateTime LastDate { get; set; }
}