using Amazon.Lambda.Core;
using RulesEngine.Models;
using RulesLambda.RulesExtensions;
using Newtonsoft.Json;
using RulesLambda.Models;
using App.CloudServices.Helpers;
using App.Core.Helpers;
using Utils = RulesEngine.HelperFunctions.Utils;
using Newtonsoft.Json.Converters;
using App.Core;

namespace RulesLambda;

public class LambdaFunction
{
    private IEnvironmentHelper _envHelper;
    private IAwsHelper _awsHelper;

    public LambdaFunction(IEnvironmentHelper envHelper, IAwsHelper awsHelper)
    {
        _envHelper = envHelper;
        _awsHelper = awsHelper;
    }
    public async Task<string> RunAsync(ExecuteWorkflow input, ILambdaContext context)
    {
        var filename = input.RuleSet;
        //if the file ruleset does not specify a file extension, assume .json
        if (!Path.HasExtension(filename)) { filename += ".json"; }
        var bucketName = await _awsHelper.GetParameterAsync(Constants.PRM_RULESENGINE_BUCKET);
        var workflowJson = await _awsHelper.ReadS3TextFileAsync(bucketName, filename);
        var workflowArr = JsonConvert.DeserializeObject<Workflow[]>(workflowJson, new ExpandoObjectConverter());
        if (workflowArr == null)
        {
            throw new InvalidDataException($"Workflow contents are invalid: {input.RuleSet}");
        }

        Fn.Region = _envHelper.CurrentAWSRegion;
        var reSettingsWithCustomTypes = new ReSettings { CustomTypes = new Type[] { typeof(AWS), typeof(Util), typeof(Fn) } };
        var engine = new RulesEngine.RulesEngine(workflowArr, reSettingsWithCustomTypes);
        var paramArray = input.Parameters.Select(p => new RuleParameter(p.Key, p.Value)).ToArray();

        var result = await engine.ExecuteAllRulesAsync(input.Workflow, paramArray);
        var response = result.Select(r => new { Rule = r.Rule.RuleName, Exception = r.ExceptionMessage, Success = r.IsSuccess });
        return JsonConvert.SerializeObject(response);
    }
}