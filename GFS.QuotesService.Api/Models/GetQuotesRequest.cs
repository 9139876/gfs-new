using GFS.Common.Attributes.Validation;
using GFS.GrailCommon.Enums;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.Api.Models;

public class GetQuotesRequest
{
    public TimeFrameEnum TimeFrame { get; init; }
    
    public Guid AssetId { get; init; }

    public QuotesProviderTypeEnum QuotesProviderType { get; init; } = QuotesProviderTypeEnum.Tinkoff;
    
    [NullableUtcDate]
    public DateTime? StartDate { get; init; }
    
    [NullableUtcDate]
    public DateTime? EndDate { get; init; }
}