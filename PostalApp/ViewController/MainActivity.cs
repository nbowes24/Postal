using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using PostalApp.Model;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Java.Lang;
using Android.Content;
using Acr.UserDialogs;
using PostalApp.Data;

namespace PostalApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button btnLogin;
        private EditText editPin;
        private StaffService staffService = new StaffService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
          
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            btnLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            editPin = FindViewById<EditText>(Resource.Id.editTextStaffPin);

            btnLogin.Click += async delegate
            {
                if (editPin.Text != "")
                {
                    using (UserDialogs.Instance.Loading("Logging In..."))
                    {
                        var staff = await CheckLogin(Integer.ParseInt(editPin.Text));

                        if (staff == null)
                        {
                            UserDialogs.Instance.Alert("Incorrect Pin");
                        }
                        else
                        {
                            Intent intent = new Intent(this, typeof(Menu));
                            intent.PutExtra("StaffId", staff.Id);
                            intent.PutExtra("FirstName", staff.FirstName);
                            intent.PutExtra("AdminFlag", staff.AdminFlag);
                            intent.SetFlags(ActivityFlags.NewTask);
                            StartActivity(intent);
                            Finish();
                        }
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("Please enter a pin");
                }
            };
        }

        private async Task<Staff> CheckLogin(int pin)
        {
            Staff staff = null;
            HttpClient client = new HttpClient();
            string url = $"https://postalwebapi.azurewebsites.net/api/Staffs/{pin}";

            var staffT = await staffService.RefreshDataAsync();
            var uri = new Uri(url);
            var result = await client.GetAsync(url);
            var json = await result.Content.ReadAsStringAsync();
            staff = JsonConvert.DeserializeObject<Staff>(json);

            return staff;
        }
    }
}