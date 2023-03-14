using GFS.Common.Attributes.Validation;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Api.Common.Enum;

namespace GFS.QuotesService.Api.Models;

public class GetQuotesRequest
{
    public TimeFrameEnum TimeFrame { get; init; }
    
    public Guid AssetId { get; init; }
    
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }
    
    [UtcDate]
    public DateTime StartDate { get; init; }
    
    [UtcDate]
    public DateTime EndDate { get; init; }
}