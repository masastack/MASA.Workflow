using System.Text.Json.Serialization;

namespace Masa.Workflow.ActivityCore;

public class ActivityMeta
{
    public string? Name { get; set; }

    public string? Color { get; set; }

    public string? Description { get; set; }

    public string Type { get; set; } = null!;

    public bool Active { get; set; } = true;

    public string? Property { get; set; }

    public List<List<Guid>>? Wires { get; set; }

    public string Icon { get; set; } = null!;

    public bool IconRight { get; set; }

    public bool ShowLabel { get; set; } = true;

    public int MinInput { get; set; }

    public int MinOutput { get; set; }

    public List<string?> InputLabels { get; set; } = new();

    public List<string?> OutputLabels { get; set; } = new();

    [JsonIgnore]
    public string DisplayName => Name ?? Type;

    public string Meta { get; set; }
}

public class ActivityMeta<TData> : ActivityMeta where TData : class, new()
{
    [JsonIgnore]
    [ValidateComplexType]
    public TData MetaData { get; set; } = new();
}
