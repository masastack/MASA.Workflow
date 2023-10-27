namespace Masa.Workflow.Activities.Inject;

public class InjectActivity : MasaWorkflowActivity<InjectMeta>
{
    public override Task<ActivityExecutionResult> RunAsync(InjectMeta meta, Message msg)
    {
        var result = new ActivityExecutionResult()
        {
            ActivityId = ActivityId.ToString(),
            Status = ActivityStatus.Finished,
            Wires = Wires
        };

        Message message = new();
        foreach (var item in meta.MessageItems)
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
