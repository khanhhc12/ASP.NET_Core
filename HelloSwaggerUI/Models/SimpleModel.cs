using Newtonsoft.Json;

namespace HelloSwaggerUI.Models
{
    public class SimpleModel
    {
        [JsonProperty("value")]
        public int id { get; set; }
        [JsonProperty("text")]
        public string name { get; set; }
    }
}