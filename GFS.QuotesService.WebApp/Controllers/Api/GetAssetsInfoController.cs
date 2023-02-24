using AutoMapper;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.WebApp.Controllers.Api;

public class GetAssetsInfoController : GetAssetsInfo
{
    private readonly IGetDataService _getDataService;
    private readonly IMapper _mapper;

    public GetAssetsInfoController(
        ILogger logger,
        IGetDataService getDataService,
        IMapper mapper) : base(logger)
    {
        _getDataService = getDataService;
        _mapper = mapper;
    }

    protected override async Task<List<AssetsInfoDto>> ExecuteInternal(AssetsFilter request)
    {
        var assets = await _getDataService.GetAssetsInfo(request);
        return _mapper.Map<List<AssetsInfoDto>>(assets);
    }
}