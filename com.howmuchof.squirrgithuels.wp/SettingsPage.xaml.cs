/*
 * How many squirrels: tool for young naturalist
 *
 * This application is created within the internship
 * in the Education Department of Tomsksoft, http://tomsksoft.com
 * Idea and leading: Sergei Borisov
 *
 * This software is licensed under a GPL v3
 * http://www.gnu.org/licenses/gpl.txt
 *
 * Created by Nadyrshin Stanislav on 21.06.2014
 */

using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Controls;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class Settings
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var tr = NavigationContext.QueryString["Parametr"];
            //_currentParametr = ViewModelLocator.Main.Parametrs.First(x => x.Name == tr);
            base.OnNavigatedTo(e);
        }

        private void Ok(object sender, EventArgs e)
        {
            if(Box.Text == "") return;


            if (MessageBox.Show("Все существующие записи будут стерты", "Вы уверены?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                ((MainViewModel) DataContext).DeleteAll();

            //ViewModelLocator.Main.Parametr = ViewModelLocator.Main.Parametrs.First(x => );
            throw new NotImplementedException();
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