namespace Masa.Workflow.Interactive;

public interface IMetadataReferenceProvider
{
    public Task<ImmutableArray<MetadataReference>> GetMetaDataReferencesAsync(params string[] packageNames);
}
