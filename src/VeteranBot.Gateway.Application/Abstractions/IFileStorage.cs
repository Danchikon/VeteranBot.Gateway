using VeteranBot.Gateway.Application.Dtos;

namespace VeteranBot.Gateway.Application.Abstractions;

public interface IFileStorage
{
    string GeneratePreSignedUrl(
        string folder, 
        string name,
        TimeSpan duration
    );
    
    string GeneratePreSignedUrl(
        string folder, 
        string name,
        DateTime expirationDate
    );
    
    Task UploadAsync(
        string folder, 
        FileDto file, 
        CancellationToken cancellationToken
        );
    
    Task<FileDto> DownloadAsync(
        string folder,
        string name,
        CancellationToken cancellationToken
        );
    
    Task RemoveAsync(
        string folder,
        string name,
        CancellationToken cancellationToken
    );
    
    Task CopyAsync(
        string sourceFolder, 
        string sourceName,
        string destinationFolder,
        string destinationName,
        CancellationToken cancellationToken
        );
}
