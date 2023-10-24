
namespace Masa.Workflow.Activities.Complete;

public class Complete : ActivityInput
{
    public Guid[]? DependentIds { get; set; }
}
