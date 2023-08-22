namespace Masa.Workflow.Service.Application.WorkFlow.Queries;

public record WorkFlowDetailQuery : DomainQuery<WorkflowDetail>
{
    public override WorkflowDetail Result { get; set; } = new();
}
