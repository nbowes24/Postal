using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PostalApp.Model
{
    public class TableOrder
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "ordertime")]
        public DateTime OrderTime { get; set; }

        [JsonProperty(PropertyName ="complete")]
        public bool Complete { get; set; }

        [JsonProperty(PropertyName = "staffid")]
        public int StaffId { get; set; }

        [JsonProperty(PropertyName = "tablenumid")]
        public int TableNumId { get; set; }
    }
}