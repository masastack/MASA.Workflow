using System.Linq.Expressions;
using System.Text.Json;

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

        var result = new WorkflowReply
        {
            Total = data.Total,
            TotalPage = data.TotalPages
        };

        foreach (var item in data.Result)
        {
            result.Workflows.Add(new WorkflowItem
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
        query.Result = new WorkflowDetail
        {
            Id = query.Id.ToString(),
            Name = "Test Flow",
            Description = "Hard Code",
            Status = WorkflowStatus.Idle,
            CreateUser = "admin",

            CreateDateTimeStamp = DateTime.UtcNow.ToTimestamp()
        };
        query.Result.EnvironmentVariables.Add("1", "test");
    }

    [EventHandler]
    public async Task WorkFlowDefinitionHandleAsync(WorkflowDefinitionQuery query)
    {
        query.Result = new WorkflowDefinition
        {
            Id = query.Id.ToString(),
            Name = "Test Flow",
        };
        query.Result.EnvironmentVariables.Add("1", "test");
        query.Result.Nodes.Add(new Node
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Node",
            Type = "Console",
            Disabled = false,
            Meta = JsonSerializer.Serialize(new
            {
                Text = "Server return data"
            }),
            RetryPolicy = new RetryPolicy(),
        });
    }
}