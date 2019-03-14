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
        private GridView gridViewMenuModify;
        private Button btnAddCategory;
        private List<Category> categoryList = new List<Category>();
        private List<Model.Menu> menuList = new List<Model.Menu>();
        private MenuAdapter adapter;
        private CategoryService categoryService = new CategoryService();
        private LinearLayout linearLayoutAdminModifyMenu;
        private MenuService menuService = new MenuService();
        private CategoryGridAdapter categoryGridAdapter;
        private ListView listviewMenuCategory;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            
            SetContentView(Resource.Layout.admin_menu_modify);

            //linearLayoutAdminModifyMenu = FindViewById<LinearLayout>(Resource.Id.linearLayout2);
            gridViewMenuModify = FindViewById<GridView>(Resource.Id.gridViewMenuModify);
            listviewMenuCategory = FindViewById<ListView>(Resource.Id.listviewMenuCategory);



            GetCategories();

            gridViewMenuModify.ItemClick += GridViewMenuModify_ItemClick;



            //listviewCategory = FindViewById<ListView>(Resource.Id.listviewCategory);
            //btnAddCategory = FindViewById<Button>(Resource.Id.buttonAddCategory);

            //GetCategories();

            //listviewCategory.ItemClick += TableListView_ItemClick;

            //btnAddCategory.Click += delegate
            //{
            //    AddCategory();
            //};

        }

        private void GridViewMenuModify_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GetMenu(categoryList[e.Position].Id);
        }

        private void TableListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            EditCategory(e);
        }

        private async void GetCategories()
        {
            categoryList = await categoryService.RefreshDataAsync();

            categoryList = categoryList.OrderBy(x => x.DisplayOrder).ToList();

            categoryGridAdapter = new CategoryGridAdapter(this, categoryList);
            gridViewMenuModify.Adapter = categoryGridAdapter;

            //foreach (Category category in categoryList)
            //{
            //    var button = new Button(this);
            //    button.Text = category.CategoryText;
            //    button.Id = category.Id;
            //    button.Click += (sender, e) => GetMenu(category.Id);
            //    linearLayoutAdminModifyMenu.AddView(button);
            //}
        }

        public async void GetMenu(int Id)
        {
            menuList = await menuService.RefreshDataAsync();

            menuList = menuList.Where(x => x.CategoryId == Id).ToList();

            adapter = new MenuAdapter(this, menuList);
            listviewMenuCategory.Adapter = adapter;
        }

        private void EditCategory(AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(CategoriesEdit));
            intent.PutExtra("Id", categoryList[e.Position].Id);
            intent.PutExtra("CategoryText", categoryList[e.Position].CategoryText);
            intent.PutExtra("DisplayOrder", categoryList[e.Position].DisplayOrder);
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