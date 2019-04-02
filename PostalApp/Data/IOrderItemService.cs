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
    public interface IOrderItemService
    {
        Task<List<OrderItem>> RefreshDataAsync();

        Task SaveTableItemAsync(OrderItem item, bool isNewItem);

        Task DeleteTableItemAsync(string id);
    }
}