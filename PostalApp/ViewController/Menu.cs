using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PostalApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class Menu : Activity
    {
        private Button btnAdmin;
        private Button btnStartTable;
        private Button btnTakeOrder;
        private Button buttonViewOrders;
        private Button btnCloseTable;
        private Button btnLogout;
        private TextView textviewUser;
        private bool AdminFlag;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.menu);
            
            btnAdmin = FindViewById<Button>(Resource.Id.buttonAdmin);
            btnStartTable = FindViewById<Button>(Resource.Id.buttonStartTable);
            btnTakeOrder = FindViewById<Button>(Resource.Id.buttonTakeOrder);
            buttonViewOrders = FindViewById<Button>(Resource.Id.buttonViewOrders);
            btnCloseTable = FindViewById<Button>(Resource.Id.buttonCloseTable);
            btnLogout = FindViewById<Button>(Resource.Id.buttonLogout);
            textviewUser = FindViewById<TextView>(Resource.Id.textviewUser);

            AdminFlag = Intent.GetBooleanExtra("AdminFlag", false);

            if (AdminFlag)
            {
                btnAdmin.Visibility = ViewStates.Visible;
            }

            textviewUser.Text = ("Logged in as: " + Intent.GetStringExtra("FirstName"));

            btnLogout.Click += delegate
            {
                Logout();
            };

            btnAdmin.Click += delegate
            {
                AdminMenu();
            };

            btnStartTable.Click += delegate
            {
                StartTable();
            };

            btnCloseTable.Click += delegate
            {
                CloseTable();
            };

            btnTakeOrder.Click += delegate
            {
                TakeOrder();
            };
            buttonViewOrders.Click += delegate
            {
                ViewOrders();
            };
        }

        private void ViewOrders()
        {
            Intent intent = new Intent(this, typeof(ViewOrder));
            intent.PutExtra("StaffId", Intent.GetIntExtra("StaffId", 1));
            StartActivity(intent);
        }

        private void TakeOrder()
        {
            Intent intent = new Intent(this, typeof(TakeOrderTable));
            intent.PutExtra("StaffId", Intent.GetIntExtra("StaffId", 1));
            StartActivity(intent);
        }

        private void StartTable()
        {
            Intent intent = new Intent(this, typeof(StartTable));
            intent.PutExtra("StaffId", Intent.GetIntExtra("StaffId",1));
            StartActivity(intent);
        }

        private void CloseTable()
        {
            Intent intent = new Intent(this, typeof(CloseTable));
            intent.PutExtra("StaffId", Intent.GetIntExtra("StaffId", 1));
            intent.PutExtra("AdminFlag", AdminFlag);
            StartActivity(intent);
        }

        private void Logout()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
            Finish();
        }


        private void AdminMenu()
        {
            Intent intent = new Intent(this, typeof(AdminMenu));
            StartActivity(intent);
        }
    }
}