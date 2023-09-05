namespace Masa.Workflow.Interactive.Compilation;

internal class ScriptCompilationException : Exception
{
    public ScriptCompilationException(IEnumerable<Diagnostic> diagnostics)
        : base($"Script compilation failure! See diagnostics below.{Environment.NewLine}" +
               $"{string.Join(Environment.NewLine, diagnostics)}")
    { }
}
