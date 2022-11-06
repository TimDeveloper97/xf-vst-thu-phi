using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThuPhi.Model.Send
{
    public class FormCode
    {
        [JsonProperty("Ten")]
        public string Name { get; set; }

        [JsonProperty("NoiDung")]
        public string Content { get; set; }

        [JsonProperty("Mau")]
        public string Pattern { get; set; }
    }
}
