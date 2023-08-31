using Masa.BuildingBlocks.Dispatcher.Events;

namespace Masa.Workflow.ActivityCore;

public class MasaWorkFlow : Workflow<string, string>, IEventHandler<CompleteEvent>
{
    List<KeyValuePair<string, string>> _completeList = new();

    public sealed override Task<string> RunAsync(WorkflowContext context, string input)
    {
        throw new NotImplementedException();
    }

    Task IEventHandler<CompleteEvent>.HandleAsync(CompleteEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
