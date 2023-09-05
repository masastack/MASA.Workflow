using BlazorComponent.I18n;
using Microsoft.Extensions.Options;

namespace Masa.Workflow.RCL.Pages;

public partial class Index
{
    [Inject] private I18n I18n { get; set; } = null!;

    [Inject] private DrawflowService DrawflowService { get; set; } = null!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] private IOptions<WorkflowActivitiesRegistered> RegisteredActivities { get; set; } = null!;

    [Parameter] public Guid ActivityId { get; set; }

    private MDrawflow _drawflow = null!;

    private List<StringNumber>? _selectedGroups;
    private StringNumber? _node;
    private bool _helpDrawer;
    private string? _helpMarkdown;
    private List<string> _treeActives = new();

    private List<TreeNode> _tree = new()
    {
        new TreeNode("workflow", "sitemap-outline", new())
    };

    private string _data;

    private List<ActivityNodeConfig> _nodes = new();
    private List<IGrouping<string?, ActivityNodeConfig>> _nodeGroups = new();

    private TreeNode? ActiveTreeNode
    {
        get
        {
            var activeKey = _treeActives.FirstOrDefault();
            // TODO: 节点的数据从哪里来？
            if (_tree.Count > 0)
            {
                return _tree[0].Children?.FirstOrDefault(u => u.Key == activeKey);
            }

            return null;
        }
    }

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

            _nodes = RegisteredActivities.Value.Select(u => u.Config).ToList();

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

        var nodeType = args.DataTransfer.Data.Value;

        var tagName = $"{nodeType}-node";

        var node = _nodes.FirstOrDefault(u => u.Type == nodeType);

        // TODO: get init meta of node
        var minInput = 1;
        var minOutput = 1;
        var metaNode = new Node2(Guid.NewGuid(), node.Type, node.Color, node.Icon, node.IconRight, false, minInput, minOutput, "");

        var id = await _drawflow.AddNodeAsync(
            nodeType,
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

        var treeNode = new TreeNode(id.ToString(), node.Type, node.Icon, node.Color);
        treeNode.Extra["nodeType"] = node.Type;
        _tree[0].Children.Add(treeNode);
    }

    private void NodeCreated(string nodeId)
    {
        Console.Out.WriteLine("NodeCreated NodeId = {0}", nodeId);
    }

    private void NodeRemoved(string nodeId)
    {
        Console.Out.WriteLine("NodeRemoved NodeId = {0}", nodeId);
        var treeNode = _tree[0].Children.FirstOrDefault(u => u.Key == nodeId);
        if (treeNode != null)
        {
            _tree[0].Children.Remove(treeNode);
        }
    }

    private void NodeSelected(string nodeId)
    {
        _treeActives = new List<string>() { nodeId };
    }

    private void NodeUnselected(string nodeId)
    {
        _treeActives.Clear();
    }

    private async Task NodeDataChanged(string nodeId)
    {
        Console.Out.WriteLine("NodeDataChanged NodeId = {0}", nodeId);

        var node = await _drawflow.GetNodeFromIdAsync<JustNodeData>(nodeId);

        var treeNode = _tree[0].Children.FirstOrDefault(u => u.Key == node.Id);
        if (treeNode != null)
        {
            using var document = JsonDocument.Parse(node.Data.Data);
            treeNode.Label = document.RootElement.GetProperty("Name").GetString();
        }
    }

    private async Task OpenHelpDrawer(string nodeType)
    {
        var activity = RegisteredActivities.Value.FirstOrDefault(u => u.Config.Type == nodeType);
        var locale = activity.Locales.Keys.FirstOrDefault(u => u.Equals(I18n.Culture.Name, StringComparison.OrdinalIgnoreCase));
        if (locale is null)
        {
            _helpMarkdown = "Locale not found.";
        }
        else
        {
            _helpMarkdown = activity.Locales[locale];

            if (string.IsNullOrWhiteSpace(_helpMarkdown))
            {
                _helpMarkdown = "Help not found.";
            }
        }

        _helpDrawer = true;
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
            var nodeData = JsonSerializer.Deserialize<FlowNodeData>(data);

            if (nodeData is null)
            {
                continue;
            }

            _tree[0].Children!.Add(new TreeNode(nodeData.Id.ToString(), nodeData.Name, nodeData.Icon, nodeData.Color));
        }
    }

    public class Node
    {
        public string? NodeId { get; set; }

        public string? NodeType { get; set; }

        public string? NodeName { get; set; }

        public FlowNodeData? Data { get; set; }
    }

    public record Node2(Guid Id, string Name, string Color, string? Icon, bool IconRight, bool HideLabel, int MinInput, int MinOutput, string Meta);
}
