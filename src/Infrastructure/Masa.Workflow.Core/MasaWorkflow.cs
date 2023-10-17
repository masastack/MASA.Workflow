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

        async Task<RunWorkflowResult> CallActivity(Guid activityId)
        {
            var activity = workflowInstance.Activities.FirstOrDefault(a => a.Id == activityId)
                ?? throw new Exception($"Not found activity ID={activityId}");

            Console.WriteLine($"------------MasaWorkFlow start {activity.Type} activity");
            var activityName = $"{activity.Type}Activity";
            var activityResult = await context.CallActivityAsync<ActivityExecutionResult>(activityName, activity.Meta, new WorkflowTaskOptions()
            {
                RetryPolicy = activity.RetryPolicy.Adapt<WorkflowRetryPolicy>()
            });

            if (activityResult.Status == ActivityStatus.Suspended)
            {
                await context.CreateTimer(TimeSpan.FromSeconds(1));
                context.ContinueAsNew(activity.Meta);
            }
            if (activityResult.Status == ActivityStatus.Faulted)
            {
                return new RunWorkflowResult(context.InstanceId, new Exception($"Activity {activityName}[id:{activityResult.ActivityId}] call error"), WorkflowStatus.Faulted);
            }

            if (activityResult.Wires.Any())
            {
                foreach (var wires in activityResult.Wires)
                {
                    foreach (var wire in wires)
                    {
                        await CallActivity(wire);
                    }
                }
            }

            return new RunWorkflowResult(context.InstanceId, null, WorkflowStatus.Finished);
        }
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
