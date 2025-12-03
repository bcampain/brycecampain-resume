using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace api;

public interface IResumeStorageService
{
    Task<byte[]> DownloadAsync(CancellationToken cancellationToken = default);
    Task<string?> GetRevisionTextAsync(CancellationToken cancellationToken = default);
}

public class ResumeStorageService : IResumeStorageService
{
    private const string ResumeUri = "https://bryceresumestore.blob.core.windows.net/public/Bryce-Campain-Resume.pdf";
    private readonly ILogger<ResumeStorageService> _logger;
    private readonly BlobClient _blobClient;

    public ResumeStorageService(ILogger<ResumeStorageService> logger)
    {
        _logger = logger;
        _blobClient = new BlobClient(new Uri(ResumeUri));
    }

    public async Task<byte[]> DownloadAsync(CancellationToken cancellationToken = default)
    {
        var download = await _blobClient.DownloadContentAsync(cancellationToken);
        return download.Value.Content.ToArray();
    }

    public async Task<string?> GetRevisionTextAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var properties = await _blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);
            if (properties.Value.Metadata.TryGetValue("revisiondate", out var revisionValue))
            {
                return revisionValue;
            }
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to read revision metadata for {Blob}", _blobClient.Name);
            return null;
        }

        return null;
    }
}
