namespace Masa.Workflow.Interactive.Runner;

internal class CSharpRunner : IRunner
{
    private readonly IEnumerable<IMetadataReferenceProvider> _metadataReferenceProviders;
    readonly Assembly[] _assembly = AppDomain.CurrentDomain.GetAssemblies();

    public TextWriter TextWriter { get; init; }

    public CSharpRunner(IEnumerable<IMetadataReferenceProvider>? metadataReferenceProviders)
    {
        _metadataReferenceProviders = metadataReferenceProviders ?? new List<IMetadataReferenceProvider>();
        TextWriter = new StringWriter();
        Console.SetOut(TextWriter);
        // 恢复控制台输出
        //Console.SetOut(Console.Out);
    }

    public async Task<T> RunAsync<T>(string code, object? globals = null, List<string>? packages = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var metadataReferences = new List<MetadataReference>();
        foreach (var metadataReferenceProvider in _metadataReferenceProviders)
        {
            var packageNames = new List<string>();
            if (metadataReferenceProvider is WasmMetadataReferenceProvider)
            {
                packageNames = _assembly.Select(x => x.GetName().Name ?? "").Where(x => !string.IsNullOrEmpty(x)).ToList();
            }
            if (metadataReferenceProvider is NuGetMetadataReferenceProvider && packages?.Any() == true)
            {
                packageNames = packages;
            }
            metadataReferences.AddRange(await metadataReferenceProvider.GetMetaDataReferencesAsync(packageNames.ToArray()));
        }
        var scriptOptions = ScriptOptions.Default.AddImports(Namespaces.ImportedNamespaces)
                .AddReferences(metadataReferences)
                .WithEmitDebugInformation(true)
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithLanguageVersion(LanguageVersion.Latest);
        var script = CSharpScript.Create<T>(code, options: scriptOptions);

        var compilation = script.GetCompilation();

        var errors = compilation.GetDiagnostics()
            .Where(x => x.Severity == DiagnosticSeverity.Error)
            .ToArray();

        foreach (var error in errors)
        {
            Console.WriteLine(error.ToString());
        }
        //int dd = await CSharpScript.EvaluateAsync<int>("return 2 * 3;");
        var result = await script.RunAsync(globals, catchException: ex =>
        {
            Console.WriteLine($"~~~catchException~~~~~\n{ex}");
            return true;
        }, cancellationToken: cancellationToken);

        return result.ReturnValue;
    }
}
