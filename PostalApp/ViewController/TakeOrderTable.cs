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
    public class TakeOrderTable : Activity
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
            SetContentView(Resource.Layout.table_take_order);

            listviewStartTable = FindViewById<ListView>(Resource.Id.listviewStartTable);

            GetTables();

            listviewStartTable.ItemClick += TableListView_ItemClick;

        }

        private async void GetTables()
        {
            tableList = await tableService.RefreshDataAsync();

            tableList = (from Table t in tableList
                                    where t.StaffId != null
                                    select t).ToList();
            
            adapter = new TableAdapter(this, tableList);
            listviewStartTable.Adapter = adapter;
        }

        private void TableListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var table = tableList[e.Position].Id;
            OrderTable(e);
        }

        private async void OrderTable(AdapterView.ItemClickEventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
                .SetTitle($"Take Order for Table: {tableList[e.Position].TableNumber}?")
                .SetOkText("Okay")
                .SetCancelText("Cancel"));

            if (result)
            {
                Intent intent = new Intent(this, typeof(TakeOrder));
                intent.PutExtra("TableId", tableList[e.Position].Id);
                intent.PutExtra("StaffId", Intent.GetIntExtra("StaffId", 1));
                StartActivity(intent);
                Finish();
            }
        }
    }
}