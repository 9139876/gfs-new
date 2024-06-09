using GFS.GrailCommon.Enums;
using GFS.QuotesService.Common.Enum;
using GFS.QuotesService.DAL.Entities;

#pragma warning disable CS8618

namespace GFS.QuotesService.BL.Models;

public class GetQuotesRequestModel
{
    public AssetEntity Asset { get; init; }
    public TimeFrameEnum TimeFrame { get; init; }
}

/// <summary>
/// Модель запроса получения партии котировок
/// </summary>
public class GetQuotesBatchRequestModel : GetQuotesRequestModel
{
    public DateTime BatchBeginningDate { get; set; }
}

public class GetQuotesBatchRequestModel2
{
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }
    public Guid AssetId { get; init; }
    public TimeFrameEnum TimeFrame { get; init; }
    public DateTime LastQuoteDate { get; init; }
}

public class GetQuotesBatchResponseModel2
{
    public List<QuoteEntity> Quotes { get; init; }
    public bool IsLastBatch { get; init; }
}