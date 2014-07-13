using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Controls;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class AddTimePage : PhoneApplicationPage
    {
        private DataItem _item;
        public AddTimePage()
        {
            InitializeComponent();
            if(ViewModelLocator.Main.ActiveParametr.Type == ParametrType.Interval)
                ValueDatePicker.Visibility = Visibility.Collapsed;
        }

        private void Ok(object sender, EventArgs e)
        {
            var add = ((AddViewModel)DataContext);

            if (_item != null)
                ViewModelLocator.Main.UpdateItem(_item, BuildTime(), add.Date, add.Date);
            else
                ViewModelLocator.Main.AddItem(new DataItem(BuildTime().ToString(), add.Date, add.Date, ViewModelLocator.Main.ActiveParametr));

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Cancel(object sender, EventArgs e)
        {
            if(NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        object BuildTime()
        {
            return ViewModelLocator.Main.ActiveParametr.Type == ParametrType.Time
                ? (object) new DateTime(ValueDatePicker.Value.Value.Year,
                    ValueDatePicker.Value.Value.Month,
                    ValueDatePicker.Value.Value.Day,
                    ValueTimePicker.Value.Value.Hour,
                    ValueTimePicker.Value.Value.Minute,
                    0)
                : new TimeSpan(ValueTimePicker.Value.Value.Hour,
                    ValueTimePicker.Value.Value.Minute,
                    0);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
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
                    context.Count = _item.Count;
                    NavigationContext.QueryString.Clear();
                }
            base.OnNavigatedTo(e);
        }

    }
}