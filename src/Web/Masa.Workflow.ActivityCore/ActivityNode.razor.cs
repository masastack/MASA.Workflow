using System.Text.Json;
using Force.DeepCloner;
using Masa.Workflow.ActivityCore.Components;
using Microsoft.JSInterop;

namespace Masa.Workflow.ActivityCore;

public partial class ActivityNode<TMeta, T> : ComponentBase, IActivityNode
    where TMeta : class, new()
    where T : ActivityMeta<TMeta>, new()
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] private DrawflowService DrawflowService { get; set; } = null!;

    [Parameter] public int NodeId { get; set; }

    /// <summary>
    /// Value from drawflow df-*
    /// </summary>
    [Parameter] public string Value { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private bool _drawer;
    private StringNumber? _tab;
    private T _cachedModel = new();

    private Node? _node;

    protected T FormModel { get; set; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _cachedModel = JsonSerializer.Deserialize<T>(Value)!;
    }

    private void OnDblClick()
    {
        BeforeDrawerOpen();

        FormModel = _cachedModel.DeepClone();

        _drawer = true;
    }

    protected virtual void BeforeDrawerOpen()
    {
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

        await DrawflowService.UpdateNodeDataFromIdAsync(NodeId, new { data =  JsonSerializer.Serialize(_cachedModel) });
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
