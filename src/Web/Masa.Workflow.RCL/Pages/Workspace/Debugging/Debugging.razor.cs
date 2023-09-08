namespace Masa.Workflow.RCL.Pages.Workspace;

public partial class Debugging
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] private IPopupService PopupService { get; set; } = null!;

    private List<DebugLog> _logs = new();

    private bool _detailDialog;
    private DebugLog? _log;

    private static object? s_monacoOptions = new
    {
        language = "json",
        readOnly = true,
    };

    internal void AddLog(string log)
    {
        AddLog(log, DateTimeOffset.Now);
    }

    internal void AddLog(string log, DateTimeOffset timestamp)
    {
        _logs.Add(new DebugLog(log, timestamp));
        StateHasChanged();
    }

    internal void AddLogs(IEnumerable<DebugLog> logs)
    {
        _logs.AddRange(logs);
        StateHasChanged();
    }

    private void OpenDetailDialog(DebugLog origin)
    {
        string log;

        try
        {
            using var doc = JsonDocument.Parse(origin.Log);
            log = doc.ToJsonString();
        }
        catch (JsonException e)
        {
            log = origin.Log;
        }

        _log = new DebugLog(log, origin.Timestamp);

        _detailDialog = true;
    }

    private void DialogChanged()
    {
        if (!_detailDialog)
        {
            _log = null;
        }
    }

    private async Task CopyDebugLog()
    {
        await JSRuntime.InvokeVoidAsync(JsInteropConstants.Copy, _log?.Log);

        await PopupService.EnqueueSnackbarAsync("i18n.workflow.copy-success", AlertTypes.Success);
    }
}
