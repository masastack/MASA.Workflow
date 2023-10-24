
namespace Masa.Workflow.Activities.Complete;

public class CompleteInput : ActivityInput
{
    public Guid[]? DependentIds { get; set; }
}
