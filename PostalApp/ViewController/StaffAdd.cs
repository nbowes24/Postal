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
using PostalApp.Data;
using PostalApp.Model;

namespace PostalApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class StaffAdd : Activity
    {
        private Button buttonStaffAdd;
        private EditText editTextAddStaffFirstName;
        private EditText editTextAddStaffLastName;
        private EditText editTextAddStaffPin;
        private CheckBox checkBoxAddStaffAdmin;
        private StaffService staffService = new StaffService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_staff_add);

            buttonStaffAdd = FindViewById<Button>(Resource.Id.buttonStaffAdd);
            editTextAddStaffFirstName = FindViewById<EditText>(Resource.Id.editTextAddStaffFirstName);
            editTextAddStaffLastName = FindViewById<EditText>(Resource.Id.editTextAddStaffLastName);
            editTextAddStaffPin = FindViewById<EditText>(Resource.Id.editTextAddStaffPin);
            checkBoxAddStaffAdmin = FindViewById<CheckBox>(Resource.Id.checkBoxAddStaffAdmin);

            buttonStaffAdd.Click += delegate
            {
                AddAccount();
            };
        }

        private async void AddAccount()
        {
            var staffList = await staffService.RefreshDataAsync();

            var staffPinCheck = staffList.Where(s => s.Pin == Int32.Parse(editTextAddStaffPin.Text)).FirstOrDefault();

            if(ValidateInputs())
            {
                if (staffPinCheck != null)
                {
                    await UserDialogs.Instance.AlertAsync("A user already exists with that pin");
                }
                else
                {
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = new HttpResponseMessage();
                    string url = $"https://postalwebapi.azurewebsites.net/api/Staffs";
                    var uri = new Uri(url);

                    var staff = new Staff()
                    {
                        FirstName = editTextAddStaffFirstName.Text,
                        LastName = editTextAddStaffLastName.Text,
                        Pin = Int32.Parse(editTextAddStaffPin.Text),
                        AdminFlag = checkBoxAddStaffAdmin.Checked
                    };

                    var json = JsonConvert.SerializeObject(staff);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    response = await client.PostAsync(uri, content);

                    Intent returnIntent = new Intent();

                    if (response.IsSuccessStatusCode)
                    {
                        SetResult(Result.Ok, returnIntent);
                    }

                    Finish();
                }
            }
            else
            {
                UserDialogs.Instance.Alert("Please enter the staff details");
            }
        }

        private bool ValidateInputs()
        {
            if (editTextAddStaffFirstName.Text == "" || editTextAddStaffLastName.Text == "" || editTextAddStaffPin.Text == "")
            {
                return false;
            }
            return true;
        }
    }
}