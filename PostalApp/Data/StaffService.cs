using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PostalApp.Model;

namespace PostalApp.Data
{
    public class StaffService : IStaffService
    {
        HttpClient client;

        public List<Staff> Items { get; private set; }

        public StaffService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<List<Staff>> RefreshDataAsync()
        {
            Items = new List<Staff>();
            
            var uri = new Uri("https://postalwebapi.azurewebsites.net/api/Staffs");

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<Staff>>(content);
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Items;
        }

        public async Task SaveTableItemAsync(Staff item, bool isNewItem = false)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    var uri = new Uri("https://postalwebapi.azurewebsites.net/api/Staffs");
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    var uri = new Uri($"https://postalwebapi.azurewebsites.net/api/Staffs/{item.Id}");
                    response = await client.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    //Debug.WriteLine(@"				TodoItem successfully saved.");
                }

            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }

        public async Task DeleteTableItemAsync(string id)
        {
            //RestUrl = http://developer.xamarin.com:8081/api/todoitems/{0}
            var uri = new Uri($"https://postalwebapi.azurewebsites.net/api/Staffs/{id}");

            try
            {
                var response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    //Debug.WriteLine(@"				TodoItem successfully deleted.");
                }

            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }
    }
}