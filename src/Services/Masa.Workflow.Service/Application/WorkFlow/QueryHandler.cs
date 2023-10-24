namespace Masa.Workflow.Service.Application.WorkFlow;

public class QueryHandler
{
    readonly IWorkflowRepository _workflowRepository;

    public QueryHandler(IWorkflowRepository workflowRepository)
    {
        _workflowRepository = workflowRepository;
    }

    [EventHandler]
    public async Task WorkFlowListHandleAsync(WorkflowListQuery query)
    {
        Expression<Func<Flow, bool>> condition = flow => true;
        condition = condition.And(!string.IsNullOrEmpty(query.Request.Name), flow => flow.Name.Contains(query.Request.Name));

        var data = await _workflowRepository.GetPaginatedListAsync(condition, new PaginatedOptions { Page = query.Request.Page, PageSize = query.Request.PageSize });

        var result = new PagedWorkflowList()
        {
            Total = data.Total,
            TotalPage = data.TotalPages
        };

        foreach (var item in data.Result)
        {
            result.Items.Add(new WorkflowItem
            {
                Name = item.Name,
                Id = item.Id.ToString()
                //todo
            });
        }

        query.Result = result;
    }

    [EventHandler]
    public async Task WorkFlowDetailHandleAsync(WorkflowDetailQuery query)
    {
        var workflow = await _workflowRepository.GetAsync(query.Id, nameof(Flow.Versions));
        query.Result = new WorkflowDetail
        {
            Id = workflow.Id.ToString(),
            Name = workflow.Name,
            Description = workflow.Description,
            Status = WorkflowStatus.Idle,
            NodeJson = workflow.LastVersion?.Json ?? "",
            CreateUser = "admin",
            CreateDateTimeStamp = DateTime.UtcNow.ToTimestamp()
        };
        query.Result.EnvironmentVariables.Add(workflow.EnvironmentVariables);
    }

    [EventHandler]
    public async Task WorkFlowDefinitionHandleAsync(WorkflowDefinitionQuery query)
    {
        var workflow = await _workflowRepository.GetAsync(query.Id, nameof(Flow.Activities));

        query.Result = new WorkflowDefinition
        {
            Id = workflow.Id.ToString(),
            Name = workflow.Name
        };
        query.Result.EnvironmentVariables.Add(workflow.EnvironmentVariables);
        foreach (var activity in workflow.Activities)
        {
            var node = new Activity
            {
                Id = activity.Id.ToString(),
                Name = activity.Name,
                Type = activity.Type,
                Disabled = activity.Disabled,
                Meta = JsonSerializer.Serialize(activity.Meta),
                RetryPolicy = new Activity.Types.RetryPolicy
                {
                    MaxNmberOfAttempts = activity.RetryPolicy.MaxNumberOfAttempts,
                    FirstRetryInterval = Duration.FromTimeSpan(activity.RetryPolicy.FirstRetryInterval),
                    MaxRetryInterval = Duration.FromTimeSpan(activity.RetryPolicy.MaxRetryInterval),
                    BackoffCoefficient = activity.RetryPolicy.BackoffCoefficient,
                    RetryTimeout = Duration.FromTimeSpan(activity.RetryPolicy.RetryTimeout)
                }
            };
            node.Wires.AddRange(activity.Wires.Select(wire =>
            {
                var wires = new Activity.Types.Wire();
                wires.Guids.AddRange(wire.ConvertAll(w => w.ToString()));
                return wires;
            }));

            query.Result.Activities.Add(node);
        }
    }
}