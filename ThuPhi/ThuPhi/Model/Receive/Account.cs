using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThuPhi.Model.Receive
{
    class Account : Send.Login
    {
        public string Role { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
