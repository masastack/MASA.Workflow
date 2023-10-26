using Masa.Workflow.Activities.Contracts.Inject;
using Masa.Workflow.Core.Models;

namespace Masa.Workflow.Activities.Inject;

public class InjectActivity : MasaWorkflowActivity<InjectInput>
{
    public override Task<ActivityExecutionResult> RunAsync(InjectInput input)
    {
        var result = new ActivityExecutionResult()
        {
            ActivityId = input.ActivityId.ToString(),
            Status = ActivityStatus.Finished,
            Wires = input.Wires
        };

        Message message = new();
        foreach (var item in input.Meta.MessageItems)
        {
            if (string.IsNullOrWhiteSpace(item.Key))
            {
                continue;
            }

            object? value = item.Value;
            if (item.Type == MessageItemType.Boolean)
            {
                value = item.Value == "True";
            }
            else if (item.Type == MessageItemType.Number)
            {
                value = Convert.ToDouble(item.Value);
            }
            else if (item.Type == MessageItemType.Timestamp)
            {
                value = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }

            if (item.Key == "Payload")
            {
                message.Payload = value;
            }
            else
            {
                message[item.Key] = value;
            }
        }

        result.Messages.Add(message);

        return Task.FromResult(result);
    }
}
