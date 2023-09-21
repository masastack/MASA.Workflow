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
        var st  = rawText.GetString();
        Console.Out.WriteLine("rawText = {0}", st);
        return JsonSerializer.Deserialize<T>(st, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false
        })!;
    }

}
