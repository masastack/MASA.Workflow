namespace Masa.Workflow.Core;

public sealed class MasaWorkFlow : Workflow<WorkflowInstance, RunWorkflowResult>, IEventHandler<CompleteEvent>
{
    List<KeyValuePair<string, string>> _completeList = new();

    public async override Task<RunWorkflowResult> RunAsync(WorkflowContext context, WorkflowInstance workflowInstance)
    {
        //Cron 
        //立即执行
        //触发执行
        return await CallActivity(workflowInstance.StartActivity.ActivityId);

        async Task<RunWorkflowResult> CallActivity(Guid activityId, Message? msg = null)
        {
            var activity = workflowInstance.Activities.FirstOrDefault(a => a.Id == activityId)
                ?? throw new Exception($"Not found activity ID={activityId}");

            Console.WriteLine($"------------MasaWorkFlow start {activity.Type} activity");
            var activityName = $"{ConvertTypeName(activity.Type)}Activity";
            Console.WriteLine($"CallActivityAsync {activityName}");

            var messageInput = new ActivityInfo
            {
                WorkflowId = workflowInstance.Id,
                Msg = msg,
                Input = activity.Meta,
                ActivityId = activityId,
                Wires = activity.Wires,
            };

            Console.WriteLine($"ActivityMessageInput is {JsonSerializer.Serialize(messageInput)}");

            var activityResult = await context.CallActivityAsync<ActivityExecutionResult>(activityName, messageInput, new WorkflowTaskOptions()
            {
                RetryPolicy = activity.RetryPolicy.Adapt<WorkflowRetryPolicy>()
            });

            if (activityResult.Wires.Count != activityResult.Messages.Count)
            {
                return new RunWorkflowResult(context.InstanceId, new Exception($"Activity {activityName}[id:{activityResult.ActivityId}] return data wires and msgs not matched"), WorkflowStatus.Faulted);
            }

            if (activityResult.Status == ActivityStatus.Suspended)
            {
                await context.CreateTimer(TimeSpan.FromSeconds(1));
                context.ContinueAsNew(activity.Meta);
            }
            if (activityResult.Status == ActivityStatus.Faulted)
            {
                return new RunWorkflowResult(context.InstanceId, new Exception($"Activity {activityName}[id:{activityResult.ActivityId}] call error"), WorkflowStatus.Faulted);
            }

            for (int i = 0; i < activityResult.Wires.Count; i++)
            {
                var m = activityResult.Messages[i];
                if (m != null)
                {
                    foreach (var wire in activityResult.Wires[i])
                    {
                        await CallActivity(wire, m);
                    }
                }
            }
            return new RunWorkflowResult(context.InstanceId, null, WorkflowStatus.Finished);
        }
    }

    string ConvertTypeName(string type)
    {
        StringBuilder resultBuilder = new StringBuilder();
        // 分割字符串
        string[] words = type.Split('-', '_');
        foreach (string word in words)
        {
            // 忽略空单词
            if (string.IsNullOrEmpty(word))
                continue;

            // 转换为 Pascal 命名法
            string pascalWord = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower());
            resultBuilder.Append(pascalWord);
        }
        return resultBuilder.ToString();
    }

    Task IEventHandler<CompleteEvent>.HandleAsync(CompleteEvent @event, CancellationToken cancellationToken)
    {
        //for (int i = 0; i < workBatch.Length; i++)
        //{
        //    Task<int> task = context.CallActivityAsync<int>("ProcessWorkItem", workBatch[i]);
        //    parallelTasks.Add(task);
        //}

        //// Everything is scheduled. Wait here until all parallel tasks have completed.
        //await Task.WhenAll(parallelTasks);
        throw new NotImplementedException();
    }
}
