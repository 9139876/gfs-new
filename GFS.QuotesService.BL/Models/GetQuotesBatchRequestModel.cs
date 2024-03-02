using GFS.GrailCommon.Enums;
using GFS.QuotesService.BL.Enum;
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
    public TimeDirectionEnum TimeDirection { get; init; }
    public DateTime BatchBeginningDate { get; set; }
}