using System.Text.Json;
using Force.DeepCloner;
using Microsoft.JSInterop;

namespace Masa.Workflow.ActivityCore;

public partial class ActivityNode<TMeta, T> : ComponentBase
    where TMeta : class, new()
    where T : ActivityMeta<TMeta>, new()
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    [Parameter]
    public string Value { get; set; } = null!;

    [Parameter]
    public string TagName { get; set; } = null!;

    [Parameter]
    public string Color { get; set; } = null!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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

    protected void OnSave(ModalActionEventArgs args)
    {
        _drawer = false;

        _cachedModel = FormModel.DeepClone();

        _cachedModel.Meta = JsonSerializer.Serialize(_cachedModel.MetaData);

        _ = JSRuntime.InvokeVoidAsync("UpdateCustomElementValue", _node?.ElementReference, JsonSerializer.Serialize(_cachedModel));
    }

    protected virtual void DrawerContent(RenderTreeBuilder __builder)
    {
    }
}
