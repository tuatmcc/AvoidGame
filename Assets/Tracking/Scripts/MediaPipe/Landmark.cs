using Newtonsoft.Json;

namespace Tracking.MediaPipe
{
    public class Landmark
    {
        [JsonProperty("x")] public float X { get; set; } = 0;
        [JsonProperty("y")] public float Y { get; set; } = 0;
        [JsonProperty("z")] public float Z { get; set; } = 0;
        [JsonProperty("visibility")] public float Visibility { get; set; } = 0;
    }
}