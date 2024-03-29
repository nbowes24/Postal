﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PostalApp.Model
{
    public class OrderItem
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "menuid")]
        public int MenuId { get; set; }

        [JsonProperty(PropertyName = "tableorderid")]
        public int TableOrderId { get; set; }
    }
}