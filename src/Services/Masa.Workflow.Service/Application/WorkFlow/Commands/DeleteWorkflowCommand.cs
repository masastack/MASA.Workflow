namespace Masa.Workflow.Service.Application.WorkFlow.Commands;

public record DeleteWorkflowCommand(Guid WorkflowId) : Command
{
}
