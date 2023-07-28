namespace App.Core.Helpers
{
    public class EnvironmentHelper : IEnvironmentHelper
    {
        public string GetEnvironmentValue(string name, string? defaultValue = null)
        {
            string? value;

            try
            {
                value = Environment.GetEnvironmentVariable(name) ?? defaultValue;
            }
            catch (Exception e)
            {
                throw new Exception($"Cannot retrieve environment variable {name}.", e);
            }

            if (value == null) throw new Exception($"Environment variable {name} not set.");

            return value;
        }

        public string CurrentAWSRegion => GetEnvironmentValue(Constants.ENVVAR_AWS_REGION) ?? string.Empty;
        public string CurrentLambdaName => GetEnvironmentValue(Constants.ENVVAR_LAMBDA_NAME) ?? string.Empty;
    }
}
