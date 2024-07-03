using Newtonsoft.Json;

namespace AvoidGame.MediaPipe
{
    // TODO: Vector3 か、それを継承したやつにすべきだったかな
    public class Landmark
    {
        [JsonProperty("x")] public float X { get; set; }
        [JsonProperty("y")] public float Y { get; set; }
        [JsonProperty("z")] public float Z { get; set; }
        [JsonProperty("visibility")] public float Visibility { get; set; }
    }
}