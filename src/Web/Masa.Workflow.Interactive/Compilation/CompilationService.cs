namespace Masa.Workflow.Interactive.Compilation;

internal class CompilationService
{
    public string Compile(string code, bool hasPdb)
    {
        var scriptOptions = ScriptOptions.Default.AddImports(Namespaces.ImportedNamespaces)
                .WithEmitDebugInformation(true)
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithLanguageVersion(LanguageVersion.Latest);

        var script = CSharpScript.Create(code, options: scriptOptions);

        var compilation = script.GetCompilation();

        foreach (var diagnostic in compilation.GetDiagnostics())
        {
            Console.WriteLine(diagnostic.ToString());
        }

        var errors = compilation.GetDiagnostics()
            .Where(x => x.Severity == DiagnosticSeverity.Error)
            .ToArray();

        if (errors.Any())
            throw new ScriptCompilationException(errors);

        using var asm = new MemoryStream();
        using var pdb = new MemoryStream();

        var emitOptions = new EmitOptions(false, DebugInformationFormat.PortablePdb);
        var emitResult = compilation.Emit(asm, pdb, options: emitOptions);

        if (emitResult.Success)
        {
            asm.Seek(0, SeekOrigin.Begin);
            byte[] buffer = asm.GetBuffer();
            if (hasPdb)
            {
                pdb.Seek(0, SeekOrigin.Begin);
                byte[] pdbBuffer = pdb.GetBuffer();
            }
            return Convert.ToBase64String(buffer);
        }
        return string.Empty;
    }
}
