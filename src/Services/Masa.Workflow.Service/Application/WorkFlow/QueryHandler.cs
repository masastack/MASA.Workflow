namespace Masa.Workflow.Service.Application.Orders;

public class QueryHandler
{
    readonly IWorkflowRepository _orderRepository;
    public QueryHandler(IWorkflowRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [EventHandler]
    public async Task OrderListHandleAsync(WorkFlowListQuery query)
    {
        //query.Result = await _orderRepository.GetListAsync();
    }
}