using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
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
    public class MenuEdit : Activity
    {
        private Button buttonMenuEditSave;
        private EditText editTextEditMenuDescription;
        private EditText editTextEditMenuPrice;
        private CheckBox checkBoxMenuAvailabilityEdit;
        private Spinner spinnerMenuCategoriesEdit;
        private List<Category> categoryList = new List<Category>();
        private CategoryService categoryService = new CategoryService();
        private CategoryAdapter categoryAdapter;
        private MenuService menuService = new MenuService();
        private int menuId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_menu_edit);

            buttonMenuEditSave = FindViewById<Button>(Resource.Id.buttonMenuEditSave);
            spinnerMenuCategoriesEdit = FindViewById<Spinner>(Resource.Id.spinnerMenuCategoriesEdit);
            editTextEditMenuDescription = FindViewById<EditText>(Resource.Id.editTextEditMenuDescription);
            editTextEditMenuPrice = FindViewById<EditText>(Resource.Id.editTextEditMenuPrice);
            checkBoxMenuAvailabilityEdit = FindViewById<CheckBox>(Resource.Id.checkBoxMenuAvailabilityEdit);

            GetCategories();

            menuId = Intent.GetIntExtra("Id", 0);
            editTextEditMenuDescription.Text = Intent.GetStringExtra("FoodDescription");
            editTextEditMenuPrice.Text = Intent.GetStringExtra("Price");
            editTextEditMenuDescription.Text = Intent.GetStringExtra("FoodDescription");
            checkBoxMenuAvailabilityEdit.Checked = Intent.GetBooleanExtra("Available",false);
            
            buttonMenuEditSave.Click += delegate
            {
                EditMenuItem();
            };
        }

        private async void GetCategories()
        {
            categoryList = await categoryService.RefreshDataAsync();

            categoryList = categoryList.OrderBy(x => x.DisplayOrder).ToList();
            categoryAdapter = new CategoryAdapter(this, categoryList);

            spinnerMenuCategoriesEdit.Adapter = categoryAdapter;

            SetCategory(Intent.GetIntExtra("CategoryId", 1));
        }

        private void SetCategory(int id)
        {
            int position = categoryList.FindIndex(x => x.Id == id);
            spinnerMenuCategoriesEdit.SetSelection(position);
        }

        private async void EditMenuItem()
        {
            var menu = new Model.Menu()
            {
                Id = menuId,
                FoodDescription = editTextEditMenuDescription.Text,
                Price = Decimal.Parse(editTextEditMenuPrice.Text),
                Available = checkBoxMenuAvailabilityEdit.Checked,
                CategoryId = categoryList[spinnerMenuCategoriesEdit.SelectedItemPosition].Id
            };

            await menuService.SaveTableItemAsync(menu, false);

            Intent returnIntent = new Intent();
            SetResult(Result.Ok, returnIntent);

            Finish();
        }
    }
}