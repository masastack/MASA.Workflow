namespace Masa.Workflow.Interactive;

internal class WasmMetadataReferenceProvider : IMetadataReferenceProvider
{
    private readonly HttpClient _httpClient;

    public WasmMetadataReferenceProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ImmutableArray<MetadataReference>> GetMetaDataReferencesAsync(params string[] packageNames)
    {
        var result = new List<MetadataReference>();
        var extendName = ".dll";
        foreach (var packageName in packageNames.Select(x => x.EndsWith(extendName) ? x : x + extendName))
        {
            result.Add(MetadataReference.CreateFromStream(await _httpClient.GetStreamAsync("/_framework/" + packageName)));
        }
        return result.ToImmutableArray();
    }
}
