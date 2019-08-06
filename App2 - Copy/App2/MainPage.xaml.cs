using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Storage;
using System.Net.Http;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

    }

        IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();
        MobileServiceCollection<TodoItem, TodoItem> items;

        public class TodoItem
        {
            public string Id { get; set; }
            public bool Complete { get; set; }
            public string ItemNumber { get; set; }
            public string ItemName { get; set; }
            public string ItemPrice { get; set; }
            public string ItemColor { get; set; }
            public string ItemSize { get; set; }
        }

        async private void Submit_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                TodoItem item = new TodoItem
                {
                    Id = null,
                    Complete = false,
                    ItemNumber = txtBxItemNumber.Text,
                    ItemName = txtBxItemName.Text,
                    ItemPrice = txtBxItemPrice.Text,
                    ItemColor = txtBxItemColor.Text,
                    ItemSize = txtBxItemSize.Text
                };

                await App.MobileService.GetTable<TodoItem>().InsertAsync(item);
                var dialog = new MessageDialog("Successful!");
                await dialog.ShowAsync();
            }
            catch (Exception em)
            {
                var dialog = new MessageDialog("An Error Occured: " + em.Message);
                await dialog.ShowAsync();
            }
        }


        private async Task RefreshTodoItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems
                items = await todoTable
                    .Where(TodoItem => TodoItem.Complete == false)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                ListItems.ItemsSource = items;
                this.btnRefresh.IsEnabled = true;
            }
        }

        private async void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            await UpdateCheckedTodoItem(item);
        }

        private async Task UpdateCheckedTodoItem(TodoItem item)
        {
            // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
            // responds, the item is removed from the list 
            await todoTable.UpdateAsync(item);
            items.Remove(item);
            ListItems.Focus(Windows.UI.Xaml.FocusState.Unfocused);

            //await SyncAsync(); // offline sync
        }

        async private void btnRefresh_Click_1(object sender, RoutedEventArgs e)
        {
          await RefreshTodoItems();
        }






        public static string AWS_Name { get; set; }






        private void btnGetAPI_Click(object sender, RoutedEventArgs e)
        {
            GetAWS();
        }
        public void GetAWS()
        {
            var Var2 = new Amazon.Auth.AccessControlPolicy.Policy();
            AWS_Name = Convert.ToString(Var2.Version);
            GetAPI.Text = "API: " + AWS_Name;
        }

        private void TxtBxItemNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtBxItemName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtBxItemPrice_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtBxItemColor_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtBxItemSize_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        async private void GeoLoc_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GeoLocation));
        }

    }
}
