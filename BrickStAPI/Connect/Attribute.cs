using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BrickStAPI.Connect
{
    public class Attribute
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dataType")]
        public int DataType { get; set; }

        [JsonProperty("singleValued")]
        public bool SingleValued { get; set; }

        [JsonProperty("boundedValues")]
        public string[] BoundedValues { get; set; }
    }
}
