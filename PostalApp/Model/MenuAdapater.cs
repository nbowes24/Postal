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
    class MenuAdapter : BaseAdapter<Model.Menu>
    {
        public List<Model.Menu> menuList;
        private Context sContext;
        public MenuAdapter(Context context, List<Model.Menu> list)
        {
            menuList = list;
            sContext = context;
        }

        public override Model.Menu this[int position]
        {
            get
            {
                return menuList[position];
            }
        }

        public override int Count
        {
            get
            {
                return menuList.Count;
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
                    row = LayoutInflater.From(sContext).Inflate(Resource.Layout.list_view_menu_category, null);
                }
                TextView txtName = row.FindViewById<TextView>(Resource.Id.textviewMenuCategoryItem);
                txtName.Text = menuList[position].FoodDescription;
                
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