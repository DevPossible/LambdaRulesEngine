using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesLambda.Tests.Helpers;

public static class TestConstants
{
    public const string TestAWSRegion = "us-east-1";
    public const string TruckVin = "3C63RRHL3NG152998"; // vin from 2022 RAM 3500 Bighorn (https://www.carmax.com/car/beta/22861891)
    public const string CarVin = "JM1NDAM74K0305334"; //vin from 2019 Mazda MX-5 Miata RF Grand Touring https://www.carmax.com/car/beta/23722054
    public const string RulesS3BucketName = "devpossible.ruleworkflows";

}
