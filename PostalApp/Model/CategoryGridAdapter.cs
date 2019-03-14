using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PostalApp.Model
{
    public class CategoryGridAdapter : BaseAdapter
    {
        public List<Category> categoryList;
        private Context sContext;
        public CategoryGridAdapter(Context context, List<Category> list)
        {
            categoryList = list;
            sContext = context;
        }

        public override int Count
        {
            get
            {
                return categoryList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            Button button = null;
            try
            {
                if (convertView == null)
                {
                    button = new Button(sContext);
                    button.LayoutParameters = new GridView.LayoutParams(200, 200);
                    button.SetPadding(8, 8, 8, 8);
                    button.SetTextColor(Color.White);
                    button.Text = categoryList[position].CategoryText;
                    button.Click += delegate
                    {
                        (sContext as ModifyMenu).GetMenu(categoryList[position].Id);

                    };
                }
                else
                {
                    button = (Button)convertView;
                }

            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally { }
            return button;
        }
    }
}
