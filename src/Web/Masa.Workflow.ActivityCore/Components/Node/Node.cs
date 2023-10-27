using System.Collections.ObjectModel;

namespace Masa.Workflow.ActivityCore.Components;

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

    [Parameter] [EditorRequired] public string? Color { get; set; }

    [Parameter] public bool Inactive { get; set; }

    [Parameter] public string? Label { get; set; }

    [Parameter] public bool HideLabel { get; set; }

    [Parameter] public string? Name { get; set; }

    [Parameter] public string? Icon { get; set; }

    [Parameter] public bool IconRight { get; set; }

    [Parameter] public IReadOnlyList<NodeStateInfo>? States { get; set; }

    [Parameter] public string? State { get; set; }

    [Parameter] public string? CustomStateLabel { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?> AdditionalAttributes { get; set; }

    private Block Block => new("mw-activity-node");

    public ElementReference ElementReference { get; private set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        ArgumentException.ThrowIfNullOrEmpty(nameof(Label));
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

        builder.AddAttribute(2, "class", Block.Modifier("draggable", _draggable).Add(Inactive).Add("right", IconRight).Build());
        builder.AddAttribute(3, "style", $"background-color:{Color}");

        RenderFragment childContent = builder2 =>
        {
            builder2.OpenElement(0, "div");
            builder2.AddAttribute(1, "class", @Block.Element("icon").Build());
            builder2.AddContent(2, iconInnerBuilder =>
            {
                iconInnerBuilder.OpenComponent<MIcon>(0);
                iconInnerBuilder.AddAttribute(1, nameof(MIcon.Dark), true);
                iconInnerBuilder.AddAttribute(2, nameof(MIcon.Icon), (Icon)Icon);
                iconInnerBuilder.CloseComponent();
            });
            builder2.CloseElement();

            if (!HideLabel)
            {
                builder2.OpenElement(3, "div");
                builder2.AddAttribute(4, "class", Block.Element("name").Build());
                builder2.AddContent(5, Label);
                builder2.CloseElement();
            }
        };

        if (_draggable)
        {
            builder.AddAttribute(4, nameof(MDrag.DataValue), Name);
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
            builder.AddElementReferenceCapture(6, e => ElementReference = e);
            builder.CloseElement();

            if (States is not null)
            {
                builder.OpenComponent<NodeState>(7);
                builder.AddAttribute(8, nameof(NodeState.States), States);
                builder.AddAttribute(9, nameof(NodeState.State), State);
                builder.CloseComponent();
            }
        }
    }
}
