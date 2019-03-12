using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Acr.UserDialogs;
using Newtonsoft.Json.Linq;

namespace PostalApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class ModifyTable : Activity
    {
        private ListView listViewTableView;
        private Button btnAddTable;
        private List<Table> tableList = new List<Table>();
        private TableAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_table_modify);

            listViewTableView = FindViewById<ListView>(Resource.Id.listviewTable);
            btnAddTable = FindViewById<Button>(Resource.Id.buttonAddTable);

            GetTables();

            listViewTableView.ItemClick += TableListView_ItemClick;

            btnAddTable.Click += delegate
            {
                AddTable();
            };

        }

        private async void GetTables()
        {
            HttpClient client = new HttpClient();
            string url = $"https://postalwebapi.azurewebsites.net/api/TableNums";
            var uri = new Uri(url);
            var result = await client.GetAsync(url);
            var json = await result.Content.ReadAsStringAsync();
            if (IsValidJson(json))
            {
                tableList = JsonConvert.DeserializeObject<List<Table>>(json);

                adapter = new TableAdapter(this, tableList);
                listViewTableView.Adapter = adapter;
            }
        }

        private void TableListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var table = tableList[e.Position].Id;
            DeleteTable(e);
        }

        private async void AddTable()
        {
            var result = await UserDialogs.Instance.PromptAsync(new PromptConfig()
                .SetTitle("Enter Table Number")
                .SetInputMode(InputType.Number)
                .SetMaxLength(10));

            if (result.Ok)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                string url = $"https://postalwebapi.azurewebsites.net/api/TableNums";
                var uri = new Uri(url);

                var table = new Table()
                {
                    TableNumber = Int32.Parse(result.Text)
                };

                var json = JsonConvert.SerializeObject(table);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    GetTables();
                }
            }
        }

        private async void DeleteTable(AdapterView.ItemClickEventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
                .SetTitle($"Delete table: {tableList[e.Position].TableNumber}")
                .SetOkText("Delete")
                .SetCancelText("Cancel"));

            if (result)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                string url = $"https://postalwebapi.azurewebsites.net/api/TableNums/{tableList[e.Position].Id}";
                var uri = new Uri(url);

                response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    GetTables();
                }
            }
        }


        public static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}