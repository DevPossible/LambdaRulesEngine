namespace App.Core.Helpers
{
    public interface IEnvironmentHelper
    {
        string CurrentAWSRegion { get; }
        string CurrentLambdaName { get; }

        string GetEnvironmentValue(string name, string? defaultValue = null);
    }
}