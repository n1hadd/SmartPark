using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using SmartPark.Models;

namespace SmartPark.Data
{
    public class OverpassApiHelper
    {
        private readonly HttpClient _http;

        public OverpassApiHelper(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Parkirisce>> GetParkiriscaLjubljanaAsync()
        {
            var parkirisca = new List<Parkirisce>();

            // Overpass query za Ljubljana parkirišča
            string query = @"[out:json];
                            area[""name""=""Ljubljana""]->.searchArea;
                            (
                              node[""amenity""=""parking""][""name""](area.searchArea);
                              way[""amenity""=""parking""][""name""](area.searchArea);
                            );
                            out center;";

            var apiUrl = "https://overpass-api.de/api/interpreter?data=" + Uri.EscapeDataString(query);

            var response = await _http.GetFromJsonAsync<JsonElement>(apiUrl);

            if (response.TryGetProperty("elements", out JsonElement elements))
            {
                foreach (var e in elements.EnumerateArray())
                {
                    double lat = 0;
                    double lon = 0;

                    // Node ali center od way
                    if (e.TryGetProperty("lat", out var latElem))
                        lat = latElem.GetDouble();
                    else if (e.TryGetProperty("center", out var center) &&
                             center.TryGetProperty("lat", out var latC))
                        lat = latC.GetDouble();

                    if (e.TryGetProperty("lon", out var lonElem))
                        lon = lonElem.GetDouble();
                    else if (e.TryGetProperty("center", out var center2) &&
                             center2.TryGetProperty("lon", out var lonC))
                        lon = lonC.GetDouble();

                    if (lat == 0 || lon == 0)
                        continue;

                    // Ime
                    string name = "Parkirišče brez naslova";
                    if (e.TryGetProperty("tags", out var tags) &&
                        tags.TryGetProperty("name", out var nameElem))
                    {
                        name = nameElem.GetString() ?? name;
                    }

                    // Capacity
                    int steviloMest = 0;
                    if (e.TryGetProperty("tags", out var tags2) &&
                        tags2.TryGetProperty("capacity", out var capElem))
                    {
                        var capStr = capElem.GetString();
                        int.TryParse(capStr, out steviloMest);
                    }

                    parkirisca.Add(new Parkirisce
                    {
                        Naslov = name,
                        Latitude = lat,
                        Longitude = lon,
                        SteviloMest = steviloMest,
                        CenaNaUro = 2.5m,
                        DelovniCas = "00:00–24:00"
                    });
                }
            }

            return parkirisca;
        }
    }
}
