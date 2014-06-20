using System;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using com.howmuchof.squirrgithuels.wp.Resources;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            var appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            appBarButton.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);

            // Create a new menu item with the localized string from AppResources.
            //ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        void appBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddPage.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            ((MainViewModel)DataContext).ReadDataFromDb();
        }

    }
}