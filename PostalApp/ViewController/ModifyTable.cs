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
    public class ModifyTable : Activity
    {
        private ListView listViewTableView;
        private Button btnAddTable;
        private List<Table> tableList = new List<Table>();
        private TableAdapter adapter;
        private TableService tableService = new TableService();

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
            tableList = await tableService.RefreshDataAsync();

            adapter = new TableAdapter(this, tableList);
            listViewTableView.Adapter = adapter;
        }

        private void TableListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            DeleteTable(e);
        }

        private async void AddTable()
        {
            var result = await UserDialogs.Instance.PromptAsync(new PromptConfig()
                .SetTitle("Enter Table Number")
                .SetInputMode(InputType.Number)
                .SetMaxLength(10));

            if (result.Ok && result.Text != "")
            {
                var table = new Table()
                {
                    TableNumber = Int32.Parse(result.Text)
                };

                if (CheckTableExists(table))
                {
                    UserDialogs.Instance.Alert($"Table number {table.TableNumber} already exists");
                }
                else
                {
                    await tableService.SaveTableItemAsync(table, true);

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
                await tableService.DeleteTableItemAsync(tableList[e.Position].Id.ToString());
                
                GetTables();
            }
        }

        private bool CheckTableExists(Table table)
        {
            var tableFilter = tableList.Where(t => t.TableNumber == table.TableNumber).FirstOrDefault();

            if (tableFilter == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}