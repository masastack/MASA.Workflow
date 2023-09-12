using Masa.Workflow.Core.Models;

namespace Masa.Workflow.Core;

public sealed class MasaWorkFlow : Workflow<Guid, RunWorkflowResult>, IEventHandler<CompleteEvent>
{
    List<KeyValuePair<string, string>> _completeList = new();

    public async override Task<RunWorkflowResult> RunAsync(WorkflowContext context, Guid instanceId)
    {
        Console.WriteLine("------------MasaWorkFlow Run");
        var workflowInstance = new WorkflowInstance
        {
            Id = instanceId,
            Name = "Test",
            Variables = new Variables { { "a", 1 } },
            Activities = new List<ActivityDefinition> {
                new ActivityDefinition { Id = Guid.NewGuid(), Name = "Test1", Type = "Console" } ,
                new ActivityDefinition { Id = Guid.NewGuid(), Name = "Test2", Type = "Switch" } ,
                new ActivityDefinition { Id = Guid.NewGuid(), Name = "Test3", Type = "Complete" } ,
            }
        };

        var activityDictionary = new Dictionary<string, string>()
        {
            { "Console","ConsoleActivity"},
            { "Switch","ConsoleActivity"},
            { "Complete","ConsoleActivity"}
        };
        foreach (var activity in workflowInstance.Activities)
        {
            Console.WriteLine($"------------MasaWorkFlow Start Activity  {activity.Type}  [{activityDictionary[activity.Type]}]");
            await context.CallActivityAsync<Wires>(activityDictionary[activity.Type], new
            {
                Text = $"{activity.Name}--{activity.Type}",
                ActivityId = activity.Id,

            }, new WorkflowTaskOptions());
        }
        return new RunWorkflowResult(context.InstanceId, null, true);
    }

    Task IEventHandler<CompleteEvent>.HandleAsync(CompleteEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
