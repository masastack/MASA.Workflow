namespace Masa.Workflow.Interractive;

internal interface IMetadataReferenceProvider
{
    public Task<ImmutableArray<MetadataReference>> GetMetaDataReferencesAsync(params string[] packageNames);
}
