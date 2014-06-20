using System;
using System.Windows;
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

            //new Timer(delegate { if(Toggle1.IsEnabled) timePicker.Value = DateTime.Now; }, null, 60 - DateTime.Now.Second, 60000);

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
        private void AppBarOkButton_OnClick(object sender, EventArgs e)
        {
            
        }

        private void Ok(object sender, EventArgs e)
        {
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