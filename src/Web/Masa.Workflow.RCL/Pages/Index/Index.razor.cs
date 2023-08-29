﻿using System.Text.Json;

namespace Masa.Workflow.RCL.Pages;

public partial class Index
{
    [Inject] private DrawflowService DrawflowService { get; set; } = null!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    [Parameter] public Guid ActivityId { get; set; }

    private MDrawflow _drawflow = null!;

    private List<StringNumber>? _selectedGroups;
    private StringNumber? _node;

    private List<TreeNode> _tree = new()
    {
        new TreeNode("workflow", "sitemap-outline", new())
    };

    private string _data;

    private Block Block => new("mw-flow");

    private List<Node> _nodes = new();
    private List<IGrouping<string?, Node>> _nodeGroups = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (ActivityId != default)
            {
                //TODO: fetch data from http
                // await _drawflow.ImportAsync();
            }

            // TODO: 多个窗口会问题吗？共享的是同一个 drawflow 吗？
            DrawflowService.SetDrawflow(_drawflow);

            // TODO: fetch nodes from http
            _nodes = new List<Node>()
            {
                new("Switch", "switch", "#e2d96e", "mdi-shuffle", "func"),
                new("Http Request", "http-request", "#e7e7ae", "mdi-earth", "net"),
            };

            _nodeGroups = _nodes.GroupBy(u => u.Tag).ToList();

            _selectedGroups = _nodeGroups.Select(g => (StringNumber)g.Key).ToList();

            StateHasChanged();
        }
    }

    private async Task Drop(ExDragEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.DataTransfer.Data.Value))
        {
            throw new InvalidOperationException("data-value cannot be found in DataTransfer.");
        }

        var nodeName = args.DataTransfer.Data.Value;

        var tagName = $"{nodeName}-node";

        var node = _nodes.FirstOrDefault(u => u.Id == nodeName);

        // TODO: get init meta of node
        var minInput = 1;
        var minOutput = 1;
        var metaNode = new Node2(node.Name, node.Color, node.Icon, node.IconRight, false, minInput, minOutput, "");

        var id = await _drawflow.AddNodeAsync(
            nodeName,
            minInput,
            minOutput,
            clientX: args.ClientX,
            clientY: args.ClientY,
            offsetX: args.DataTransfer.Data.OffsetX,
            offsetY: args.DataTransfer.Data.OffsetY,
            className: "",
            new { data = JsonSerializer.Serialize(metaNode) },
            $"<{tagName} df-data></{tagName}>");

        await _drawflow.UpdateNodeHTMLAsync(id, $"<{tagName} node-id='{id}' df-data></{tagName}>");
        await JSRuntime.InvokeVoidAsync(JSInteropConstants.SetNodeIdToCustomElement, id);

        _tree[0].Children.Add(new TreeNode(id.ToString(), node.Name, node.Icon, node.Color));
    }

    private async Task NodeCreated(string nodeId)
    {
        Console.Out.WriteLine("NodeCreated NodeId = {0}", nodeId);
    }

    private async Task NodeRemoved(string nodeId)
    {
        Console.Out.WriteLine("NodeRemoved NodeId = {0}", nodeId);
    }

    private async Task NodeDataChanged(string nodeId)
    {
        Console.Out.WriteLine("NodeDataChanged NodeId = {0}", nodeId);

        var node = await _drawflow.GetNodeFromIdAsync<NodeData>(nodeId);

        var treeNode = _tree[0].Children.FirstOrDefault(u => u.Key == node.Id);
        if (treeNode != null)
        {
            using var document = JsonDocument.Parse(node.Data.Data);
            treeNode.Label = document.RootElement.GetProperty("Name").GetString();
        }
    }

    private async Task Export()
    {
        var data = await _drawflow.ExportAsync(withoutData: false);
        Console.WriteLine($"export result: {data}");
        _data = data;
    }

    private async Task Import(string json)
    {
        await _drawflow.ImportAsync(json);

        if (string.IsNullOrWhiteSpace(json))
        {
            return;
        }

        _tree[0].Children!.Clear();

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        foreach (var e in root.GetProperty("drawflow").GetProperty("Home").GetProperty("data").EnumerateObject())
        {
            var id = e.Value.GetProperty("id").GetRawText();
            var name = e.Value.GetProperty("name").GetString();
            var data = e.Value.GetProperty("data").GetProperty("data").GetString();
            var node = JsonSerializer.Deserialize<Node>(data);

            if (node is null)
            {
                continue;
            }

            node.Id = id;
            node.Name = name;

            _tree[0].Children!.Add(new TreeNode(node.Id, node.Name, node.Icon, node.Color));
        }
    }

    public class Node
    {
        public string? Name { get; set; }

        public string? Id { get; set; }

        public string? Color { get; init; }

        public string? Icon { get; init; }

        public string? Tag { get; init; }

        public bool IconRight { get; init; }

        public Node()
        {
        }

        public Node(string name, string id, string color, string icon, string tag, bool iconRight = false)
        {
            Name = name;
            Id = id;
            Color = color;
            Icon = icon;
            Tag = tag;
            IconRight = iconRight;
        }
    }

    public record Node2(string Name, string Color, string? Icon, bool IconRight, bool HideLabel, int MinInput, int MinOutput, string Meta);
}
