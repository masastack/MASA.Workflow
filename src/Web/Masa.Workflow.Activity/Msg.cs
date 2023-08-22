namespace Masa.Workflow.Activity;

public sealed class Msg<TMeta> : DynamicObject where TMeta : new()
{
    public Guid ActivityId { get; set; }

    public ExpandoObject Payload { get; set; } = new ExpandoObject();

    public TMeta Meta { get; set; } = new TMeta();
}


