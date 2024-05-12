using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace foodTrackerAuth.Models
{
    public class RefreshTokenRequest
    {
        [JsonProperty("refresh_token")]
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
