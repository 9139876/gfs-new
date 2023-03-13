using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.Api.Models;

public class GetQuotesRequest
{
    public TimeFrameEnum TimeFrame { get; init; }
    
    public Guid AssetId { get; init; }
    
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }
    
    public DateTime StartDate { get; init; }
    
    public DateTime EndDate { get; init; }
}