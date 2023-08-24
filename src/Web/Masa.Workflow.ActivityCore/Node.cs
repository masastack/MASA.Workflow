using BemIt;
using BlazorComponent;
using Masa.Blazor;
using Microsoft.AspNetCore.Components.Rendering;

namespace Masa.Workflow.ActivityCore;

public class Node : ComponentBase
{
    private readonly bool _draggable;

    public Node()
    {
    }

    public Node(bool draggable)
    {
        _draggable = draggable;
    }

    [Parameter]
    [EditorRequired]
    public string? Color { get; set; }

    [Parameter]
    [EditorRequired]
    public string? Name { get; set; }

    [Parameter]
    // [EditorRequired]
    public string? NodeId { get; set; }

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public bool IconRight { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?> AdditionalAttributes { get; set; }

    private Block Block => new("mw-activity-node");

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        ArgumentException.ThrowIfNullOrEmpty(nameof(Name));
        // ArgumentException.ThrowIfNullOrEmpty(nameof(NodeId));
        ArgumentException.ThrowIfNullOrEmpty(nameof(Color));
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (_draggable)
        {
            builder.OpenComponent<MDrag>(0);
        }
        else
        {
            builder.OpenElement(0, "div");
        }
        
        builder.AddMultipleAttributes(1, AdditionalAttributes);

        builder.AddAttribute(2, "class", Block.Modifier("draggable", _draggable).Build());
        builder.AddAttribute(3, "style", $"background-color:{Color}");

        RenderFragment childContent = builder2 =>
        {
            builder2.OpenElement(0, "div");
            builder2.AddAttribute(1, "class", @Block.Element("icon").Modifier("right", IconRight).Build());
            builder2.AddContent(2, iconInnerBuilder =>
            {
                iconInnerBuilder.OpenComponent<MIcon>(0);
                iconInnerBuilder.AddAttribute(1, nameof(MIcon.Dark), true);
                iconInnerBuilder.AddAttribute(2, nameof(MIcon.Icon), (Icon)Icon);
                iconInnerBuilder.CloseComponent();
            });
            builder2.CloseElement();

            builder2.OpenElement(3, "div");
            builder2.AddAttribute(4, "class", Block.Element("name").Build());
            builder2.AddContent(5, Name);
            builder2.CloseElement();
        };

        if (_draggable)
        {
            builder.AddAttribute(4, nameof(MDrag.DataValue), NodeId);
            builder.AddAttribute(5, "ChildContent", childContent);
        }
        else
        {
            builder.AddContent(4, childContent);
        }

        if (_draggable)
        {
            builder.CloseComponent();
        }
        else
        {
            builder.CloseElement();
        }
    }
}
