using Amazon;
using App.CloudServices.Models;

namespace App.CloudServices.Helpers
{
    public interface IAwsHelper
    {
        string CurrentRegion { get; }
        RegionEndpoint CurrentRegionEndpoint { get; }

        Task<SecretCredentials> GetDBCredentialsAsync(string secretName, string? region = null);
        Task<string> GetParameterAsync(string name, string? region = null);
        string GetPostgresConnectionString(string host, string database, string username, string password);
        Task<string> GetSecretAsync(string secretName, string? region = null);
        Task<string> ReadS3TextFileAsync(string bucket, string fileName, string? region = null);
    }
}