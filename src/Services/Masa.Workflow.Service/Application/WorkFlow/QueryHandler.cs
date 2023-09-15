using Google.Protobuf.WellKnownTypes;

namespace Masa.Workflow.Service.Application.WorkFlow;

public class QueryHandler
{
    readonly IWorkflowRepository _orderRepository;

    public QueryHandler(IWorkflowRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [EventHandler]
    public async Task WorkFlowListHandleAsync(WorkFlowListQuery query)
    {
        //todo work
    }

    [EventHandler]
    public async Task WorkFlowDetailHandleAsync(WorkFlowDetailQuery query)
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
        query.Result.EnvironmentVariables.Add("1", new Any
        {
            Value = Google.Protobuf.ByteString.CopyFromUtf8("1")
        });
        query.Result.Nodes.Add(new Node
        {
            Name = "Test Node",
            Disabled = false,
            Point = new Node.Types.Point()
            {
                X = 1,
                Y = 2
            },
            Meta = new Any
            {
                Value = Google.Protobuf.ByteString.CopyFromUtf8("1")
            }
        });
    }
}