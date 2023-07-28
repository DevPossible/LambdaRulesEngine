namespace GetVehicleEnrichment.Models;

public class GetVehicleEnrichmentResponse
{
    public int Count { get; set; }
    public string Message { get; set; }
    public VinLookupValueRecord[] Results { get; set; }
}