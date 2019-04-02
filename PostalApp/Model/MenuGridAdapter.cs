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
    public class MenuGridAdapter : BaseAdapter
    {
        public List<Menu> menuList;
        private Context sContext;
        public MenuGridAdapter(Context context, List<Menu> list)
        {
            menuList = list;
            sContext = context;
        }

        public override int Count
        {
            get
            {
                return menuList.Count;
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
                    button.Text = menuList[position].FoodDescription;
                    var name = sContext.Class.Name;
                    if (name.Contains("ModifyMenu"))
                    {
                        button.Click += delegate
                        {
                            (sContext as ModifyMenu).EditMenu(menuList[position]);

                        };
                    }
                    else if (name.Contains("TakeOrder"))
                    {
                        button.Click += delegate
                        {
                            (sContext as TakeOrder).AddToOrder(menuList[position]);

                        };
                    }

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
