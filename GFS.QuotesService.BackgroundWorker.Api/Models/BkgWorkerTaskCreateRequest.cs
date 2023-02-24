using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.BackgroundWorker.Api.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models;

/// <summary>
/// Модель для создания задачи для BackgroundWorker`а
/// </summary>
public class BkgWorkerTaskCreateRequest
{
    public Guid? AssetId { get; init; }

    public QuotesProviderTypeEnum QuotesProviderType { get; init; }

    public GetQuotesTaskTypeEnum TaskType { get; init; }
}