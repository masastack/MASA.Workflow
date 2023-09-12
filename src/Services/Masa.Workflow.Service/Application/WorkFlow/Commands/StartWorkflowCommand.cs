namespace Masa.Workflow.Service.Application.WorkFlow.Commands;

public record StartWorkflowCommand(Guid WorkflowId) : DomainCommand
{
}
