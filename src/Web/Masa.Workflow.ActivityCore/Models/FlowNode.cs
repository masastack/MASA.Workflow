namespace Masa.Workflow.ActivityCore;

public class FlowNode
{
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Type { get; set; }

    public FlowNodeData Data { get; set; }
}
