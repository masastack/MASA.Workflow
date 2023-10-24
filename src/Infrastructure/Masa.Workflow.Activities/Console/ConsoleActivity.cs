using Masa.Workflow.Core.Models;

namespace Masa.Workflow.Activities.Console;

internal class ConsoleActivity : MasaWorkflowActivity<ConsoleInput>
{
    private readonly TextWriter _output;

    public ConsoleActivity(Msg msg) : base(msg)
    {
        _output = System.Console.Out;
    }

    public override Task<ActivityExecutionResult> RunAsync(ConsoleInput meta)
    {
        _output.WriteLine(meta.Text);
        System.Console.WriteLine("------------ConsoleActivity Run");
        return base.RunAsync(meta);
    }
}
