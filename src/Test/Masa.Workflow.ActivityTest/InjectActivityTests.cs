using Masa.Workflow.Activities.Contracts.Inject;
using Masa.Workflow.Activities.Inject;

namespace Masa.Workflow.ActivityTest;

public class InjectActivityTests
{
    [Fact]
    public async Task Test()
    {
        var injectActivity = new InjectActivity();

        var injectMeta = new InjectMeta()
        {
            MessageItems = new List<MessageItem>()
                {
                    new MessageItem()
                    {
                        Key = "Topic",
                        Type = MessageItemType.String,
                        Value = "banana"
                    },
                    new MessageItem()
                    {
                        Key = "Payload",
                        Type = MessageItemType.Timestamp
                    }
                }
        };

        var result = await injectActivity.RunAsync(injectMeta, null);
        dynamic message = result.Messages[0];
        Assert.Equal("banana", message["Topic"]);
        Assert.Equal("banana", message.Topic);
        Assert.Equal(10, message.Payload.ToString().Length); // 1698308212
    }
}
