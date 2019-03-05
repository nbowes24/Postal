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

namespace PostalApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class ModifyStaff : Activity
    {
        private ListView listViewStaffView;
        private Button btnAddAccount;
        private List<Staff> staffList = new List<Staff>();
        private StaffAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_staff_menu);

            listViewStaffView = FindViewById<ListView>(Resource.Id.listviewStaff);
            btnAddAccount = FindViewById<Button>(Resource.Id.buttonAddAccount);

            GetStaff();

            listViewStaffView.ItemClick += StaffListView_ItemClick;

            btnAddAccount.Click += delegate
            {
                AddAccount();
            };

            //btnLogout.Click += delegate
            //{
            //    Intent intent = new Intent(this, typeof(MainActivity));
            //    intent.SetFlags(ActivityFlags.NewTask);
            //    StartActivity(intent);
            //    Finish();
            //};

        }

        private async void GetStaff()
        {
            HttpClient client = new HttpClient();
            string url = $"https://postalwebapi.azurewebsites.net/api/Staffs";
            var uri = new Uri(url);
            var result = await client.GetAsync(url);
            var json = await result.Content.ReadAsStringAsync();
            staffList = JsonConvert.DeserializeObject<List<Staff>>(json);

            adapter = new StaffAdapter(this, staffList);
            listViewStaffView.Adapter = adapter;
        }

        private void StaffListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = staffList[e.Position].Id;
            Android.Widget.Toast.MakeText(this, select.ToString(), Android.Widget.ToastLength.Long).Show();
            //Toast.MakeText(this, "No user found for that pin", ToastLength.Long).Show();
        }

        private async void AddAccount()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string url = $"https://postalwebapi.azurewebsites.net/api/Staffs";
            var uri = new Uri(url);

            var staffTest = new Staff()
            {
                FirstName = "Post",
                LastName = "Test",
                Pin = 1234,
                AdminFlag = false
            };

            var json = JsonConvert.SerializeObject(staffTest);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await client.PostAsync(uri, content);




            //var result = await client.GetAsync(url);
            //var json = await result.Content.ReadAsStringAsync();
            //staffList = JsonConvert.DeserializeObject<List<Staff>>(json);




        }
    }
}