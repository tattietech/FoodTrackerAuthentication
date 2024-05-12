using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace foodTrackerAuth.Models
{
    public class AuthResponse
    {
        [JsonProperty("access_token")]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
