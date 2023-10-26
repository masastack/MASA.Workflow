using Masa.Workflow.Interactive.Runner;

namespace Masa.Workflow.Core;

public static class CSharpRunnerExtensions
{
    public static Task<T> RunWithMessageAsync<T>(this CSharpRunner runner, string code, Message message, List<string>? packages = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        return runner.RunAsync<T>(code, new GlobalMessage(message), packages, cancellationToken);
    }
}
