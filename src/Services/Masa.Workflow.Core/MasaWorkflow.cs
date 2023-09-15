namespace Masa.Workflow.Core;

public sealed class MasaWorkFlow : Workflow<WorkflowInstance, RunWorkflowResult>, IEventHandler<CompleteEvent>
{
    List<KeyValuePair<string, string>> _completeList = new();

    public async override Task<RunWorkflowResult> RunAsync(WorkflowContext context, WorkflowInstance workflowInstance)
    {
        Console.WriteLine("------------MasaWorkFlow Run");
        workflowInstance = new WorkflowInstance
        {
            Id = Guid.Parse(context.InstanceId),
            Name = "Test",
            Variables = new Variables { { "a", 1 } },
            Activities = new List<ActivityDefinition> {
                new ActivityDefinition { Id = Guid.NewGuid(), Name = "Test1", Type = "Console" } ,
                new ActivityDefinition { Id = Guid.NewGuid(), Name = "Test2", Type = "Switch" } ,
                new ActivityDefinition { Id = Guid.NewGuid(), Name = "Test3", Type = "Complete" } ,
            }
        };

        foreach (var activity in workflowInstance.Activities)
        {
            if (ActivityProvider.TryGet(activity.Type, out string? activityName))
            {
                Console.WriteLine($"------------MasaWorkFlow Start Activity  {activity.Type}  [{activityName}]");

                await context.CallActivityAsync<Wires>(activityName!, new
                {
                    Text = $"{activity.Name}--{activity.Type}",
                    ActivityId = activity.Id,

                }, new WorkflowTaskOptions());
            }
            else
            {
                Console.WriteLine($"------------ActivityProvider not contain a key {activity.Type}");
            }
        }
        return new RunWorkflowResult(context.InstanceId, null, true);
    }

    Task IEventHandler<CompleteEvent>.HandleAsync(CompleteEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
