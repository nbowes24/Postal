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
    class CategoryAdapter : BaseAdapter<Category>
    {
        public List<Category> categoryList;
        private Context sContext;
        public CategoryAdapter(Context context, List<Category> list)
        {
            categoryList = list;
            sContext = context;
        }

        public override Category this[int position]
        {
            get
            {
                return categoryList[position];
            }
        }

        public override int Count
        {
            get
            {
                return categoryList.Count;
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
                    row = LayoutInflater.From(sContext).Inflate(Resource.Layout.list_view_category, null);
                }
                TextView txtName = row.FindViewById<TextView>(Resource.Id.textviewCategories);
                txtName.Text = categoryList[position].CategoryText;
                
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