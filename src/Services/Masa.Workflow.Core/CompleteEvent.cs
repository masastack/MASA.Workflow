namespace Masa.Workflow.Core;

internal record CompleteEvent(Guid ActivityId) : Event
{
}
