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
    public class CategoriesEdit : Activity
    {
        private Button btnCategoryModifySave;
        private Button btnCategoryModifyDelete;
        private EditText editTextCategoryText;
        private EditText editTextCategoryOrder;
        private int categoryId;
        private CategoryService categoryService = new CategoryService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.admin_categories_modify);

            UserDialogs.Init(this);

            btnCategoryModifySave = FindViewById<Button>(Resource.Id.buttonCategoryModifySave);
            btnCategoryModifyDelete = FindViewById<Button>(Resource.Id.buttonCategoryModifyDelete);
            editTextCategoryText = FindViewById<EditText>(Resource.Id.editTextCategoryText);
            editTextCategoryOrder = FindViewById<EditText>(Resource.Id.editTextCategoryOrder);

            categoryId = Intent.GetIntExtra("Id", 0);
            editTextCategoryText.Text = Intent.GetStringExtra("CategoryText");
            editTextCategoryOrder.Text = Intent.GetIntExtra("DisplayOrder", 0).ToString();

            btnCategoryModifySave.Click += delegate
            {
                EditCategory();
            };

            btnCategoryModifyDelete.Click += delegate
            {
                DeleteCategory();
            };

        }

        private async void EditCategory()
        {
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = new HttpResponseMessage();
            //string url = $"https://postalwebapi.azurewebsites.net/api/Categories/{categoryId}";
            //var uri = new Uri(url);

            var category = new Category()
            {
                Id = categoryId,
                CategoryText = editTextCategoryText.Text,
                DisplayOrder = Int32.Parse(editTextCategoryOrder.Text),
            };

            await categoryService.SaveTableItemAsync(category, false);

            //var json = JsonConvert.SerializeObject(category);

            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            //response = await client.PutAsync(uri, content);

            Intent returnIntent = new Intent();

            SetResult(Result.Ok, returnIntent);

            Finish();
        }

        private async void DeleteCategory()
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
                .SetTitle($"Delete category: {editTextCategoryText.Text}")
                .SetOkText("Delete")
                .SetCancelText("Cancel"));

            if (result)
            {
                await categoryService.DeleteTableItemAsync(categoryId.ToString());

                Intent returnIntent = new Intent();

                SetResult(Result.Ok, returnIntent);

                Finish();
            }
        }

    }
}