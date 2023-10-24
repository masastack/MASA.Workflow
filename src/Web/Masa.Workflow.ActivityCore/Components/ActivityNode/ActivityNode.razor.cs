using Force.DeepCloner;

namespace Masa.Workflow.ActivityCore.Components;

public partial class ActivityNode<TMeta> : ComponentBase, IActivityNode
    where TMeta : class, new()
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] protected DrawflowService DrawflowService { get; set; } = null!;

    [Parameter] [EditorRequired] public string NodeId { get; set; } = null!;

    /// <summary>
    /// Value from drawflow df-*
    /// </summary>
    [Parameter] public string Value { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private bool _drawer;
    private StringNumber? _tab;
    private ActivityMeta<TMeta> _cachedModel = new();

    private Node? _node;
    private string _prevValue = null!;

    protected ActivityMeta<TMeta> FormModel { get; set; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        DeserializeValue();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        DeserializeValue();
    }

    private void DeserializeValue()
    {
        if (_prevValue != Value)
        {
            _prevValue = Value;

            _cachedModel = JsonSerializer.Deserialize<ActivityMeta<TMeta>>(Value)!;
            if (!string.IsNullOrWhiteSpace(_cachedModel.Meta))
            {
                _cachedModel.MetaData = JsonSerializer.Deserialize<TMeta>(_cachedModel.Meta);
            }
        }
    }

    private async Task OnDblClick()
    {
        await OnBeforeDrawerOpenAsync();

        FormModel = _cachedModel.DeepClone();

        _drawer = true;
    }

    protected virtual Task OnBeforeDrawerOpenAsync()
    {
        return Task.CompletedTask;
    }

    protected async Task OnSave(ModalActionEventArgs args)
    {
        _drawer = false;

        _cachedModel = FormModel.DeepClone();

        _cachedModel.Meta = JsonSerializer.Serialize(_cachedModel.MetaData);

        await _inputOutputs.ForEachAsync(async item =>
        {
            if (item.OutputsToRemove.Any())
            {
                await item.OutputsToRemove.OrderByDescending(u => u).ToList().ForEachAsync(async id =>
                {
                    _ = DrawflowService.RemoveNodeOutputAsync(NodeId, "output_" + id);
                });
            }

            for (int i = 0; i < item.NewOutputsCount; i++)
            {
                _ = DrawflowService.AddNodeOutputAsync(NodeId);
            }
        });

        await DrawflowService.UpdateNodeDataFromIdAsync(NodeId, new { data = JsonSerializer.Serialize(_cachedModel) });
    }

    private void OnCancel()
    {
        _drawer = false;
    }

    private async Task OnDelete()
    {
        _drawer = false;
        await DrawflowService.RemoveNodeAsync(NodeId);
    }

    protected virtual void DrawerContent(RenderTreeBuilder __builder)
    {
    }

    private readonly List<INodeOutputs> _inputOutputs = new();

    public void RegisterInputOutput(INodeOutputs comp)
    {
        _inputOutputs.Add(comp);
    }
}
