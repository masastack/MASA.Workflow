namespace Masa.Workflow.Core.Models;

public class ActivityDefinition
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string Name { get; set; }

    public MetaBase Meta { get; set; }

    public RetryPolicy RetryPolicy { get; set; }

    public bool Disabled { get; set; }
}
