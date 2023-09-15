namespace Masa.Workflow.Worker.Application.Commands;

public record StartWorkflowCommand(string WorkflowId) : Command
{
}
