using Masa.Workflow.Activities.Contracts.Function;
using Masa.Workflow.Core.Models;
using Masa.Workflow.Interactive.Runner;

namespace Masa.Workflow.Activities.Function;

public class FunctionActivity : MasaWorkflowActivity<FunctionMeta>
{
    public async override Task<ActivityExecutionResult> RunAsync(FunctionMeta meta, Message msg)
    {
        var runner = new CSharpRunner(typeof(Message).Assembly);
        var messages = await runner.RunWithMessageAsync<object>(meta.Code, msg);

        var result = new ActivityExecutionResult()
        {
            ActivityId = ActivityId.ToString(),
            Status = ActivityStatus.Finished,
            Wires = Wires
        };

        if (messages is Message[] list)
        {
            for (var i = 0; i < list.Length; i++)
            {
                result.Messages.Add(list[i]);
            }
        }
        else if (messages is Message m)
        {
            result.Messages.Add(m);
        }
        else
        {
            throw new Exception("FunctionActivity return type not supported");
        }

        return result;
    }
}
