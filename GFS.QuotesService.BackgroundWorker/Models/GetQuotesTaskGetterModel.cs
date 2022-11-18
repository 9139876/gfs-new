using GFS.BkgWorker.Abstraction;
using GFS.QuotesService.Api.Common.Enum;
using GFS.QuotesService.Api.Enum;

namespace GFS.QuotesService.BackgroundWorker.Models;

public class GetQuotesTaskGetterModel : ITaskGetterModel
{
    public QuotesProviderTypeEnum QuotesProviderType { get; init; }
}