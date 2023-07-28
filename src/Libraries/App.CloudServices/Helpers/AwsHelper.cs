using Amazon;
using Amazon.S3.Model;
using Amazon.S3;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using App.Core.Helpers;
using System.Threading.Tasks;
using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using App.CloudServices.Models;
using Newtonsoft.Json;
using Amazon.Runtime.Endpoints;

namespace App.CloudServices.Helpers
{
    public class AwsHelper : IAwsHelper
    {
        private IEnvironmentHelper _environmentHelper;

        public AwsHelper(IEnvironmentHelper environmentHelper)
        {
            _environmentHelper = environmentHelper;
        }

        public string CurrentRegion => _environmentHelper.CurrentAWSRegion;
        public RegionEndpoint CurrentRegionEndpoint => RegionEndpoint.GetBySystemName(_environmentHelper.CurrentAWSRegion);

        private RegionEndpoint GetRegionEndpoint(string? name)
        {
            return RegionEndpoint.GetBySystemName(name ?? CurrentRegion ?? string.Empty);
        }

        public async Task<string> GetParameterAsync(string name, string? region = null)
        {
            var ssmClient = new AmazonSimpleSystemsManagementClient(GetRegionEndpoint(region));

            var response = await ssmClient.GetParameterAsync(new GetParameterRequest
            {
                Name = name,
                WithDecryption = true
            });

            return response.Parameter.Value;
        }

        public async Task<string> ReadS3TextFileAsync(string bucket, string fileName, string? region = null)
        {
            var client = new AmazonS3Client(GetRegionEndpoint(region));
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucket,
                Key = fileName
            };

            GetObjectResponse response = await client.GetObjectAsync(request);
            StreamReader reader = new StreamReader(response.ResponseStream);
            string content = await reader.ReadToEndAsync();
            return content;
        }

        public async Task<SecretCredentials> GetDBCredentialsAsync(string secretName, string? region = null)
        {
            var result = await GetSecretAsync(secretName, region);
            var creds = JsonConvert.DeserializeObject<SecretCredentials>(result);
            return creds;
        }

        public async Task<string> GetSecretAsync(string secretName, string? region = null)
        {

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(GetRegionEndpoint(region));

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName
            };

            GetSecretValueResponse response;

            try
            {
                response = await client.GetSecretValueAsync(request);
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving secret: {secretName} from region: {GetRegionEndpoint(region)}", e);
            }

            string secret = response.SecretString;

            return secret;
        }

        public string GetPostgresConnectionString(string host, string database, string username, string password)
        {
            return $"Host={host};Database={database};Username={username};Password={password}";
        }
    }
}