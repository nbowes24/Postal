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

namespace PostalApp.Model
{
    class StaffAdapter : BaseAdapter<Staff>
    {
        public List<Staff> staffList;
        private Context sContext;
        public StaffAdapter(Context context, List<Staff> list)
        {
            staffList = list;
            sContext = context;
        }

        public override Staff this[int position]
        {
            get
            {
                return staffList[position];
            }
        }

        public override int Count
        {
            get
            {
                return staffList.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(sContext).Inflate(Resource.Layout.list_view_staff, null);
                }
                TextView txtName = row.FindViewById<TextView>(Resource.Id.textviewStaffName);
                txtName.Text = staffList[position].FirstName;
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally { }
            return row;
        }
    }
}