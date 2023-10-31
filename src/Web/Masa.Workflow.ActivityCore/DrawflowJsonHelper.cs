namespace Masa.Workflow.ActivityCore;

public static class DrawflowJsonHelper
{
    public static List<FlowNode> ParseToFlowNodeList(string json)
    {
        var list = new List<FlowNode>();

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        foreach (var e in root.GetProperty("drawflow").GetProperty("Home").GetProperty("data").EnumerateObject())
        {
            var id = e.Value.GetProperty("id").GetRawText();
            var name = e.Value.GetProperty("name").GetString();
            var data = e.Value.GetProperty("data").GetProperty("data").GetString();
            var nodeData = JsonSerializer.Deserialize<FlowNodeData>(data);
            list.Add(new FlowNode()
            {
                Id = id,
                Type = name,
                Data = nodeData
            });
        }

        return list;
    }

    public static T GetNodeData<T>(string json, string nodeId)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        var rawText = root.GetProperty("drawflow").GetProperty("Home").GetProperty("data").GetProperty(nodeId).GetProperty("data")
                          .GetProperty("data");
        return JsonSerializer.Deserialize<T>(rawText.GetString(), new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false
        })!;
    }

    /// <summary>
    /// Check if the json contains the node named "debug"
    /// </summary>
    /// <param name="json">Json exported from drawflow</param>
    /// <returns></returns>
    public static bool ContainsDebugNode(string json)
    {
        using var objectEnumerator = GetJsonElementObjectEnumerator(json);
        return objectEnumerator.Any(o => o.Value.GetProperty("name").GetString() == "debug");
    }
    
    private static JsonElement.ObjectEnumerator GetJsonElementObjectEnumerator(string json)
    {
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        return root.GetProperty("drawflow").GetProperty("Home").GetProperty("data").EnumerateObject();
    }
}
