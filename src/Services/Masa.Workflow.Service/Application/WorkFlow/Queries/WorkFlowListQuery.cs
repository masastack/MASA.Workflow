namespace Masa.Workflow.Service.Application.WorkFlow.Queries;

public record WorkflowListQuery : DomainQuery<List<WorkflowItem>>
{
    public override List<WorkflowItem> Result { get; set; } = new();
}