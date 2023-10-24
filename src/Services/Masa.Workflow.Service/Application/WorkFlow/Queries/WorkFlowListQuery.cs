namespace Masa.Workflow.Service.Application.WorkFlow.Queries;

public record WorkflowListQuery(WorkflowListRequest Request) : DomainQuery<PagedWorkflowList>
{
    public override PagedWorkflowList Result { get; set; } = new();
}