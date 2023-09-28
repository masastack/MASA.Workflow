namespace Masa.Workflow.Core.Models;

public class WorkflowInstance
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ContextVariables ContextVariables { get; set; } = new();

    public ICollection<ActivityDefinition> Activities { get; set; } = new List<ActivityDefinition>();

    public StartActivity StartActivity { get; set; }
}

public record StartActivity(Guid ActivityId);