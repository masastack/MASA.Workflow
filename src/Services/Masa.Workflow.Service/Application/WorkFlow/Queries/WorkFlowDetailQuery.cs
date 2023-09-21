namespace Masa.Workflow.Service.Application.WorkFlow.Queries;

public record WorkflowDetailQuery(Guid Id) : DomainQuery<WorkflowDetail>
{
    public override WorkflowDetail Result { get; set; } = new();
}
