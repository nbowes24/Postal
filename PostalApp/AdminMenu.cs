﻿using System;
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

namespace PostalApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class AdminMenu : Activity
    {
        private Button btnAccounts;
        private Button btnTables;
        private Button btnBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_menu);

            btnAccounts = FindViewById<Button>(Resource.Id.buttonAccounts);
            btnTables = FindViewById<Button>(Resource.Id.buttonTables);

            btnAccounts.Click += delegate
            {
                AccountMenu();
            };

            btnTables.Click += delegate
            {
                TableMenu();
            };

        }

        private void AccountMenu()
        {
            Intent intent = new Intent(this, typeof(ModifyStaff));
            StartActivity(intent);
        }

        private void TableMenu()
        {
            Intent intent = new Intent(this, typeof(ModifyTable));
            StartActivity(intent);
        }
    }
}