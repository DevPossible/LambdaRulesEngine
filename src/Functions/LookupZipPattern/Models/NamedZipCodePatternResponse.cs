namespace NamedZipCodePattern.Models;

public class NamedZipCodePatternResponse
{
    public bool Success { get; set; }
    public string? Pattern { get; set; }
    public string? Message { get; set; }
}