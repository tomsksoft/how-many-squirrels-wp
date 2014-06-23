using System;
using System.Windows;
using System.Windows.Controls;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class AddPage
    {
        private readonly DataItem _item;
        public AddPage()
        {
            InitializeComponent();
        }

        public AddPage(DataItem item)
        {
            _item              = item;
            Toggle1.IsChecked  = false;
            Toggle1.Visibility = Visibility.Collapsed;
            DatePicker.Value   = item.Date;
            TimePicker.Value   = item.Time;
            Box.Text           = item.Count.ToString();
        }

        private void Ok(object sender, EventArgs e)
        {
            Box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            var add = ((AddViewModel) DataContext);

            if (_item != null)
            {
                _item.Count = add.Count;
                _item.Time  = add.Date;
                _item.Date  = add.Date;
            }
            else
                ViewModelLocator.Main.AddItem(new DataItem(add.Count, add.Date, add.Date));

            if(NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Cancel(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

    }
}