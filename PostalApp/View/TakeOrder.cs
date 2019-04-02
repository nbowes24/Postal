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
    public class TakeOrder : Activity
    {
        private List<Category> categoryList = new List<Category>();
        private List<Model.Menu> menuList = new List<Model.Menu>();
        private MenuGridAdapter menuGridAdapter;
        private MenuAdapter menuAdapter;
        private CategoryService categoryService = new CategoryService();
        private MenuService menuService = new MenuService();
        private CategoryAdapter categoryAdapter;
        private GridView gridViewMenuItems;
        private Spinner spinnerMenuCategories;
        private ListView listviewOrderItems;
        private List<Model.Menu> orderList = new List<Model.Menu>();
        private TextView textViewTotal;
        private Button btnSubmitOrder;
        private OrderItemService orderItemService = new OrderItemService();
        private TableOrderService tableOrderService = new TableOrderService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            // Set our view from the "main" layout resource
            
            SetContentView(Resource.Layout.take_order);

            gridViewMenuItems = FindViewById<GridView>(Resource.Id.gridViewMenuItems);
            spinnerMenuCategories = FindViewById<Spinner>(Resource.Id.spinnerMenuCategories);
            listviewOrderItems = FindViewById<ListView>(Resource.Id.listviewOrderItems);
            textViewTotal = FindViewById<TextView>(Resource.Id.textViewTotal);
            btnSubmitOrder = FindViewById<Button>(Resource.Id.buttonSubmitOrder);

            menuAdapter = new MenuAdapter(this, orderList);

            listviewOrderItems.Adapter = menuAdapter;

            GetCategories();
            

            spinnerMenuCategories.ItemSelected += SpinnerMenuCategories_ItemSelected;

            listviewOrderItems.ItemClick += listviewOrderItems_ItemClick;

            btnSubmitOrder.Click += delegate
            {
                SubmitOrder();
            };
        }

        private void listviewOrderItems_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            RemoveItemAsync(e);
        }

        private async void RemoveItemAsync(AdapterView.ItemClickEventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
                .SetTitle($"Delete item: {orderList[e.Position].FoodDescription}")
                .SetOkText("Delete")
                .SetCancelText("Cancel"));

            if (result)
            {
                orderList.RemoveAt(e.Position);

                SetPrice();

                menuAdapter.NotifyDataSetChanged();
            }
        }

        public void AddToOrder(Model.Menu menu)
        {
            orderList.Add(menu);

            menuAdapter = new MenuAdapter(this, orderList);

            listviewOrderItems.Adapter = menuAdapter;

            SetPrice();
        }

        private void SetPrice()
        {
            decimal total = 0;
            foreach(Model.Menu item in orderList)
            {
                total += item.Price;
            }
            textViewTotal.Text = string.Format("Total: {0:N2}", total);
        }

        private void SpinnerMenuCategories_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            GetMenu(categoryList[e.Position].Id);
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

        private async void SubmitOrder()
        {
            using (UserDialogs.Instance.Loading("Submitting Order..."))
            {

                TableOrder tableOrder = new TableOrder()
                {
                    OrderTime = DateTime.Now,
                    StaffId = Intent.GetIntExtra("StaffId", 1),
                    TableNumId = Intent.GetIntExtra("TableId", 1)
                };

                HttpResponseMessage tableOrderResponse = await tableOrderService.SaveTableItemAsync(tableOrder, true);

                var tableOrderContent = await tableOrderResponse.Content.ReadAsStringAsync();
                var newTableOrder = JsonConvert.DeserializeObject<TableOrder>(tableOrderContent);

                foreach (Model.Menu item in orderList)
                {
                    OrderItem orderItem = new OrderItem()
                    {
                        MenuId = item.Id,
                        TableOrderId = newTableOrder.Id
                    };

                    await orderItemService.SaveTableItemAsync(orderItem, true);
                }
            }

            await UserDialogs.Instance.AlertAsync("Order submitted");

            Finish();
        }
    }
}