using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using com.howmuchof.squirrgithuels.wp.Resources;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class AddEnumPage : PhoneApplicationPage
    {
        private DataItem _item;
        public AddEnumPage()
        {
            InitializeComponent();
        }

        private void Ok(object sender, EventArgs e)
        {
            var add = ((AddViewModel)DataContext);
            var count = Picker.SelectedItem.ToString();

            if (_item != null)
                ViewModelLocator.Main.UpdateItem(_item, count, add.Date, add.Date);
            else
            {
                var param = ViewModelLocator.Main.ActiveParametr;
                ViewModelLocator.Main.AddItem(new DataItem(count, add.Date, add.Date, param));
            }

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Cancel(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("Item"))
                using (var db = new ItemDataContext())
                {
                    _item = db.DataItems.FirstOrDefault(
                        x => x.ItemId.ToString() == NavigationContext.QueryString["Item"]); 
                    if (_item == null) return;

                    var context = (AddViewModel)DataContext;

                    context.IsSelfTime = true;
                    Toggle1.Visibility = Visibility.Collapsed;
                    context.Date = new DateTime(_item.Date.Year, _item.Date.Month, _item.Date.Day,
                                                       _item.Time.Hour, _item.Time.Minute, _item.Time.Second);
                    Picker.SelectedItem = _item.Count;
                    NavigationContext.QueryString.Clear();
                }
            base.OnNavigatedTo(e);
        }
    }
}