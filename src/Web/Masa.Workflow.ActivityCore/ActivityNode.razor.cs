using Masa.Blazor.Presets;
using Microsoft.AspNetCore.Components.Rendering;

namespace Masa.Workflow.ActivityCore;

public partial class ActivityNode<TModel> : ComponentBase where TModel : class, new()
{
    [Parameter]
    [EditorRequired]
    public string? Color { get; set; }

    [Parameter]
    [EditorRequired]
    public string? Name { get; set; }

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public bool IconRight { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool _drawer;

    protected TModel FromModel { get; set; } = new();

    private async Task OnDblClick()
    {
        _drawer = true;
    }

    protected virtual Task OnSave(ModalActionEventArgs args)
    {
        _drawer = false;

        return Task.CompletedTask;
    }

    protected virtual void DrawerContent(RenderTreeBuilder __builder)
    {
    }
}
