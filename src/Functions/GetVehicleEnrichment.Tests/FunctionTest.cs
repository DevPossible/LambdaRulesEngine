using Amazon.Lambda.TestUtilities;
using App.Core;
using App.Core.Helpers;
using GetVehicleEnrichment;
using GetVehicleEnrichment.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace EvalApprovedZipPair.Tests;

public class FunctionTest
{

    private IEnvironmentHelper MockEnvironmentHelper()
    {
        var environmentHelperStub = new Mock<IEnvironmentHelper>();
        environmentHelperStub.Setup(x => x.GetEnvironmentValue(Constants.ENVVAR_AWS_REGION, null)).Returns(TestConstants.TestAWSRegion);
        environmentHelperStub.Setup(x => x.CurrentAWSRegion).Returns(TestConstants.TestAWSRegion);

        return environmentHelperStub.Object;
    }

    [Fact]
    public async void GetValidVehicleInformationShouldReturnNotNull()
    {
        var mEnvHelper = MockEnvironmentHelper();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<LambdaFunction>();
        serviceCollection.AddSingleton<IEnvironmentHelper>(mEnvHelper);

        var startup = new Startup(mEnvHelper);
        var services = await startup.ConfigureServices(serviceCollection);

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new LambdaEntry(services);
        var context = new TestLambdaContext();

        var requestObj = new GetVehicleEnrichment.Models.GetVehicleEnrichmentRequest
        {
            Vin = TestConstants.TruckVin
        };

        var result = await function.MainEntryPoint(requestObj, context);
        Assert.NotNull(result);
    }

}
