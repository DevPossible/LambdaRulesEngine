using Amazon.Lambda.TestUtilities;
using App.CloudServices.Helpers;
using App.Core;
using App.Core.Helpers;
using LookupZipPattern;
using LookupZipPattern.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NamedZipCodePattern.Models;
using Xunit;

namespace NamedZipCodePattern.Tests;

public class FunctionTest
{

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



    [Fact]
    public async void LookupValidZipPatternShouldReturnTrue()
    {
        var mEnvHelper = MockEnvironmentHelper();
        var mAwsHelper = new AwsHelper(mEnvHelper);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<LambdaFunction>();
        serviceCollection.AddSingleton<IEnvironmentHelper>(mEnvHelper);
        serviceCollection.AddSingleton<IAwsHelper>(mAwsHelper);

        var startup = new Startup(mEnvHelper, mAwsHelper);
        var services = await startup.ConfigureServices(serviceCollection);

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new LambdaEntry(services);
        var context = new TestLambdaContext();

        var requestObj = new NamedZipCodePatternRequest
        {
            Names = "West_Coast"
        };

        var result = await function.MainEntryPoint(requestObj, context);
        Assert.NotNull(result);
    }
}
