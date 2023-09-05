namespace Masa.Workflow.Interactive;

internal interface IMetadataReferenceProvider
{
    public Task<ImmutableArray<MetadataReference>> GetMetaDataReferencesAsync(params string[] packageNames);
}
