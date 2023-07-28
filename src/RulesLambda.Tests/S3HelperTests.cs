using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using App.CloudServices.Helpers;
using App.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RulesLambda.Tests.Helpers;
using App.Core;

namespace RulesLambda.Tests;

public class S3HelperTests
{

    private IEnvironmentHelper MockEnvironmentHelper()
    {
        var environmentHelperStub = new Mock<IEnvironmentHelper>();
        environmentHelperStub.Setup(x => x.GetEnvironmentValue(Constants.ENVVAR_AWS_REGION, null)).Returns(TestConstants.TestAWSRegion);
        environmentHelperStub.Setup(x => x.CurrentAWSRegion).Returns(TestConstants.TestAWSRegion);

        return environmentHelperStub.Object;
    }

    [Fact]
    public async void TestS3Helper()
    {

        var result = (await new AwsHelper(MockEnvironmentHelper()).ReadS3TextFileAsync(TestConstants.RulesS3BucketName, "ValidRoute.json"));


        Assert.NotNull(result);
    }
}
