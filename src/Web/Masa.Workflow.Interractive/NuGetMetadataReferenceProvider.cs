using NuGet.Packaging;

namespace Masa.Workflow.Interractive;

internal class NuGetMetadataReferenceProvider : IMetadataReferenceProvider
{
    SourceCacheContext _cache = new SourceCacheContext();
    SourceRepository _repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");

    /// <summary>
    /// Get metadata references from nuget packages
    /// </summary>
    /// <param name="packageNames">
    /// nuget package eg:"AutoMapper,6.0.0"
    /// </param>
    /// <returns></returns>
    public async Task<ImmutableArray<MetadataReference>> GetMetaDataReferencesAsync(params string[] packageNames)
    {
        var result = new List<MetadataReference>();
        CancellationToken cancellationToken = CancellationToken.None;
        FindPackageByIdResource resource = await _repository.GetResourceAsync<FindPackageByIdResource>();
        var packages = packageNames.Select(x => x.Split(',')).Select(x => new { PackageId = x[0], PackageVersion = x[1] }).ToList();
        foreach (var package in packages)
        {
            using MemoryStream packageStream = new MemoryStream();

            await resource.CopyNupkgToStreamAsync(
                package.PackageId,
                new NuGetVersion(package.PackageVersion),
                packageStream,
                _cache,
                NullLogger.Instance,
                cancellationToken);

            using PackageArchiveReader packageReader = new PackageArchiveReader(packageStream);
            NuspecReader nuspecReader = await packageReader.GetNuspecReaderAsync(cancellationToken);

            foreach (var file in packageReader.GetFiles())
            {
                if (Path.GetExtension(file).Equals(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    using Stream fileStream = packageReader.GetStream(file);
                    using MemoryStream memoryStream = new MemoryStream();
                    await fileStream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin); // 重置内存流位置到起始位置
                    result.Add(MetadataReference.CreateFromStream(memoryStream));
                }
            }
        }
        return result.ToImmutableArray();
    }
}