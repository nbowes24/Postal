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

            //var test = GetStaff();




            //btnAdmin = FindViewById<Button>(Resource.Id.buttonAdmin);
            //textviewUser = FindViewById<TextView>(Resource.Id.textviewUser);

            //if (Intent.GetBooleanExtra("AdminFlag", false))
            //{
            //    btnAdmin.Visibility = ViewStates.Visible;
            //}

            //textviewUser.Text = ("Logged in as: " + Intent.GetStringExtra("FirstName"));

            //btnLogout = FindViewById<Button>(Resource.Id.buttonLogout);
            ////editPin = FindViewById<EditText>(Resource.Id.input_pin);

            //btnLogout.Click += delegate
            //{
            //    Intent intent = new Intent(this, typeof(MainActivity));
            //    intent.SetFlags(ActivityFlags.NewTask);
            //    StartActivity(intent);
            //    Finish();
            //};

        }

        private void AccountMenu()
        {
            Intent intent = new Intent(this, typeof(ModifyStaff));
            StartActivity(intent);
        }
    }
}