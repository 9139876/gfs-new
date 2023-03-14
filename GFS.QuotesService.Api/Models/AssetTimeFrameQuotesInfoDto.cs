using GFS.Common.Attributes.Validation;
using GFS.GrailCommon.Enums;

namespace GFS.QuotesService.Api.Models;

public class AssetTimeFrameQuotesInfoDto
{
    public TimeFrameEnum TimeFrame { get; init; }
    
    public int Count { get; init; }
    
    [UtcDate]
    public DateTime FirstDate { get; set; }
    
    [UtcDate]
    public DateTime LastDate { get; set; }
}