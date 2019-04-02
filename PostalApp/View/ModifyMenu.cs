using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PostalApp.Data;
using PostalApp.Model;

namespace PostalApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class ModifyMenu : Activity
    {
        private List<Category> categoryList = new List<Category>();
        private List<Model.Menu> menuList = new List<Model.Menu>();
        private MenuGridAdapter menuGridAdapter;
        private CategoryService categoryService = new CategoryService();
        private MenuService menuService = new MenuService();
        private CategoryAdapter categoryAdapter;
        private GridView gridViewMenuItems;
        private Button btnAddMenuItem;
        private Spinner spinnerMenuCategories;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            
            SetContentView(Resource.Layout.admin_menu_modify);
            gridViewMenuItems = FindViewById<GridView>(Resource.Id.gridViewMenuItems);
            btnAddMenuItem = FindViewById<Button>(Resource.Id.buttonAddMenuItem);
            spinnerMenuCategories = FindViewById<Spinner>(Resource.Id.spinnerMenuCategories);



            GetCategories();
            

            spinnerMenuCategories.ItemSelected += SpinnerMenuCategories_ItemSelected;

            btnAddMenuItem.Click += delegate
            {
                AddMenuItemActivity();
            };

        }

        private void SpinnerMenuCategories_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            GetMenu(categoryList[e.Position].Id);
        }

        private void AddMenuItemActivity()
        {
            Intent intent = new Intent(this, typeof(MenuAdd));
            StartActivityForResult(intent, 0);
        }

        private async void GetCategories()
        {
            categoryList = await categoryService.RefreshDataAsync();

            categoryList = categoryList.OrderBy(x => x.DisplayOrder).ToList();



            categoryAdapter = new CategoryAdapter(this, categoryList);
            spinnerMenuCategories.Adapter = categoryAdapter;
        }

        public async void GetMenu(int Id)
        {
            menuList = await menuService.RefreshDataAsync();

            menuList = menuList.Where(x => x.CategoryId == Id).ToList();

            menuGridAdapter = new MenuGridAdapter(this, menuList);
            gridViewMenuItems.Adapter = menuGridAdapter;
            
        }

        public void EditMenu(Model.Menu menu)
        {
            Intent intent = new Intent(this, typeof(MenuEdit));
            intent.PutExtra("Id", menu.Id);
            intent.PutExtra("FoodDescription", menu.FoodDescription);
            intent.PutExtra("Price", string.Format("{0:N2}", menu.Price));
            intent.PutExtra("Available", menu.Available);
            intent.PutExtra("CategoryId", menu.CategoryId);
            StartActivityForResult(intent, 0);
        }

        private void AddCategory()
        {
            Intent intent = new Intent(this, typeof(CategoriesAdd));
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 0)
            {
                if (resultCode == Result.Ok)
                {
                    GetCategories();
                }
            }
        }
    }
}