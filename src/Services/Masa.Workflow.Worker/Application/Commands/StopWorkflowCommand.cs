namespace Masa.Workflow.Worker.Application.Commands;

public record StopWorkflowCommand(string WorkflowId) : Command
{
}
