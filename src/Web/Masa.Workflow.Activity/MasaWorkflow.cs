namespace Masa.Workflow.Activity;

public class MasaWorkFlow : Workflow<string, string>
{
    public sealed override Task<string> RunAsync(WorkflowContext context, string input)
    {
        //https://docs.masastack.com/framework/building-blocks/rules-engine/microsoft-rules-engine
        throw new NotImplementedException();
    }
}
