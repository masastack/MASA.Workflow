using Masa.BuildingBlocks.Dispatcher.Events;

namespace Masa.Workflow.ActivityCore;

internal record CompleteEvent(Guid ActivityId) : Event
{
}
