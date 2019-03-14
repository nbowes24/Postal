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
using PostalApp.Data;

namespace PostalApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class StartTable : Activity
    {
        private ListView listviewStartTable;
        private List<Table> tableList = new List<Table>();
        private TableAdapter adapter;
        private TableService tableService = new TableService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.start_table);

            listviewStartTable = FindViewById<ListView>(Resource.Id.listviewStartTable);

            GetTables();

            listviewStartTable.ItemClick += TableListView_ItemClick;

        }

        private async void GetTables()
        {
            tableList = await tableService.RefreshDataAsync();

            tableList = (from Table t in tableList
                                    where t.StaffId == null
                                    select t).ToList();
            
            adapter = new TableAdapter(this, tableList);
            listviewStartTable.Adapter = adapter;
        }

        private void TableListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var table = tableList[e.Position].Id;
            AssignTable(e);
        }

        private async void AssignTable(AdapterView.ItemClickEventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
                .SetTitle($"Start Table Number {tableList[e.Position].TableNumber}?")
                .SetOkText("Okay")
                .SetCancelText("Cancel"));

            if (result)
            {

                var table = new Table()
                {
                    Id = tableList[e.Position].Id,
                    TableNumber = tableList[e.Position].TableNumber,
                    StaffId = Intent.GetIntExtra("StaffId", 1)
                };

                await tableService.SaveTableItemAsync(table, false);

                GetTables();
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