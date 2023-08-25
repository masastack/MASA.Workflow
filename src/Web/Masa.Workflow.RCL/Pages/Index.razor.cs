namespace Masa.Workflow.RCL.Pages;

public partial class Index
{
    [Inject] private DrawflowService DrawflowService { get; set; } = null!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    private MDrawflow _drawflow = null!;

    private List<StringNumber>? _selectedGroups;

    private Block Block => new("mw-flow");

    private async Task Export()
    {
        var res = await _drawflow.ExportAsync(withoutData: false);
        Console.WriteLine($"export result: {res}");
    }

    private List<Node> _nodes = new();
    private List<IGrouping<string?, Node>> _nodeGroups = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
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


        var input = 1;
        var output = 1;
        Node2 metaNode = new Node2(node.Name, node.Icon, node.IconRight, false, input, output, new string[] { }, new string[] { }, "");

        var str = JsonSerializer.Serialize(metaNode);

        Console.Out.WriteLine(str);

        var id = await _drawflow.AddNodeAsync(
            nodeName,
            input,
            output,
            clientX: args.ClientX,
            clientY: args.ClientY,
            offsetX: args.DataTransfer.Data.OffsetX,
            offsetY: args.DataTransfer.Data.OffsetY,
            className: "",
            new { data = JsonSerializer.Serialize(metaNode) },
            $"<{tagName} color='{node.Color}' df-data></{tagName}>");

        await JSRuntime.InvokeVoidAsync("updateCustomElementIdFromDrawflow", tagName, id);
    }


    public record Node(string Name, string Id, string Color, string? Icon = null, string? Tag = null, bool IconRight = false);

    public record Node2(string Name, string? Icon, bool IconRight, bool HideLabel, int Input, int Output, string[] Inputs, string[] Outputs,
        string Meta);
}
