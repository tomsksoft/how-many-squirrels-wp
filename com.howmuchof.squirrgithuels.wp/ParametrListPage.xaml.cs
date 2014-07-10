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
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class ParametrListPage : PhoneApplicationPage
    {
        private ApplicationBarIconButton _addButton;
        private ApplicationBarIconButton _selectButton;
        private ApplicationBarIconButton _deleteButton;

        public ParametrListPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar() 
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            _addButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            _addButton.Click += (sender, args) => NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));

            _selectButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/ApplicationBar.Select.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            _selectButton.Click += (sender, args) => MultiSelector.IsSelectionEnabled = true;

            _deleteButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/delete.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            _deleteButton.Click += delegate
            {
                while (MultiSelector.SelectedItems.Count > 0)
                    ViewModelLocator.Main.DeleteParamrtr((Parametr) MultiSelector.SelectedItems[0]);

                MultiSelector.IsSelectionEnabled = false;
            };
            ViewButtons(_addButton, _selectButton);
        }
        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml?Parametr=" + ((TextBlock)sender).Text, UriKind.Relative));
        }

        private void ViewButtons(params ApplicationBarIconButton[] buttons)
        {
            ApplicationBar.Buttons.Clear();
            foreach (var button in buttons)
                ApplicationBar.Buttons.Add(button);
        }

        private void MultiSelector_OnIsSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue)
                ViewButtons(_deleteButton);
            else
                ViewButtons(_addButton, _selectButton);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (MultiSelector.IsSelectionEnabled)
            {
                MultiSelector.IsSelectionEnabled = false;
                return;
            }

            base.OnBackKeyPress(e);
        }
    }
}