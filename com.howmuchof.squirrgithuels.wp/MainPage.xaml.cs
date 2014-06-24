using System;
using System.Windows.Controls;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Shell;
using com.howmuchof.squirrgithuels.wp.Resources;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class MainPage
    {
        private ApplicationBarIconButton _addButton;
        private ApplicationBarIconButton _selectButton;
        private ApplicationBarIconButton _deleteButton;
        private ApplicationBarIconButton _settingsButton;
        private ApplicationBarIconButton _cancelButton;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
            
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar {Opacity = 0.5};

            _addButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            _addButton.Click += AddButton_Click;
            //ApplicationBar.Buttons.Add(_addButton);

            _selectButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/ApplicationBar.Select.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            _selectButton.Click += SelectButtonOnClick;

            _deleteButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/delete.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            _deleteButton.Click += DeleteButtonOnClick;

            _settingsButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.settings.png", UriKind.Relative))
            {
                Text = AppResources.Settings
            };
            _settingsButton.Click += SettingsButtonOnClick;

            _cancelButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/cancel.png", UriKind.Relative))
            {
                Text = AppResources.Cancel
            };
            _cancelButton.Click += CancelButtonOnClick;

        }

        private void CancelButtonOnClick(object sender, EventArgs eventArgs)
        {
            if(MultiSelector.SelectedItems.Count > 0) MultiSelector.SelectedItems.Clear();
            MultiSelector.IsSelectionEnabled = false;
        }

        private void SettingsButtonOnClick(object sender, EventArgs eventArgs)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
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
                ViewButtons(_addButton, _selectButton, _settingsButton);
            else
                ViewButtons(_addButton, _settingsButton);
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

            MultiSelector.IsSelectionEnabled = false;
        }

        private void MultiSelector_IsSelectionEnabledChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                ViewButtons(_deleteButton, _cancelButton);
            else
                ViewButtons(_addButton, _selectButton, _settingsButton);
        }
        
        private void OnTap(object sender, GestureEventArgs e)
        {
            var block = (TextBlock)((Grid)sender).Children[0];
            using (var db = new ItemDataContext())
                NavigationService.Navigate(new Uri("/AddPage.xaml?Item=" + block.Text, UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!MultiSelector.IsSelectionEnabled) return;

            MultiSelector.IsSelectionEnabled = false;
            e.Cancel = true;
        }

    }
}