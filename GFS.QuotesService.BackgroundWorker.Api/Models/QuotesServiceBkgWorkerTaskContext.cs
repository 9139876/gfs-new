using GFS.BackgroundWorker.Models;
using GFS.QuotesService.BackgroundWorker.Api.Enum;
using GFS.QuotesService.Common.Enum;

namespace GFS.QuotesService.BackgroundWorker.Api.Models;

/// <summary>
/// Контекст задачи для BackgroundWorker`а QuotesService
/// </summary>
public class QuotesServiceBkgWorkerTaskContext : IBkgWorkerTaskContext
{
    public Guid? AssetId { get; init; }

    public QuotesProviderTypeEnum QuotesProviderType { get; init; }

    public GetQuotesTaskTypeEnum TaskType { get; init; }

    private bool NeedAssetId()
        => TaskType != GetQuotesTaskTypeEnum.GetInitialData;

    public bool IsValid()
    {
        if (NeedAssetId() && !AssetId.HasValue)
            return false;

        if (!System.Enum.IsDefined(typeof(QuotesProviderTypeEnum), QuotesProviderType))
            return false;

        if (!System.Enum.IsDefined(typeof(GetQuotesTaskTypeEnum), TaskType))
            return false;

        return true;
    }

    public int GetPriority() => (int)TaskType;
}