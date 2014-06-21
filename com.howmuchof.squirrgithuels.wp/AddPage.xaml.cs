using System;
using System.Windows.Controls;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Controls;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class AddPage : PhoneApplicationPage
    {
        // Constructor
        public AddPage()
        {
            InitializeComponent();
            
        }

        private void Ok(object sender, EventArgs e)
        {
            Box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            var add = ((AddViewModel) DataContext);
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