using GFS.Api.Services;
using GFS.QuotesService.Api.Interfaces;
using GFS.QuotesService.Api.Models;
using GFS.QuotesService.BL.Services;

namespace GFS.QuotesService.WebApp.Controllers.Api;

public class AddGetQuotesTaskController : AddGetQuotesTask
{
    private readonly IQuotesWorkerTasksService _quotesWorkerTasksService;


    public AddGetQuotesTaskController(
        IQuotesWorkerTasksService quotesWorkerTasksService,
        ILogger<ApiServiceWithRequest<AddGetQuotesTaskRequest>> logger) : base(logger)
    {
        _quotesWorkerTasksService = quotesWorkerTasksService;
    }

    protected override async Task ExecuteInternal(AddGetQuotesTaskRequest request)
    {
        await _quotesWorkerTasksService.AddGetQuotesTask(request);
    }
}