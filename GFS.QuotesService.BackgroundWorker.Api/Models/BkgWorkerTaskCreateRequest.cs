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
}