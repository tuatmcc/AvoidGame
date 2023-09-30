using Newtonsoft.Json;

namespace Tracking
{
    public struct LandmarkJson
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("x")] public string X { get; set; }
        [JsonProperty("y")] public string Y { get; set; }
        [JsonProperty("z")] public string Z { get; set; }
        [JsonProperty("visibility")] public string Visibility { get; set; }
    }
}