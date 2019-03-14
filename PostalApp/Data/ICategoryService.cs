using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PostalApp.Model;

namespace PostalApp.Data
{
    public interface ICategoryService
    {
        Task<List<Category>> RefreshDataAsync();

        Task SaveTableItemAsync(Category item, bool isNewItem);

        Task DeleteTableItemAsync(string id);
    }
}