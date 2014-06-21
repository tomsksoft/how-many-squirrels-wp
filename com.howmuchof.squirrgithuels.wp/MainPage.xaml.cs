using System;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using com.howmuchof.squirrgithuels.wp.Resources;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class MainPage
    {
        private ApplicationBarIconButton addButton;
        private ApplicationBarIconButton selectButton;
        private ApplicationBarIconButton deleteButton;
        private ApplicationBarIconButton settingsButton;

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
            addButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            addButton.Click += AddButton_Click;
            ApplicationBar.Buttons.Add(addButton);

            selectButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/edit.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            selectButton.Click += SelectButtonOnClick;

            deleteButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/delete.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            deleteButton.Click += DeleteButtonOnClick;

            settingsButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.settings.png", UriKind.Relative))
            {
                Text = AppResources.Settings
            };
            settingsButton.Click += SettingsButtonOnClick;

        }

        private void SettingsButtonOnClick(object sender, EventArgs eventArgs)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }


        private void ViewButtons(params ApplicationBarIconButton[] buttons)
        {
            ApplicationBar.Buttons.Clear();
            foreach (var button in buttons)
                ApplicationBar.Buttons.Add(button);
        }

        private void Pivot_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (pivot1.SelectedIndex == 1)
                ViewButtons(addButton, selectButton, settingsButton);
            else
                ViewButtons(addButton, settingsButton);
        }


        void AddButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddPage.xaml", UriKind.Relative));
        }
       
        private void SelectButtonOnClick(object sender, EventArgs eventArgs)
        {
            MultiSelector.EnforceIsSelectionEnabled = true;
        }
        private void DeleteButtonOnClick(object sender, EventArgs eventArgs)
        {
            while(MultiSelector.SelectedItems.Count > 0)
                ViewModelLocator.Main.DeleteItem((DataItem) MultiSelector.SelectedItems[0]);

            MultiSelector.EnforceIsSelectionEnabled = false;
        }

        private void MultiSelector_IsSelectionEnabledChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                ViewButtons(deleteButton);
            else
                ViewButtons(addButton, selectButton, settingsButton);
        }
    }
}