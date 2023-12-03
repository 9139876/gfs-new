using GFS.Api.Client.Services;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;

namespace GFS.ChartService.BL.Services;

public interface IQuotesService
{
    Task<List<AssetsInfoDto>> GetAssetsInfo(AssetsFilter request);
    Task<AssetQuotesInfoDto> GetAssetTimeFrameQuotesInfo(Guid assetId);
}

internal class QuotesService : IQuotesService
{
    private readonly IRemoteApiClient _remoteApiClient;

    public QuotesService(
        IRemoteApiClient remoteApiClient)
    {
        _remoteApiClient = remoteApiClient;
    }

    public async Task<List<AssetsInfoDto>> GetAssetsInfo(AssetsFilter request)
    {
        return await _remoteApiClient.Call<GetAssetsInfo, AssetsFilter, List<AssetsInfoDto>>(request);
    }

    public async Task<AssetQuotesInfoDto> GetAssetTimeFrameQuotesInfo(Guid assetId)
    {
        return await _remoteApiClient.Call<GetAssetQuotesInfo, Guid, AssetQuotesInfoDto>(assetId);
    }
}