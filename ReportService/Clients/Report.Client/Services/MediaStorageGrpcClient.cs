using Google.Protobuf;
using MediaClient.Grpc;
using Microsoft.AspNetCore.Http;
using Report.Client.AbstructServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Report.Client.Services
{
    public class MediaStorageGrpcClient(MediaStorage.MediaStorageClient client) : IMediaStorageGrpcClient
    {
        public async Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(file);

            using var call = client.UploadMedia(cancellationToken: cancellationToken);

            await call.RequestStream.WriteAsync(new UploadMediaRequest
            {
                Metadata = new FileMetadata
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType
                }
            });

            await using var stream = file.OpenReadStream();
            var buffer = new byte[64 * 1024];

            int read;
            while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken)) > 0)
            {
                await call.RequestStream.WriteAsync(new UploadMediaRequest
                {
                    Chunk = ByteString.CopyFrom(buffer, 0, read)
                });
            }

            await call.RequestStream.CompleteAsync();

            return (await call.ResponseAsync).Filepath;
        }
    }
}
