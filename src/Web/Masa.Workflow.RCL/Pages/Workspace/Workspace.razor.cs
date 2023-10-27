using BlazorComponent.I18n;
using Masa.Workflow.ActivityCore.Components;
using Masa.Workflow.RCL.Pages.Workspace.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Masa.Workflow.RCL.Pages.Workspace;

public partial class Workspace : IAsyncDisposable
{
    [Inject] private I18n I18n { get; set; } = null!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] private IPopupService PopupService { get; set; } = null!;

    [Inject] private DrawflowService DrawflowService { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private IOptions<WorkflowActivitiesRegistered> RegisteredActivities { get; set; } = null!;

    [Inject] private WorkflowAgent.WorkflowAgentClient WorkflowAgentClient { get; set; } = null!;

    [Inject] private WorkflowRunner.WorkflowRunnerClient WorkflowRunnerClient { get; set; } = null!;

    [Parameter] public Guid WorkflowId { get; set; }

    private MDrawflow _drawflow = null!;

    private WorkflowFormModal? _workflowFormModal;

    private WorkflowDetail? _workflowDetail;
    private Guid _testMqttInGuid = Guid.NewGuid(); // TODO: just for test mqtt in demo, remove it later

    private List<StringNumber>? _selectedGroups;
    private StringNumber? _node;

    private Debugging? _debugging;
    private ExportDialog _exportDialog = null!;
    private ImportDialog _importDialog = null!;

    private bool _helpDrawer;
    private string? _helpMarkdown;
    private List<string> _treeActives = new();
    private Dictionary<Guid, string> _activityNodeIdMap = new();

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

    internal string? WorkflowName { get; set; }
    internal string? WorkflowDescription { get; set; }

    private HubConnection _hubConnection;

    private async Task TestHub()
    {
        await _hubConnection.SendAsync("UpdateMqttInState", _testMqttInGuid, "connected");
    }

    protected override async Task OnInitializedAsync()
    {
        _hubConnection = new HubConnectionBuilder().WithUrl(NavigationManager.ToAbsoluteUri("/workflow-client-hub")).Build();
        _hubConnection.On<Guid, string>("WatchMqttInState", async (id, state) =>
        {
            if (_activityNodeIdMap.TryGetValue(id, out string nodeId))
            {
                var drawflowData = await _drawflow.ExportAsync();
                var activityMeta = DrawflowJsonHelper.GetNodeData<ActivityMeta>(drawflowData, nodeId);
                activityMeta.State = state;
                await _drawflow.UpdateNodeDataAsync(nodeId, new { data = JsonSerializer.Serialize(activityMeta) });
            }
            else
            {
                // LOG
            }
        });

        await _hubConnection.StartAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            // TODO: 多个窗口会问题吗？共享的是同一个 drawflow 吗？
            DrawflowService.SetDrawflow(_drawflow);

            _nodes = RegisteredActivities.Value.Select(u => u.Config).ToList();

            _nodeGroups = _nodes.GroupBy(u => u.Tag).ToList();
            _selectedGroups = _nodeGroups.Select(g => (StringNumber)g.Key).ToList();

            if (WorkflowId != default)
            {
                _workflowDetail = await WorkflowAgentClient.GetDetailAsync(new WorkflowId
                {
                    Id = WorkflowId.ToString()
                });

                WorkflowName = _workflowDetail.Name;
                WorkflowDescription = _workflowDetail.Description;

                await ImportWorkflow(_workflowDetail.NodeJson);
            }
            else
            {
                WorkflowName = "New workflow";
                WorkflowDescription = string.Empty;
            }

            StateHasChanged();
        }
    }

    private async Task Drop(ExDragEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.DataTransfer.Data.Value))
        {
            return;
        }

        var nodeType = args.DataTransfer.Data.Value;

        var tagName = $"{nodeType}-node";

        var node = _nodes.FirstOrDefault(u => u.Type == nodeType);

        var activityId = Guid.NewGuid();

        // TODO: just for test mqtt in demo, remove it later
        if (node.Type == "mqtt-in")
        {
            activityId = _testMqttInGuid;
        }

        var metaNode = new Node2(activityId, node.Type, node.Color, node.Icon, node.IconRight, false, node.States, node.MinInput, node.MinOutput, "");

        var id = await _drawflow.AddNodeAsync(
            nodeType,
            node.MinInput,
            node.MinOutput,
            clientX: args.ClientX,
            clientY: args.ClientY,
            offsetX: args.DataTransfer.Data.OffsetX,
            offsetY: args.DataTransfer.Data.OffsetY,
            className: "",
            new { data = JsonSerializer.Serialize(metaNode) },
            $"<{tagName} df-data></{tagName}>");

        await _drawflow.UpdateNodeHTMLAsync(id, $"<{tagName} node-id='{id}' df-data></{tagName}>");
        await JSRuntime.InvokeVoidAsync(JSInteropConstants.SetNodeIdToCustomElement, id);

        var treeNode = new TreeNode(id, node.Type, node.Icon, node.Color);
        treeNode.Extra["nodeType"] = node.Type;
        _tree[0].Children.Add(treeNode);

        _activityNodeIdMap[activityId] = id;
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
        Console.Out.WriteLine("NodeSelected nodeId = {0}", nodeId);
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

    private async Task TabChanged()
    {
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

    private async Task Save(bool publish)
    {
        await WorkflowAgentClient.SaveAsync(new WorkflowSaveRequest
        {
            Id = WorkflowId == Guid.Empty ? "" : WorkflowId.ToString(),
            Name = WorkflowName,
            Description = WorkflowDescription,
            Disabled = false,
            IsDraft = !publish,
            NodeJson = await _drawflow.ExportAsync(indented: true)
        });
        
        await PopupService.EnqueueSnackbarAsync("Saved", AlertTypes.Success);
    }

    private async Task Run()
    {
        await WorkflowRunnerClient.RunAsync(new WorkflowId
        {
            Id = "51A4F68A-1CDE-4324-6F34-08DBCBB9851E"
        });
    }

    private void OpenImportWorkflowDialog()
    {
        _importDialog.OpenDialog();
    }

    private async Task OpenExportWorkflowDialog()
    {
        var data = await _drawflow.ExportAsync(indented: true);
        _exportDialog.OpenDialog(data);
    }

    internal async Task ImportWorkflow(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return;
        }

        await _drawflow.ImportAsync(json);

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

            var treeNode = new TreeNode(id, nodeData.Name, nodeData.Icon, nodeData.Color);
            treeNode.Extra["nodeType"] = name;
            _tree[0].Children!.Add(treeNode);
            _activityNodeIdMap[nodeData.Id] = id;
        }

        StateHasChanged();
    }

    private async Task ToggleWorkflow()
    {
        // TODO: toggle workflow to http
    }

    private async Task DeleteWorkflow()
    {
        var confirm = await PopupService.ConfirmAsync("Delete Workflow(TODO: i18n)", "Are you sure to delete this workflow?(TODO: i18n)",
            AlertTypes.Error);
        if (confirm)
        {
            // delete workflow from http
        }
    }

    private void OpenWorkflowFormModal()
    {
        _workflowFormModal?.Open(WorkflowName, WorkflowDescription);
    }

    private void HandleOnWorkflowInfoSave((string name, string description) model)
    {
        WorkflowName = model.name;
        WorkflowDescription = model.description;
    }

    public record Node2(
        Guid Id,
        string Name,
        string Color,
        string? Icon,
        bool IconRight,
        bool HideLabel,
        IReadOnlyList<NodeStateInfo>? States,
        int MinInput,
        int MinOutput,
        string Meta);

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
