using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using RulesLambda.Models;
using Amazon.Runtime.Internal.Transform;
using RulesEngine.Models;
using System.Diagnostics.Metrics;
using System.Dynamic;
using App.CloudServices.Helpers;
using App.Core.Helpers;
using RulesLambda.RulesExtensions;
using App.Core;
using RulesLambda.Tests.Helpers;

namespace RulesLambda.Tests;

public class WorkflowTests
{

    public WorkflowTests()
    {
        //This should not be necessary, and should be handled via mock and dependency injection
        //Environment.SetEnvironmentVariable(Constants.ENVVAR_AWS_REGION, TestConstants.TestAWSRegion);
    }


    private IEnvironmentHelper MockEnvironmentHelper()
    {
        var environmentHelperStub = new Mock<IEnvironmentHelper>();
        environmentHelperStub.Setup(x => x.GetEnvironmentValue(Constants.ENVVAR_AWS_REGION, null)).Returns(TestConstants.TestAWSRegion);
        environmentHelperStub.Setup(x => x.CurrentAWSRegion).Returns(TestConstants.TestAWSRegion);

        return environmentHelperStub.Object;
    }

    private IAwsHelper MockAwsHelper(Dictionary<string, string> s3files)
    {
        var stub = new Mock<IAwsHelper>();
        foreach (var file in s3files)
        {
            stub.Setup(x => x.ReadS3TextFileAsync(Moq.It.IsAny<string>(), file.Key, null)).ReturnsAsync(file.Value);
        }

        return stub.Object;
    }

    private Workflow[] GetTestWorkflowArray(string[] expressions)
    {
        var wf = new Workflow { WorkflowName = "TestWorkflow" };
        var rules = new List<Rule>();

        for (int i = 0; i < expressions.Length; i++)
        {
            var name = $"TestRule{i}";
            rules.Add(new Rule()
            {
                RuleName = name,
                SuccessEvent = "10",
                RuleExpressionType = RuleExpressionType.LambdaExpression,
                ErrorMessage = $"{name} failed",
                Expression = expressions[i]
            }
            );
        }

        wf.Rules = rules;

        return new[] { wf };
    }

    public class ExecuteWorkflowResponse
    {
        public string Rule { get; set; }
        public string Exception { get; set; }
        public bool Success { get; set; }
    }

    [Fact]
    public async void ValidRouteWorkflowShouldNotFail()
    {

        //simulate normal startup
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<LambdaFunction>();
        var mEnvHelper = MockEnvironmentHelper();
        var startup = new Startup(mEnvHelper, new AwsHelper(mEnvHelper));
        var services = await startup.ConfigureServices(serviceCollection);

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new LambdaEntry(services);
        var context = new TestLambdaContext();

        dynamic paramObj = new ExpandoObject();
        paramObj.Origin = "35216";
        paramObj.Destination = "32123";

        var requestObj = new ExecuteWorkflow
        {
            RuleSet = "ValidRoute",
            Workflow = "ValidRoute",

            Parameters = paramObj
        };

        var reqStr = JsonConvert.SerializeObject(requestObj);
        Assert.NotNull(reqStr);

        var resultJson = function.MainEntryPoint(requestObj, context).Result;
        var result = JsonConvert.DeserializeObject<ExecuteWorkflowResponse[]>(resultJson);
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.True(result[0].Success);
        Assert.True(result[1].Success);
    }


    [Fact]
    public async void ValidVehicleWorkflowShouldNotFailForTruck()
    {


        //simulate normal startup
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<LambdaFunction>();
        var mEnvHelper = MockEnvironmentHelper();
        var startup = new Startup(mEnvHelper, new AwsHelper(mEnvHelper));
        var services = await startup.ConfigureServices(serviceCollection);

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new LambdaEntry(services);
        var context = new TestLambdaContext();

        dynamic paramObj = new ExpandoObject();
        paramObj.vin = TestConstants.TruckVin;

        var requestObj = new ExecuteWorkflow
        {
            RuleSet = "ValidVehicle",
            Workflow = "ValidVehicle",

            Parameters = paramObj
        };

        var reqStr = JsonConvert.SerializeObject(requestObj);
        Assert.NotNull(reqStr);

        var resultJson = function.MainEntryPoint(requestObj, context).Result;
        var result = JsonConvert.DeserializeObject<ExecuteWorkflowResponse[]>(resultJson);
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.True(result[0].Success);
        Assert.True(result[1].Success);
    }

    [Fact]
    public async void ValidVehicleWorkflowShouldFailForCar()
    {


        //simulate normal startup
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<LambdaFunction>();
        var mEnvHelper = MockEnvironmentHelper();
        var startup = new Startup(mEnvHelper, new AwsHelper(mEnvHelper));
        var services = await startup.ConfigureServices(serviceCollection);

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new LambdaEntry(services);
        var context = new TestLambdaContext();

        dynamic paramObj = new ExpandoObject();
        paramObj.vin = TestConstants.CarVin;

        var requestObj = new ExecuteWorkflow
        {
            RuleSet = "ValidVehicle",
            Workflow = "ValidVehicle",

            Parameters = paramObj
        };

        var reqStr = JsonConvert.SerializeObject(requestObj);
        Assert.NotNull(reqStr);

        var resultJson = function.MainEntryPoint(requestObj, context).Result;
        var result = JsonConvert.DeserializeObject<ExecuteWorkflowResponse[]>(resultJson);
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.False(result[0].Success);
        Assert.False(result[1].Success);
    }



}
