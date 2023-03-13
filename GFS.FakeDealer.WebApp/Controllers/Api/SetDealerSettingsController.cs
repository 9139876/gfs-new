using GFS.Common.Models;
using GFS.FakeDealer.Api.Interfaces;
using GFS.FakeDealer.Api.Models;
using GFS.FakeDealer.BL.Services;

namespace GFS.FakeDealer.WebApp.Controllers.Api;

public class SetDealerSettingsController : SetDealerSettings
{
    private readonly ISettingsService _settingsService;
    
    public SetDealerSettingsController(
        ILogger logger, 
        ISettingsService settingsService) : base(logger)
    {
        _settingsService = settingsService;
    }

    protected override Task<StandardResponse> ExecuteInternal(DealerSettings request)
    {
        try
        {
             _settingsService.SetDealerSettings(request);
            return Task.FromResult(StandardResponse.GetSuccessResponse());
        }
        catch (Exception ex)
        {
            return Task.FromResult(StandardResponse.GetFailResponse(ex.Message));
        }
    }
}