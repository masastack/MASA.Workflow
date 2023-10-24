namespace Masa.Workflow.Activities.Contracts.Function;

public class FunctionMeta
{
    public string Code { get; set; }

    public FunctionMeta()
    {
        Code = "return msg;";
    }
}
