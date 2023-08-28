using Masa.Workflow.ActivityCore.Components;

namespace Masa.Workflow.ActivityCore;

public interface IActivityNode
{
    void RegisterInputOutput(INodeOutputs component);
}
