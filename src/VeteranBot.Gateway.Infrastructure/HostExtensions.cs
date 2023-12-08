using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VeteranBot.Gateway.Infrastructure;

public static class HostExtensions
{
    public static async Task<IHost> AwsS3PutBucketsAsync(
        this IHost host,
        IEnumerable<string> buckets, 
        CancellationToken cancellationToken
        )
    {
        await using var scope = host.Services.CreateAsyncScope();

        return host;
    }
}