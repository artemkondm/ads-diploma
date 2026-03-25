using System.Text.Json;
using Ads.DTO.Ads;
using Ads.Services.Interfaces;

namespace Ads.Services;

public class GeoService : IGeoService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeoService(HttpClient client, IConfiguration config)
    {
        _httpClient = client;
        _apiKey = config["Yandex:ApiKey"];    
    }

    public async Task<GeocodeResult?> GeocodeAsync(string address)
{
    // 1. Проверяем, что ключ вообще есть в конфиге
    if (string.IsNullOrEmpty(_apiKey)) 
        throw new InvalidOperationException("API Key for Yandex Maps is missing in configuration.");

    var url = $"https://geocode-maps.yandex.ru/1.x/?apikey={_apiKey}&geocode={Uri.EscapeDataString(address)}&format=json";
    
    var response = await _httpClient.GetAsync(url);
    if (!response.IsSuccessStatusCode) return null;

    var jsonString = await response.Content.ReadAsStringAsync();
    using var json = JsonDocument.Parse(jsonString);

    // Безопасно проваливаемся до featureMember
    if (!json.RootElement.TryGetProperty("response", out var resp) ||
        !resp.TryGetProperty("GeoObjectCollection", out var coll) ||
        !coll.TryGetProperty("featureMember", out var featureMember) ||
        featureMember.GetArrayLength() == 0)
    {
        return null;
    }

    var geoObject = featureMember[0].GetProperty("GeoObject");

    // Извлекаем координаты
    var pos = geoObject.GetProperty("Point").GetProperty("pos").GetString();
    if (string.IsNullOrEmpty(pos)) return null;

    // Извлекаем компоненты адреса
    var metaData = geoObject.GetProperty("metaDataProperty").GetProperty("GeocoderMetaData");
    var addressDetails = metaData.GetProperty("Address");
    
    if (!addressDetails.TryGetProperty("Components", out var components)) return null;

    var result = new GeocodeResult();

    foreach (var component in components.EnumerateArray())
    {
        var kind = component.GetProperty("kind").GetString();
        var name = component.GetProperty("name").GetString();

        switch (kind)
        {
            case "province": result.Region = name; break;
            case "locality": result.City = name; break;
            case "street": result.Street = name; break;
            case "house": result.House = name; break;
        }
    }

    var coords = pos.Split(' '); // ВНИМАНИЕ: Яндекс часто отдает координаты через пробел, а не запятую
    if (coords.Length < 2) return null;

    result.Longitude = double.Parse(coords[0], System.Globalization.CultureInfo.InvariantCulture);
    result.Latitude = double.Parse(coords[1], System.Globalization.CultureInfo.InvariantCulture);
    
    // if (metaData.TryGetProperty("id", out var idProp))
    // {
    //     result.CityGeoId = idProp.GetString();
    // }

    return result;
}
}