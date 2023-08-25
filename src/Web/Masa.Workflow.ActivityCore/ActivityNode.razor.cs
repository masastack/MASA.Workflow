using System.Text.Json;
using Force.DeepCloner;
using Masa.Workflow.RCL;
using Microsoft.JSInterop;

namespace Masa.Workflow.ActivityCore;

public partial class ActivityNode<TMeta, T> : ComponentBase
    where TMeta : class, new()
    where T : ActivityMeta<TMeta>, new()
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] private DrawflowService DrawflowService { get; set; } = null!;

    [Parameter] public string Color { get; set; } = null!;

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
        FormModel = _cachedModel.DeepClone();

        _drawer = true;
    }

    protected async Task OnSave(ModalActionEventArgs args)
    {
        _drawer = false;

        _cachedModel = FormModel.DeepClone();

        _cachedModel.Meta = JsonSerializer.Serialize(_cachedModel.MetaData);

        await DrawflowService.UpdateNodeDataFromIdAsync(NodeId, _cachedModel);
    }

    private async Task OnDelete()
    {
        _drawer = false;
        await DrawflowService.RemoveNodeAsync(NodeId);
    }

    protected virtual void DrawerContent(RenderTreeBuilder __builder)
    {
    }

    protected async Task AddInputAsync()
    {
        FormModel.Input++;

        await DrawflowService.AddInputAsync(NodeId);
    }

    protected async Task AddOutputAsync()
    {
        FormModel.Output++;

        await DrawflowService.AddOutputAsync(NodeId);
    }

    protected async Task RemoveInputAsync()
    {
        FormModel.Input--;

        // await DrawflowService.RemoveInputAsync(NodeId);
    }
    
    protected async Task RemoveOutputAsync()
    {
        FormModel.Output--;

        // await DrawflowService.RemoveOutputAsync(NodeId);
    }
}
