using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    public class CategoriesAdd : Activity
    {
        private Button buttonCategoryAdd;
        private EditText editTextCategoryText;
        private EditText editTextCategoryOrder;
        private CategoryService categoryService = new CategoryService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_category_add);

            buttonCategoryAdd = FindViewById<Button>(Resource.Id.buttonCategoryAdd);
            editTextCategoryText = FindViewById<EditText>(Resource.Id.editTextAddCategoryText);
            editTextCategoryOrder = FindViewById<EditText>(Resource.Id.editTextAddCategoryOrder);

            buttonCategoryAdd.Click += delegate
            {
                AddCategory();
            };
        }

        private async void AddCategory()
        {
            if (ValidateInputs())
            {
                var category = new Category()
                {
                    CategoryText = editTextCategoryText.Text,
                    DisplayOrder = Int32.Parse(editTextCategoryOrder.Text)
                };

                await categoryService.SaveTableItemAsync(category, true);

                Intent returnIntent = new Intent();
                SetResult(Result.Ok, returnIntent);

                Finish();
            }
            else
            {
                UserDialogs.Instance.Alert("Please enter the category details");
            }

        }

        private bool ValidateInputs()
        {
            if(editTextCategoryText.Text == "" || editTextCategoryOrder.Text == "")
            {
                return false;
            }
            return true;
        }
    }
}