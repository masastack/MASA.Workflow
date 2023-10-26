namespace Masa.Workflow.Activities.Contracts.Inject;

public class InjectMeta
{
    public List<MessageItem> MessageItems { get; init; } = new()
    {
        new MessageItem("Payload")
    };
}

public class MessageItem
{
    public MessageItem()
    {
    }
    
    public MessageItem(string key)
    {
        Key = key;
    }

    public string? Key { get; set; }

    public string? Value { get; set; }

    public MessageItemType Type { get; set; }
}

public enum MessageItemType
{
    String,
    Number,
    Boolean,
    Timestamp,
}
