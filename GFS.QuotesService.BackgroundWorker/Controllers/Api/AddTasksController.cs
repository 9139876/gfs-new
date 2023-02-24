using AutoMapper;
using GFS.Common.Models;
using GFS.QuotesService.BackgroundWorker.Api.Interfaces;
using GFS.QuotesService.BackgroundWorker.Api.Models;
using GFS.QuotesService.BackgroundWorker.Api.Models.RequestResponse;
using GFS.QuotesService.BackgroundWorker.Execution;

namespace GFS.QuotesService.BackgroundWorker.Controllers.Api;

public class AddTasksController : AddTasks
{
    private readonly IMapper _mapper;

    public AddTasksController(
        ILogger logger,
        IMapper mapper) : base(logger)
    {
        _mapper = mapper;
    }

    protected override Task<StandardResponse> ExecuteInternal(AddTasksRequest request)
    {
        TasksStorage.AddTasks(_mapper.Map<List<BkgWorkerTask>>(request.Tasks));
        return Task.FromResult(StandardResponse.GetSuccessResponse());
    }
}