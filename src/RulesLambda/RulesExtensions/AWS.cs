using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Newtonsoft.Json;

namespace RulesLambda.RulesExtensions
{
    public static class AWS
    {
        public static string ExecuteLambda(string region, string name, object input)
        {
            var serializer = new JsonSerializer();
            
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            serializer.Serialize(sw,input);
            var payloadJsonString = sb.ToString();

            var lambdaConfig = new AmazonLambdaConfig() { RegionEndpoint = RegionEndpoint.GetBySystemName(region) };
            var lambdaClient = new AmazonLambdaClient(lambdaConfig);

            var lambdaRequest = new InvokeRequest
            {
                FunctionName = name,
                InvocationType = InvocationType.RequestResponse,
                Payload = payloadJsonString
            };

            var response = lambdaClient.InvokeAsync(lambdaRequest).Result;
            var result = new StreamReader(response.Payload).ReadToEnd();
            return result;
        }
    }
}
