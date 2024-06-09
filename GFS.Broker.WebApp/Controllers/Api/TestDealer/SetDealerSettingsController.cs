using GFS.Broker.Api.Interfaces.TestDealer;
using GFS.Broker.Api.Models.TestDealer;
using GFS.Broker.BL.Services;
using GFS.Common.Models;

namespace GFS.Broker.WebApp.Controllers.Api.TestDealer;

public class SetDealerSettingsController : SetDealerSettings
{
    private readonly ISettingsService _settingsService;
    
    public SetDealerSettingsController(
        ILogger<SetDealerSettings> logger, 
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