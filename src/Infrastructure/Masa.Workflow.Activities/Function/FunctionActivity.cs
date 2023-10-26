using Masa.Workflow.Core.Models;
using Masa.Workflow.Interactive.Runner;

namespace Masa.Workflow.Activities.Function;

public class FunctionActivity : MasaWorkflowActivity<FunctionInput>
{
    public override async Task<ActivityExecutionResult> RunAsync(FunctionInput input)
    {
        var runner = new CSharpRunner(typeof(Message).Assembly);
        var messages = await runner.RunWithMessageAsync<object>(input.Meta.Code, input.Message);

        var result = new ActivityExecutionResult()
        {
            ActivityId = input.ActivityId.ToString(),
            Status = ActivityStatus.Finished,
        };

        if (messages is Message[] list)
        {
            for (var i = 0; i < list.Length; i++)
            {
                result.Messages.Add(list[i]);
                result.Wires.Add(input.Wires[i]);
            }
        }
        else if (messages is Message msg)
        {
            result.Messages.Add(msg);
            result.Wires.Add(input.Wires[0]);
        }
        else
        {
            throw new Exception("FunctionActivity return type not supported");
        }

        return result;
    }
}
