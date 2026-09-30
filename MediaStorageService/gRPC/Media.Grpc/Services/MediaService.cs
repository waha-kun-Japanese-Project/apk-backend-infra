using Grpc.Core;
using Media;
using Media.Grpc.Protos;
using Media.ServiceAbstraction;
using Media.Shared.DTOS;

namespace Media.Grpc.Services
  
{
    public class MediaService(IStorageService storageService) : MediaStorage.MediaStorageBase {

        public override async Task<UploadMediaRespones> UploadMedia( IAsyncStreamReader<UploadMediaRequest> requestStream, ServerCallContext context)
        {

            string? fileName = null;
            string? contentType = null;

            await using var stream = new MemoryStream();

            await foreach (var request in requestStream.ReadAllAsync())
            {
                if (request.DataCase == UploadMediaRequest.DataOneofCase.Metadata)
                {
                    fileName = request.Metadata.FileName;
                    contentType = request.Metadata.ContentType;
                }
                else if (request.DataCase == UploadMediaRequest.DataOneofCase.Chunk)
                {
                    request.Chunk.WriteTo(stream);
                }
            }

            stream.Position = 0;

            var result = await storageService.UploadStreamAsync(
                new UploadStreamRequest(
                    stream,
                    fileName!,
                    contentType ?? "application/octet-stream",
                    "issues"),
                context.CancellationToken);

            return new UploadMediaRespones
            {
                Filepath = result.filePath
            };
        }
    }

    }

