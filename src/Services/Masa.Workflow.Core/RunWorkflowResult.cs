namespace Masa.Workflow.Core;

public record RunWorkflowResult(string InstanceId, Exception? Exception, bool Executed);
