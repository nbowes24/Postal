using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PostalApp.Data;
using PostalApp.Model;

namespace PostalApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class MenuAdd : Activity
    {
        private Button buttonMenuAdd;
        private EditText editTextAddMenuDescription;
        private EditText editTextAddMenuPrice;
        private CheckBox checkBoxMenuAvailability;
        private Spinner spinnerMenuCategoriesAdd;
        private List<Category> categoryList = new List<Category>();
        private CategoryService categoryService = new CategoryService();
        private CategoryAdapter categoryAdapter;
        private MenuService menuService = new MenuService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_menu_add);

            buttonMenuAdd = FindViewById<Button>(Resource.Id.buttonMenuAdd);
            spinnerMenuCategoriesAdd = FindViewById<Spinner>(Resource.Id.spinnerMenuCategoriesAdd);
            editTextAddMenuDescription = FindViewById<EditText>(Resource.Id.editTextAddMenuDescription);
            editTextAddMenuPrice = FindViewById<EditText>(Resource.Id.editTextAddMenuPrice);
            checkBoxMenuAvailability = FindViewById<CheckBox>(Resource.Id.checkBoxMenuAvailability);

            GetCategories();

            buttonMenuAdd.Click += delegate
            {
                AddMenuItem();
            };
        }

        private async void GetCategories()
        {
            categoryList = await categoryService.RefreshDataAsync();

            categoryList = categoryList.OrderBy(x => x.DisplayOrder).ToList();
            categoryAdapter = new CategoryAdapter(this, categoryList);

            spinnerMenuCategoriesAdd.Adapter = categoryAdapter;
        }

        private async void AddMenuItem()
        {
            var menu = new Model.Menu()
            {
                FoodDescription = editTextAddMenuDescription.Text,
                Price = Decimal.Parse(editTextAddMenuPrice.Text),
                Available = checkBoxMenuAvailability.Checked,
                CategoryId = categoryList[spinnerMenuCategoriesAdd.SelectedItemPosition].Id
            };

            await menuService.SaveTableItemAsync(menu, true);

            Intent returnIntent = new Intent();
            SetResult(Result.Ok);

            Finish();
        }
    }
}