namespace Masa.Workflow.RCL.Models;

public class WorkflowActivityRegistered
{
    public Dictionary<string, string> Locales { get; set; } = new();

    public ActivityNodeConfig? Config { get; set; }
}
