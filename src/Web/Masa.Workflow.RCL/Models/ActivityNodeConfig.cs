using Masa.Workflow.ActivityCore.Components;

namespace Masa.Workflow.RCL.Models;

public record ActivityNodeConfig(
    string Type,
    string Name,
    string Color,
    string Icon,
    bool IconRight,
    int MinInput,
    int MinOutput,
    string Tag,
    IReadOnlyList<NodeStateInfo>? States);
