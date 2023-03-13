using GFS.FakeDealer.Api.Interfaces;
using GFS.FakeDealer.Api.Models;
using GFS.FakeDealer.BL.Services;

namespace GFS.FakeDealer.WebApp.Controllers.Api;

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