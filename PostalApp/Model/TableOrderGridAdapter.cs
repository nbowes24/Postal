using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PostalApp.Model
{
    public class TableOrderGridAdapter : BaseAdapter
    {
        public List<TableOrder> tableOrderList;
        private Context sContext;
        public TableOrderGridAdapter(Context context, List<TableOrder> list)
        {
            tableOrderList = list;
            sContext = context;
        }

        public override int Count
        {
            get
            {
                return tableOrderList.Count;
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
                    GradientDrawable drawable = new GradientDrawable();
                    button = new Button(sContext);
                    button.LayoutParameters = new GridView.LayoutParams(175, 125);
                    button.SetPadding(8, 8, 8, 8);
                    button.SetTextColor(Color.White);
                    
                    drawable.SetShape(ShapeType.Rectangle);
                    drawable.SetStroke(2, Color.White);
                    drawable.SetColor(Color.Rgb(69, 135, 244));

                    button.SetBackgroundDrawable(drawable);
                    button.Text = $"Order#{tableOrderList[position].Id.ToString()} {System.Environment.NewLine} {tableOrderList[position].OrderTime.ToShortTimeString()}";
                    button.Click += delegate
                    {
                        (sContext as ViewOrder).ViewOrderDetails(tableOrderList[position]);

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
