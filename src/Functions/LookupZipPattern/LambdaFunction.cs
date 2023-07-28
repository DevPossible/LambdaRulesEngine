using Amazon.Lambda.Core;
using App.CloudServices.Helpers;
using App.Core.Helpers;
using NamedZipCodePattern.Models;
using Newtonsoft.Json;

namespace LookupZipPattern;

public class LambdaFunction
{
    private IEnvironmentHelper _envHelper;
    private IAwsHelper _awsHelper;

    private Dictionary<string, string> _data = new Dictionary<string, string>()
    {
        { "West_Coast", "9*" },
        { "East_Coast", "0*" },
        { "Midwest", "4*" },
        { "South", "7*" }
    };

    public LambdaFunction(IEnvironmentHelper envHelper, IAwsHelper awsHelper)
    {
        _envHelper = envHelper;
        _awsHelper = awsHelper;
    }


    public async Task<NamedZipCodePatternResponse> RunAsync(NamedZipCodePatternRequest input, ILambdaContext context)
    {
        var inputObj = input;
        if (inputObj == null)
        {
            throw new ArgumentException("Could not deserialize input into ExecuteRule model", nameof(input));
        }

        if (_data.ContainsKey(input.Names))
        {
            var result = new NamedZipCodePatternResponse() { Message = $"Pattern {input.Names} exists", Pattern = _data[input.Names], Success = true };
            var response = await Task.FromResult(result);
            return response;
        }
        else
        {
            var result = new NamedZipCodePatternResponse() { Message = $"Pattern {input.Names} not found!", Pattern = string.Empty, Success = false };
            var response = await Task.FromResult(result);
            return response;
        }
    }
}