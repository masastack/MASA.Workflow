namespace Masa.Workflow.RCL.Pages.Workspace;

public class DebugLog
{
    public DebugLog(string log, DateTimeOffset timestamp)
    {
        Log = log;
        Timestamp = timestamp;
    }

    public DateTimeOffset Timestamp { get; }

    public string Log { get; } 
}
