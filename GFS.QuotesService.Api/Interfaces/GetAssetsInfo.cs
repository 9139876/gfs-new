using GFS.Api.Services;
using GFS.QuotesService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFS.QuotesService.Api.Interfaces;

[Route(nameof(GetAssetsInfo))]
public abstract class GetAssetsInfo: ApiServiceWithRequestResponse<AssetsFilter, List<AssetsInfoDto>>
{
    protected GetAssetsInfo(ILogger logger) : base(logger)
    {
    }
}