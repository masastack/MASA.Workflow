namespace Masa.Workflow.Core.Models;

//TODO move to worker library
public class WorkflowInstance
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Variables Variables { get; set; } = new();

    public ICollection<ActivityDefinition> Activities { get; set; } = new List<ActivityDefinition>();
}
