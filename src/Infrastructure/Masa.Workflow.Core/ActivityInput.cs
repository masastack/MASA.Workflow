namespace Masa.Workflow.Core;

public class ActivityInput<TMeta>
    where TMeta : class, new()
{
    public TMeta Meta { get; set; } = new();
}
