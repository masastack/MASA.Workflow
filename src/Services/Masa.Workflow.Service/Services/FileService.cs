using static Masa.Workflow.FileService;

namespace Masa.Workflow.Service.Services;

public class FileService : FileServiceBase
{
    public override async Task<Empty> Upload(IAsyncStreamReader<FileRequest> requestStream, ServerCallContext context)
    {
        await using var writeStream = File.Create("data.bin");

        await foreach (var message in requestStream.ReadAllAsync())
        {
            if (message.File != null)
            {
                await writeStream.WriteAsync(message.File.Memory);
            }
        }
        return new Empty();
    }
}
