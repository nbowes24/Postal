using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace PostalApp.Model
{
    public class Table
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "staffid")]
        public int? StaffId { get; set; }

        [JsonProperty(PropertyName = "tablenumber")]
        public int TableNumber { get; set; }
    }
}