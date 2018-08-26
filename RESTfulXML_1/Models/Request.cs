using Newtonsoft.Json;
using System;

namespace RESTfulXML_1.Models
{
    public class Request
    {
        [JsonProperty(PropertyName = "Ix")]
        public int Index { get; set; }

        public string Name { get; set; }

        public int? Visits { get; set; }

        public DateTime Date { get; set; }
    }
}