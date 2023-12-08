using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using VeteranBot.Gateway.Application.Abstractions;
using VeteranBot.Gateway.Application.Dtos;

namespace VeteranBot.Gateway.Infrastructure.Implementations;

public class GcpFileStorage(
    StorageClient storageClient,
    GoogleCredential googleCredential
    ) : IFileStorage
{
    public string GeneratePreSignedUrl(
        string folder, 
        string name, 
        TimeSpan duration
        )
    {
        var urlSigner = UrlSigner.FromCredential(googleCredential);

        var url = urlSigner.Sign(folder, name, duration, HttpMethod.Get);

        return url;
    }

    public string GeneratePreSignedUrl(
        string folder, 
        string name, 
        DateTime expirationDate
        )
    {
        var url = GeneratePreSignedUrl(folder, name, expirationDate - DateTime.UtcNow);

        return url;
    }

    public async Task UploadAsync(
        string folder, 
        FileDto file, 
        CancellationToken cancellationToken
        )
    {
        await storageClient.UploadObjectAsync(
            folder, 
            file.Name, 
            file.ContentType, 
            file.Stream,
            cancellationToken: cancellationToken
            );
    }

    public async Task<FileDto> DownloadAsync(
        string folder,
        string name,
        CancellationToken cancellationToken
        )
    {
        var memoryStream = new MemoryStream();

        var @object = await storageClient.DownloadObjectAsync(
            folder, 
            name, 
            memoryStream,
            cancellationToken: cancellationToken
            );

        return new FileDto
        {
            Name = @object.Name,
            ContentType = @object.ContentType,
            Stream = memoryStream
        };
    }

    public async Task RemoveAsync(
        string folder, 
        string name, 
        CancellationToken cancellationToken
        )
    {
        await storageClient.DeleteObjectAsync(folder, name, cancellationToken: cancellationToken);
    }

    public async Task CopyAsync(
        string sourceFolder, 
        string sourceName, 
        string destinationFolder, 
        string destinationName,
        CancellationToken cancellationToken
        )
    {
        await storageClient.CopyObjectAsync(
            sourceFolder, 
            sourceName, 
            destinationFolder, 
            destinationName,
            cancellationToken: cancellationToken
            );
    }
}