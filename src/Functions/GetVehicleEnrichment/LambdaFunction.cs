using Amazon.Lambda.Core;
using GetVehicleEnrichment.Helpers;
using GetVehicleEnrichment.Models;
using Newtonsoft.Json;

namespace GetVehicleEnrichment;

public class LambdaFunction
{

    public LambdaFunction()
    {
    }

    public async Task<GetVehicleEnrichmentResponse> RunAsync(GetVehicleEnrichmentRequest input, ILambdaContext context)
    {
        var inputObj = input;
        if (inputObj == null)
        {
            throw new ArgumentException("Could not deserialize input into ExecuteRule model", nameof(input));
        }

        try
        {
            var decoder = new VinDecoderHelper();
            var result = await decoder.DecodeAsync(input.Vin);

            return result;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return new GetVehicleEnrichmentResponse() { Count = 0, Message = e.Message };
        }
    }
}