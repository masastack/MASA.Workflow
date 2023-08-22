namespace Masa.Workflow.Service.Application.Orders.Queries;

public record WorkFlowListQuery : DomainQuery<List<WorkflowItem>>
{
    public override List<WorkflowItem> Result { get; set; } = new();
}