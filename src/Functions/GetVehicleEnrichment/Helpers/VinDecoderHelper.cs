using System.Dynamic;
using System.Runtime.Serialization;
using GetVehicleEnrichment.Models;
using Newtonsoft.Json;

namespace GetVehicleEnrichment.Helpers;

public class VinDecoderHelper
{

    private const string RequestUrl = "https://vpic.nhtsa.dot.gov/api/vehicles/decodevin";

    public async Task<GetVehicleEnrichmentResponse> DecodeAsync(string vin)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, $"{RequestUrl}/{vin}?format=json");

        var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(message);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var vinRestResponse = JsonConvert.DeserializeObject<GetVehicleEnrichmentResponse>(content);
            if (vinRestResponse == null) { throw new SerializationException("Could not deserialize response from NHTSA."); }

            return vinRestResponse;
        }

        throw new HttpRequestException($"[{response.StatusCode}] Could not decode VIN: {vin}.");
    }
}