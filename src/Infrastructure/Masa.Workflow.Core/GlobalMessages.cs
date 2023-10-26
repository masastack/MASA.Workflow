namespace Masa.Workflow.Core;

public class GlobalMessage
{
    public GlobalMessage(Message message)
    {
        msg = message;
    }

    public dynamic msg { get; init; }
}
