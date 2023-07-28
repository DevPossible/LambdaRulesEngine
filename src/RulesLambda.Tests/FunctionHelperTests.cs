using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RulesLambda.RulesExtensions;
using RulesLambda.Tests.Helpers;
using App.Core;

namespace RulesLambda.Tests;

public class FunctionHelperTests
{
    public FunctionHelperTests()
    {
        //functions require the region environment variable
        //since functions must be static, this is difficult to mock and must be set via this property
        Fn.Region = TestConstants.TestAWSRegion;
    }



    [Fact]
    public void GetVehicleTypeShouldNotFail()
    {
        var result = Fn.GetVehicleType(TestConstants.TruckVin);

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData("3C63RRHL3NG152998", "TRUCK")]

    public void GetVehicleTypeShouldReturnExpected(string vin, string vehicleType)
    {
        var result = Fn.GetVehicleType(vin);

        Assert.Equal(vehicleType, result);
    }

    [Theory]
    [InlineData(TestConstants.TruckVin, "Vehicle Type", "TRUCK")]
    [InlineData(TestConstants.TruckVin, "Model Year", "2022")]
    [InlineData(TestConstants.TruckVin, "Model", "3500")]

    public void GetVehicleDataByVariableShouldReturnExpected(string vin, string variable, string expectedResult)
    {
        var result = Fn.GetVehicleData(vin, variable);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(TestConstants.TruckVin, "39", "TRUCK")] //39 = Vehicle Type
    [InlineData(TestConstants.TruckVin, "29", "2022")] //29 = Model year
    [InlineData(TestConstants.TruckVin, "28", "3500")] //28 = Model

    public void GetVehicleDataByVariableIdShouldReturnExpected(string vin, string variable, string expectedResult)
    {
        var result = Fn.GetVehicleData(vin, variable);

        Assert.Equal(expectedResult, result);
    }
}
