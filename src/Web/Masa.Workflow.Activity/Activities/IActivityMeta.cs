namespace Masa.Workflow.Activity.Activities;

public interface IActivityMeta
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Type { get; set; }

    public bool Disabled { get; set; }

    public string Property { get; set; }

    public List<List<Guid>> Wires { get; set; }

    public string Icon { get; set; }

    public bool ShowLabel { get; set; }

    public List<string> InputLabels { get; set; }

    public List<string> OutputLabels { get; set; }

    public int Output { get; set; }
}

