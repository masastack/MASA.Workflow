namespace Masa.Workflow.ActivityCore.Components;

public interface INodeOutputs
{
    List<int> OutputsToRemove { get; }

    int NewOutputsCount { get; }
}
