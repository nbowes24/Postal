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
    public class MenuService : IMenuService
    {
        HttpClient client;

        public List<Model.Menu> Items { get; private set; }

        public MenuService()
        {
            //var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
            //var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public async Task<List<Model.Menu>> RefreshDataAsync()
        {
            Items = new List<Model.Menu>();

            //RestUrl = https://postalwebapi.azurewebsites.net/api/TableNums
            var uri = new Uri("https://postalwebapi.azurewebsites.net/api/Menus");

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<Model.Menu>>(content);
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Items;
        }

        public async Task SaveTableItemAsync(Model.Menu item, bool isNewItem = false)
        {
            // RestUrl = http://developer.xamarin.com:8081/api/todoitems
            

            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    var uri = new Uri("https://postalwebapi.azurewebsites.net/api/Menus");
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    var uri = new Uri($"https://postalwebapi.azurewebsites.net/api/Menus/{item.Id}");
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
            var uri = new Uri($"https://postalwebapi.azurewebsites.net/api/Menus/{id}");

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