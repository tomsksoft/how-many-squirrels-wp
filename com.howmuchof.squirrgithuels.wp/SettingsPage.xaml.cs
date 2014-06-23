using System;
using System.Windows;
using System.Windows.Controls;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Controls;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class Settings : PhoneApplicationPage
    {
        // Constructor
        public Settings()
        {
            InitializeComponent();

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
        private void Ok(object sender, EventArgs e)
        {
            if(Box.Text == "") return;
            
            Box.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (MessageBox.Show("Все существующие записи будут стерты", "Вы уверены?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                ((MainViewModel) DataContext).DeleteAll();

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Cancel(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}