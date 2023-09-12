namespace Masa.Workflow.Activities.Console;

internal class ConsoleActivity : MasaWorkflowActivity<ConsoleMeta>
{
    private readonly TextWriter _output;

    public ConsoleActivity(Msg msg) : base(msg)
    {
        _output = System.Console.Out;
    }

    public override Task<List<List<Guid>>> RunAsync(ConsoleMeta meta)
    {
        _output.WriteLine(meta.Text);
        System.Console.WriteLine("------------ConsoleActivity Run");
        System.Console.WriteLine(meta.Text);
        return base.RunAsync(meta);
    }
}
