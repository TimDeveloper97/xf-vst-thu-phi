using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThuPhi.Model.Receive
{
    class Form
    {
        [JsonProperty("Ten")]
        public string Name { get; set; }

        [JsonProperty("NoiDung")]
        public string Content { get; set; }

        //[JsonProperty("Mau")]
        //public string Pattern { get; set; }

        //[JsonProperty("items")]
        //public List<Info> Items { get; set; }

        [JsonProperty("ThoiGian")]
        public DateTime Time { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    class Info
    {
        [JsonProperty("HoTen")]
        public string Name { get; set; }

        [JsonProperty("SoTK")]
        public string AccountNumber { get; set; }

        [JsonProperty("SoTT")]
        public string Order { get; set; }
    }
}
