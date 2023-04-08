using GFS.Broker.Api.Interfaces.TestDealer;
using GFS.Broker.Api.Models.TestDealer;
using GFS.Broker.BL.Services;

namespace GFS.Broker.WebApp.Controllers.Api.TestDealer;

public class GetDealerSettingsController : GetDealerSettings
{
    private readonly ISettingsService _settingsService;

    public GetDealerSettingsController(
        ILogger logger,
        ISettingsService settingsService) : base(logger)
    {
        _settingsService = settingsService;
    }

    protected override Task<DealerSettings> ExecuteInternal()
    {
        return Task.FromResult(_settingsService.GetDealerSettings());
    }
}