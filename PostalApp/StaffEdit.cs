using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

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
    public class StaffEdit : Activity
    {
        private Button btnStaffModifySave;
        private EditText editTextStaffFirstName;
        private EditText editTextStaffLastName;
        private EditText editTextStaffPin;
        private int staffId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_staff_modify);
            
            btnStaffModifySave = FindViewById<Button>(Resource.Id.buttonStaffModifySave);
            editTextStaffFirstName = FindViewById<EditText>(Resource.Id.editTextStaffFirstName);
            editTextStaffLastName = FindViewById<EditText>(Resource.Id.editTextStaffLastName);
            editTextStaffPin = FindViewById<EditText>(Resource.Id.editTextStaffPin);

            staffId = Intent.GetIntExtra("Id", 0);
            editTextStaffFirstName.Text = Intent.GetStringExtra("FirstName");
            editTextStaffLastName.Text = Intent.GetStringExtra("LastName");
            editTextStaffPin.Text = Intent.GetIntExtra("Pin", 0).ToString();

            btnStaffModifySave.Click += delegate
            {
                EditAccount();
            };

        }

        private async void EditAccount()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string url = $"https://postalwebapi.azurewebsites.net/api/Staffs/{staffId}";
            var uri = new Uri(url);

            var staffTest = new Staff()
            {
                Id = staffId,
                FirstName = editTextStaffFirstName.Text,
                LastName = editTextStaffLastName.Text,
                Pin = Int32.Parse(editTextStaffPin.Text)
            };

            var json = JsonConvert.SerializeObject(staffTest);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await client.PutAsync(uri, content);
        }

    }
}