namespace Masa.Workflow.Core.Models;

public record RunWorkflowResult
{
    public string InstanceId { get; set; }
    public Exception? Exception { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WorkflowStatus Status { get; set; }

    public RunWorkflowResult(string instanceId, Exception? exception, WorkflowStatus status)
    {
        InstanceId = instanceId;
        Exception = exception;
        Status = status;
    }
}