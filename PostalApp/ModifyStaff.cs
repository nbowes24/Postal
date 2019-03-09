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
            EditAccount(e);
        }

        private void AddAccount()
        {
            Intent intent = new Intent(this, typeof(StaffAdd));
            StartActivity(intent);
        }

        private void EditAccount(AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(StaffEdit));
            intent.PutExtra("Id", staffList[e.Position].Id);
            intent.PutExtra("FirstName", staffList[e.Position].FirstName);
            intent.PutExtra("LastName", staffList[e.Position].LastName);
            intent.PutExtra("Pin", staffList[e.Position].Pin);
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
        }
    }
}