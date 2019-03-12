using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Acr.UserDialogs;
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
        private Button btnStaffModifyDelete;
        private EditText editTextStaffFirstName;
        private EditText editTextStaffLastName;
        private EditText editTextStaffPin;
        private int staffId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_staff_modify);

            UserDialogs.Init(this);

            btnStaffModifySave = FindViewById<Button>(Resource.Id.buttonStaffModifySave);
            btnStaffModifyDelete = FindViewById<Button>(Resource.Id.buttonStaffModifyDelete);
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

            btnStaffModifyDelete.Click += delegate
            {
                DeleteAccount();
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

            Intent returnIntent = new Intent();

            if (response.IsSuccessStatusCode)
            {
                SetResult(Result.Ok, returnIntent);
            }

            Finish();
        }

        private async void DeleteAccount()
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
                .SetTitle($"Delete table: {editTextStaffFirstName.Text}")
                .SetOkText("Delete")
                .SetCancelText("Cancel"));

            if (result)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                string url = $"https://postalwebapi.azurewebsites.net/api/Staffs/{staffId}";
                var uri = new Uri(url);

                response = await client.DeleteAsync(uri);

                Intent returnIntent = new Intent();

                if (response.IsSuccessStatusCode)
                {
                    SetResult(Result.Ok, returnIntent);
                }

                Finish();
            }
        }

        private void ShowConfirmDialog()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetTitle("Confirm Delete");
            builder.SetMessage("Are you sure you want to delete: " + editTextStaffFirstName.Text);
            builder.SetPositiveButton("YES", delegate
            {
                DeleteAccount();
            });
            builder.SetNegativeButton("NO", delegate
            {

            });

            builder.Show();
        }

    }
}