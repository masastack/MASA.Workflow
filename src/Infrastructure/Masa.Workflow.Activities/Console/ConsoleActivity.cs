using Masa.Workflow.Core.Models;

namespace Masa.Workflow.Activities.Console;

internal class ConsoleActivity : MasaWorkflowActivity<ConsoleInput>
{
    private readonly TextWriter _output;

    public ConsoleActivity()
    {
        _output = System.Console.Out;
    }

    public override Task<ActivityExecutionResult> RunAsync(ConsoleInput input, Message msg)
    {
        _output.WriteLine(input.Text);
        System.Console.WriteLine("------------ConsoleActivity Run");
        return base.RunAsync(input, msg);
    }
}
