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
 * Created by Nadyrshin Stanislav on 18.04.2014
 */

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Shell;
using com.howmuchof.squirrgithuels.wp.Resources;
using Sparrow.Chart;
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
        private ApplicationBarIconButton _graphButton;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();

            pivot1.SelectedIndex = (int)((MainViewModel) DataContext).LastActiveTab;
            
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();// {Opacity = 0.5};

            _addButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative))
            {
                Text = AppResources.AppBarButtonText
            };
            _addButton.Click += AddButton_Click;

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

            _graphButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh.png", UriKind.Relative))
            {
                Text = AppResources.GraphView
            };
            _graphButton.Click += GraphButtonOnClick;
            
        }

        private void GraphButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (Chart1.Visibility == Visibility.Visible)
            {
                Chart1.Visibility = Visibility.Collapsed;
                Chart2.Visibility = Visibility.Visible;
            }
            else
            {
                Chart1.Visibility = Visibility.Visible;
                Chart2.Visibility = Visibility.Collapsed;
            }
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

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (pivot1.SelectedIndex)
            {
                case 1:
                    ViewButtons(_addButton, _selectButton, _settingsButton);
                    break;
                case 2:
                    ViewButtons(_addButton, _settingsButton, _graphButton);
                    break;
                default:
                    ViewButtons(_addButton, _settingsButton);
                    break;
            }
            ((MainViewModel) DataContext).LastActiveTab = (Tab)pivot1.SelectedIndex;
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

        private void GraphButton(object sender, RoutedEventArgs e)
        {
            pivot1.SelectedIndex = 2;
        }
        
    }
}