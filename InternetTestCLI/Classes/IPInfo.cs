using System.Text.Json;
using System.Text.Json.Serialization;

namespace InternetTestCLI.Classes;

/// <summary>
/// This class contains an IP Information
/// </summary>
public class IPInfo
{
    /// <summary>
    /// <c>success</c> or <c>fail</c>.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    /// Country name.
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; set; }

    /// <summary>
    /// Two-letter country code ISO 3166-1 alpha-2.
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; }

    /// <summary>
    /// Region/state short code (FIPS or ISO).
    /// </summary>
    [JsonPropertyName("region")]
    public string Region { get; set; }

    /// <summary>
    /// Region/state.
    /// </summary>
    [JsonPropertyName("regionName")]
    public string RegionName { get; set; }

    /// <summary>
    /// City.
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; set; }

    /// <summary>
    /// Zip code.
    /// </summary>
    [JsonPropertyName("zip")]
    public string Zip { get; set; }

    /// <summary>
    /// Latitude.
    /// </summary>
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    /// <summary>
    /// Longitude.
    /// </summary>
    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    /// <summary>
    /// Timezone (tz).
    /// </summary>
    [JsonPropertyName("timezone")]
    public string TimeZone { get; set; }

    /// <summary>
    /// ISP name.
    /// </summary>
    [JsonPropertyName("isp")]
    public string ISP { get; set; }

    /// <summary>
    /// Organization name.
    /// </summary>
    [JsonPropertyName("org")]
    public string Org { get; set; }

    /// <summary>
    /// IP used for the query.
    /// </summary>
    [JsonPropertyName("query")]
    public string Query { get; set; }

    public override string ToString()
    {
        return $"Country: {Country}\n" +
            $"Region: {RegionName}\n" +
            $"City: {City}\n" +
            $"ZIP Code: {Zip}\n" +
            $"Latitude: {Lat}\n" +
            $"Longitude: {Lon}\n" +
            $"Timezone: {TimeZone}\n" +
            $"ISP: {ISP}\n";
    }

    public async static Task<IPInfo?> GetIPInfoAsync(string ip)
    {
        HttpClient httpClient = new();
        string result = await httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");

        return JsonSerializer.Deserialize<IPInfo>(result);
    }
}