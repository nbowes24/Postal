using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PostalApp.Model
{
    public class Menu
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "fooddescription")]
        public string FoodDescription { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "available")]
        public bool Available { get; set; }

        [JsonProperty(PropertyName = "CategoryId")]
        public int CategoryId { get; set; }
    }
}