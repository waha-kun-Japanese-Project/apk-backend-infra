using Media.ServiceAbstraction;
using Media.Settings;
using Media.Shared.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;


namespace Media.Service
{
    public class MinioStorageService : IStorageService
    {
        private readonly IMinioClient client;
        private readonly MinioSettings settings;
        public MinioStorageService(IMinioClient client, IOptions<MinioSettings> options)
        {
            this.client = client;
            this.settings = options.Value;
        }

        public async Task DeleteAsync(string objectName, CancellationToken cancellationToken = default)
        {
            await client.RemoveObjectAsync(
                new RemoveObjectArgs()
                    .WithBucket(settings.BucketName)
                    .WithObject(objectName),
                cancellationToken);


        }

        public async Task<Stream> DownloadAsync(string objectName, CancellationToken cancellationToken = default)
        {
           var memoeryStream = new MemoryStream();
            await client.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(settings.BucketName)
                    .WithObject(objectName)
                    .WithCallbackStream(stream =>
                    {
                        stream.CopyTo(memoeryStream);
                    }),
                cancellationToken);
            memoeryStream.Position = 0;
            return memoeryStream;
        }

        public async Task<FileResponse> GetAsync(string objectName, CancellationToken cancellationToken = default)
        {
            var stat = await client.StatObjectAsync(
            new StatObjectArgs()
                .WithBucket(settings.BucketName)
                .WithObject(objectName),
            cancellationToken);

            if (stat == null)
                return null;

       return new FileResponse(stat.ObjectName,
           $"{settings.Endpoint}/{settings.BucketName}/{stat.ObjectName}",
           $"{settings.Endpoint}/{settings.BucketName}/{stat.ObjectName}",
           stat.Size,
           stat.ContentType);
        }

        public async Task<UploadFileResponse> UploadFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
        {
            var bucketExists = await client.BucketExistsAsync(
                  new BucketExistsArgs().WithBucket(settings.BucketName), cancellationToken);
            if (!bucketExists)
            {
                await client.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(settings.BucketName), cancellationToken);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var objectName = $"{folder}/{fileName}";
            using var stream = file.OpenReadStream();
            await client.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(settings.BucketName)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(file.ContentType),
                cancellationToken);

            return new UploadFileResponse(fileName, objectName, $"{settings.Endpoint}/{settings.BucketName}/{objectName}");
        }
    }
}
