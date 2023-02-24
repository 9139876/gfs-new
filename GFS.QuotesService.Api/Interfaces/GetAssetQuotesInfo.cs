using GFS.Api.Services;
using GFS.QuotesService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.Api.Interfaces;

[Route(nameof(GetAssetQuotesInfo))]
public abstract class GetAssetQuotesInfo : ApiServiceWithRequestResponse<Guid, AssetQuotesInfoDto>
{
    protected GetAssetQuotesInfo(ILogger logger) : base(logger)
    {
    }
}