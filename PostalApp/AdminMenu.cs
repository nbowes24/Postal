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

namespace PostalApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class AdminMenu : Activity
    {
        private Button btnAccounts;
        private Button btnBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_menu);

            btnAccounts = FindViewById<Button>(Resource.Id.buttonAccounts);

            btnAccounts.Click += delegate
            {
                AccountMenu();
            };

        }

        private void AccountMenu()
        {
            Intent intent = new Intent(this, typeof(ModifyStaff));
            StartActivity(intent);
        }
    }
}