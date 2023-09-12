namespace Masa.Workflow.Service.Services;

public class WorkflowService : ServiceBase
{
    public WorkflowService()
    {
    }

    [Authorize]
    public async Task<IResult> QueryList(WorkflowDomainService orderDomainService)
    {
        var orders = await orderDomainService.QueryListAsync();
        return Results.Ok(orders);
    }

    [Authorize]
    public async Task<IResult> PlaceOrder(IEventBus eventBus)
    {
        var comman = new CreateWorkflowCommand();
        await eventBus.PublishAsync(comman);
        return Results.Ok();
    }
}

public class WorkflowGrpcService : WorkflowServiceBase
{
    private readonly IEventBus _eventBus;

    public WorkflowGrpcService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public override Task<WorkflowDetail> GetDetail(WorkflowId request, ServerCallContext context)
    {
        var result = new WorkflowDetail();
        result.Name = "33333";
        return Task.FromResult(result);
    }

    public override Task<WorkflowReply> GetList(WorkflowRequest request, ServerCallContext context)
    {
        return base.GetList(request, context);
    }

    public override async Task<StatusResponse> Start(WorkflowId request, ServerCallContext context)
    {
        await _eventBus.PublishAsync(new StartWorkflowCommand(Guid.NewGuid()));
        return new StatusResponse();
    }
}