namespace Masa.Workflow.Worker.Application.Commands;

public record RunWorkflowCommand(string WorkflowId) : Command
{
}
