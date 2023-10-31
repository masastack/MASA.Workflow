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

        var functionMeta = new FunctionMeta()
        {
            Code = """
                       if (msg.Topic == "banana") {
                           return new Message[] { null, msg };
                       }
                       else {
                            return new Message[] { msg, null };                       
                       }
                       """
        };

        var result = await functionActivity.RunAsync(functionMeta, message);
        Assert.Equal(2, result.Messages.Count); ;
        Assert.Equal(null, result.Messages[0]);
        Assert.Equal(message.topic, result.Messages[1]["Topic"]);
    }
    
    [Fact]
    public async Task Test2()
    {
        var functionActivity = new FunctionActivity();

        dynamic message = new Message();
        message.Topic = "banana";

        var functionMeta = new FunctionMeta()
        {
            Code = """
                   return msg;
                   """
        };

        var result = await functionActivity.RunAsync(functionMeta, message);
        Assert.Equal(1, result.Messages.Count);
        Assert.Equal("banana", result.Messages[0].Topic);
    }
}
