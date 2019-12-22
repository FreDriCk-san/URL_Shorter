using Newtonsoft.Json;

namespace BitlyDotNet
{
    internal class ShortenRequest
    {
        [JsonProperty("long_url")]
        public string LongUrl { get; set; }

        [JsonProperty("group_guid")]
        public string GroupGuid { get; set; }
    }
}
