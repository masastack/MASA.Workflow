namespace Masa.Workflow.RCL.Pages.Workflows;

public partial class Workflows
{
    [Inject] private WorkflowAgent.WorkflowAgentClient WorkflowAgentClient { get; set; } = null!;

    [Inject] private IPopupService PopupService { get; set; } = null!;

    private readonly WorkflowListRequest _query = new()
    {
        Page = 1,
        PageSize = 10
    };

    private PagedWorkflowList? _pagedWorkflowList;
    private bool _loading;

    private List<DataTableHeader<WorkflowItem>> _headers = new()
    {
        new() { Text = "Id", Value = nameof(WorkflowItem.Id), Sortable = false},
        new() { Text = "Status", Value = nameof(WorkflowItem.Status), Sortable = false },
        new() { Text = "Name", Value = nameof(WorkflowItem.Name), Sortable = false },
        new() { Text = "UpdatedAt", Value = nameof(WorkflowItem.UpdateDateTimeStamp) },
        new() { Text = "Actions", Value = "actions", Sortable = false, Width = 120 }
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await GetListAsync();

            StateHasChanged();
        }
    }

    private async Task GetListAsync()
    {
        _loading = true;
        StateHasChanged();

        try
        {
            _pagedWorkflowList = await WorkflowAgentClient.GetListAsync(_query);
        }
        catch (Exception e)
        {
            await PopupService.EnqueueSnackbarAsync(e);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task HandleOnSearch()
    {
        _query.Page = 1;

        await GetListAsync();
    }

    private async Task HandleOnDataTableOptionsUpdate(DataOptions options)
    {
        _query.Page = options.Page;
        _query.PageSize = options.ItemsPerPage;

        await GetListAsync();
    }
}
