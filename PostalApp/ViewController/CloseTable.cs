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
    public class CloseTable : Activity
    {
        private ListView listviewCloseTable;
        private List<Table> tableList = new List<Table>();
        private TableAdapter adapter;
        private TableService tableService = new TableService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.close_table);

            listviewCloseTable = FindViewById<ListView>(Resource.Id.listviewCloseTable);

            GetTables();

            listviewCloseTable.ItemClick += TableListView_ItemClick;

        }

        private async void GetTables()
        {
            tableList = await tableService.RefreshDataAsync();

            if(Intent.GetBooleanExtra("AdminFlag", false))
            {
                tableList = (from Table t in tableList
                             where t.StaffId != null
                             select t).ToList();
            }
            else
            {
                tableList = (from Table t in tableList
                             where t.StaffId == Intent.GetIntExtra("StaffId", 1)
                             select t).ToList();
            }

            
            
            adapter = new TableAdapter(this, tableList);
            listviewCloseTable.Adapter = adapter;
        }

        private void TableListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var table = tableList[e.Position].Id;
            AssignTable(e);
        }

        private async void AssignTable(AdapterView.ItemClickEventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
                .SetTitle($"Close Table Number {tableList[e.Position].TableNumber}?")
                .SetOkText("Okay")
                .SetCancelText("Cancel"));

            if (result)
            {

                var table = new Table()
                {
                    Id = tableList[e.Position].Id,
                    TableNumber = tableList[e.Position].TableNumber,
                    StaffId = null
                };

                await tableService.SaveTableItemAsync(table, false);

                GetTables();
            }
        }
        
    }
}