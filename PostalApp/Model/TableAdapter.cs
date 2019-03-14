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
    class TableAdapter : BaseAdapter<Table>
    {
        public List<Table> tableList;
        private Context sContext;
        public TableAdapter(Context context, List<Table> list)
        {
            tableList = list;
            sContext = context;
        }

        public override Table this[int position]
        {
            get
            {
                return tableList[position];
            }
        }

        public override int Count
        {
            get
            {
                return tableList.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            Android.Views.View row = convertView;
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(sContext).Inflate(Resource.Layout.list_view_table, null);
                }
                TextView txtName = row.FindViewById<TextView>(Resource.Id.textviewTable);
                txtName.Text = tableList[position].TableNumber.ToString();
                
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