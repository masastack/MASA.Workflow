namespace Masa.Workflow.Service.Application.WorkFlow.Queries;

public record WorkflowListQuery(WorkflowListRequest Request) : DomainQuery<WorkflowReply>
{
    public override WorkflowReply Result { get; set; } = new();
}