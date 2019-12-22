using Newtonsoft.Json;
using System;

namespace BitlyDotNet
{
    public class Group
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string GUID { get; set; }

        [JsonProperty("organization_guid")]
        public string OrganizationGUID { get; set; }

        public string Name { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        public string Role { get; set; }
    }
}
