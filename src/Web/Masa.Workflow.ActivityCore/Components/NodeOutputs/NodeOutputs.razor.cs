namespace Masa.Workflow.ActivityCore.Components;

public partial class NodeOutputs<TItem>
{
    [CascadingParameter] private IActivityNode? ActivityNode { get; set; }

    [CascadingParameter(Name = "ActivityNodeDrawer")]
    private bool Drawer { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    [Parameter] public string? Title { get; set; }

    [Parameter] public string? TitleClass { get; set; }

    [Parameter] [EditorRequired] public List<TItem> Items { get; set; } = new();

    [Parameter] public RenderFragment<(Action<TItem> RemoveItem, Action AddNew)>? ChildContent { get; set; }

    private bool _prevDrawer;
    private List<TItem> _initialItems = new();

    public int NewOutputsCount { get; private set; }

    public List<int> OutputsToRemove { get; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ActivityNode?.RegisterInputOutput(this);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (Drawer != _prevDrawer)
        {
            _prevDrawer = Drawer;

            if (Drawer)
            {
                _initialItems = Items.ToList();
            }
            else
            {
                _initialItems.Clear();
                OutputsToRemove.Clear();
                NewOutputsCount = 0;
            }
        }
    }

    private void RemoveItem(TItem item)
    {
        var index = _initialItems.IndexOf(item);
        if (index > -1)
        {
            OutputsToRemove.Add(index + 1);
        }
        else
        {
            NewOutputsCount--;
        }
    }

    private void AddNew()
    {
        NewOutputsCount++;
    }
}
