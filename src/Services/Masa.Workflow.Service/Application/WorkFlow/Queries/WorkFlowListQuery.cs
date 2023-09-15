namespace Masa.Workflow.Service.Application.WorkFlow.Queries;

public record WorkFlowListQuery : DomainQuery<List<WorkflowItem>>
{
    public override List<WorkflowItem> Result { get; set; } = new();
}