namespace Masa.Workflow.Core.Models;

public record RunWorkflowResult(string InstanceId, Exception? Exception, WorkflowStatus Status);
