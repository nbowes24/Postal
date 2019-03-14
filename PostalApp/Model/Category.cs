using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PostalApp.Model
{
    public class Category
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "categorytext")]
        public string CategoryText { get; set; }

        [JsonProperty(PropertyName = "displayorder")]
        public int DisplayOrder { get; set; }
    }
}