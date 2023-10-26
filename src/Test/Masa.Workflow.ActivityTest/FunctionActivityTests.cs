using Masa.Workflow.Activities;
using Masa.Workflow.Activities.Contracts.Function;
using Masa.Workflow.Activities.Function;
using Masa.Workflow.Core;

namespace Masa.Workflow.ActivityTest;

public class FunctionActivityTests
{
    [Fact]
    public async Task Test()
    {
        var functionActivity = new FunctionActivity();

        dynamic message = new Message();
        message.Topic = "banana";
        
        var functionInput = new FunctionInput()
        {
            Message = message,
            Meta = new FunctionMeta()
            {
                Code = """
                       if (msg.Topic == "banana") {
                           return new Message[] { null, msg };
                       }
                       else {
                            return new Message[] { msg, null };                       
                       }
                       """
            },
            Wires = new List<List<Guid>>()
            {
                new List<Guid>()
                {
                    Guid.NewGuid(),
                },
                new List<Guid>()
                {
                    Guid.NewGuid(),
                }
            }
        };

        var result = await functionActivity.RunAsync(functionInput);
        Assert.Equal(2, result.Messages.Count);;
        Assert.Equal(null, result.Messages[0]);
        Assert.Equal(message.topic, result.Messages[1]["Topic"]);
    }
}
