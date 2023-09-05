namespace Masa.Workflow.Interractive.Runner;

internal interface IRunner
{
    public TextWriter TextWriter { get; init; }

    public Task<T> RunAsync<T>(string code, object? globals, List<string> packages, CancellationToken cancellationToken);
}
