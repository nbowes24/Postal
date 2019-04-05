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
    public class ViewOrder : Activity
    {
        private List<TableOrder> tableOrderList = new List<TableOrder>();
        private List<Table> tableList = new List<Table>();
        private List<OrderItem> orderItemList = new List<OrderItem>();
        private List<Model.Menu> menuList = new List<Model.Menu>();
        private TableOrderGridAdapter tablePendingOrderGridAdapter;
        private TableOrderGridAdapter tableCompleteOrderGridAdapter;
        private GridView gridViewPendingTableOrders;
        private GridView gridViewCompleteTableOrders;
        private TextView textViewTotal;
        private Button btnSubmitOrder;
        private OrderItemService orderItemService = new OrderItemService();
        private TableOrderService tableOrderService = new TableOrderService();
        private TableService tableService = new TableService();
        private MenuService menuService = new MenuService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            
            SetContentView(Resource.Layout.view_orders);

            gridViewPendingTableOrders = FindViewById<GridView>(Resource.Id.gridViewPendingTableOrders);
            gridViewCompleteTableOrders = FindViewById<GridView>(Resource.Id.gridViewCompleteTableOrders);
            textViewTotal = FindViewById<TextView>(Resource.Id.textViewTotal);
            btnSubmitOrder = FindViewById<Button>(Resource.Id.buttonSubmitOrder);

            GetOrders();
        }

        public async void ViewOrderDetails(TableOrder tableOrder)
        {
            tableList = await tableService.RefreshDataAsync();

            Table table = tableList.Where(t => t.Id == tableOrder.TableNumId).FirstOrDefault();

            orderItemList = await orderItemService.RefreshDataAsync();

            List<OrderItem> orderedItems = orderItemList.Where(t => t.TableOrderId == tableOrder.Id).ToList();

            menuList = await menuService.RefreshDataAsync();

            List<Model.Menu> orderedMenuItems = new List<Model.Menu>();

            foreach (OrderItem item in orderedItems)
            {
                orderedMenuItems.Add(menuList.Where(i => i.Id == item.MenuId).FirstOrDefault());
            }

            string orderedItemsString = System.Environment.NewLine;

            foreach (Model.Menu item in orderedMenuItems)
            {
                orderedItemsString += item.FoodDescription + System.Environment.NewLine;
            }

            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetTitle("Order");
            builder.SetMessage($"Order for Table: {table.TableNumber} {orderedItemsString}");
            builder.SetPositiveButton("Done", delegate
            {
                CompleteOrder(tableOrder);
            });
            builder.SetNegativeButton("Cancel", delegate
            {

            });

            builder.Show();
        }
        
        private async void CompleteOrder(TableOrder tableOrder)
        {
            tableOrder.Complete = true;

            await tableOrderService.SaveTableItemAsync(tableOrder, false);

            GetOrders();
        }

        private async void GetOrders()
        {
            tableOrderList = await tableOrderService.RefreshDataAsync();

            var pendingTableOrderList = tableOrderList.Where(t => !t.Complete && t.OrderTime.Date == DateTime.Now.Date).ToList();

            var completeTableOrderList = tableOrderList.Where(t => t.Complete && t.OrderTime.Date == DateTime.Now.Date).ToList();

            tablePendingOrderGridAdapter = new TableOrderGridAdapter(this, pendingTableOrderList);
            gridViewPendingTableOrders.Adapter = tablePendingOrderGridAdapter;
            

            tableCompleteOrderGridAdapter = new TableOrderGridAdapter(this, completeTableOrderList);
            gridViewCompleteTableOrders.Adapter = tableCompleteOrderGridAdapter;
        }

    }
}