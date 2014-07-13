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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;
using Microsoft.Phone.Controls;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class Settings
    {
        private Parametr Parametr { get; set; }
        private bool flag;
        private string oldString;

        public Settings()
        {
            InitializeComponent();
            
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("Parametr"))
            {
                var tr = NavigationContext.QueryString["Parametr"];
                Parametr = ViewModelLocator.Main.Parametrs.First(x => x.Name == tr);

                var l = Parametr.EnumList;
                if(l != null)
                foreach (var s in l)
                    EnumBox.Text += s + "; ";

                DataContext = Parametr;
                
                Picker.SelectedIndex = (int) Parametr.Type;
            }
            base.OnNavigatedTo(e);
        }

        private void Ok(object sender, EventArgs e)
        {
            if (flag)
            {
                Focus();
                return;
            }
            if (Box.Text == "")
            {
                MessageBox.Show("Название не может быть пустым");
                return;
            }

            if (Parametr != null) // Значит меняем значения у существующего параметра
            {
                if (Box.Text != Parametr.Name || Picker.SelectedIndex != (int)Parametr.Type)
                    if (
                        MessageBox.Show("Все существующие записи будут стерты", "Вы уверены?", MessageBoxButton.OKCancel) ==
                        MessageBoxResult.OK)
                    {
                        ViewModelLocator.Main.DeleteAll(Parametr);

                        if(((ParametrType)Picker.SelectedIndex) == ParametrType.Enum)
                            ViewModelLocator.Main.UpdateParametr(Parametr, Box.Text, GetEnum());
                        else
                            ViewModelLocator.Main.UpdateParametr(Parametr, Box.Text, (ParametrType)Picker.SelectedIndex);   
                    }
            }
            else
            {
                try
                {
                    Parametr = ((ParametrType) Picker.SelectedIndex) == ParametrType.Enum
                        ? new Parametr(Box.Text, GetEnum())
                        : new Parametr(Box.Text, (ParametrType) Picker.SelectedIndex);

                    ViewModelLocator.Main.AddParametr(Parametr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            if(sender is bool) ViewModelLocator.Main.ActiveParametr = Parametr;

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            
        }

        private IEnumerable<string> GetEnum()
        {
            return EnumBox.Text.Replace(" ", "").Split(';');
        }

        private void Cancel(object sender, EventArgs e)
        {
            if (flag)
            {
                EnumBox.Text = oldString;
                Focus();
                return;
            }
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Activate(object sender, EventArgs e)
        {
            Ok(true, null);
        }
        private void EnumBox_KeyDown(object sender, KeyEventArgs e)
        {
            //EnumBox.Text = EnumBox.Text.Trim();
            switch (e.Key)
            {
                case Key.Space:
                {
                    var tr = EnumBox.Text.Last();
                    if (char.IsWhiteSpace(tr))
                        e.Handled = true;
                    else
                        EnumBox.Text += "; ";

                    EnumBox.SelectionStart = EnumBox.Text.Length;
                    break;
                }

                case Key.Back:
                {
                    var tr = EnumBox.SelectionStart - 1;
                    if (tr >=0 && (EnumBox.Text[tr] == ' ' || EnumBox.Text[tr] == ';'))
                    {
                        if (EnumBox.Text[tr] == ';') tr++;
                        while (--tr >= 0 && EnumBox.Text[tr] != ' ')
                            EnumBox.Text = EnumBox.Text.Remove(tr, 1);

                        EnumBox.Text = EnumBox.Text.Length == 1 
                            ? "" 
                            : EnumBox.Text.Replace("  ", " ");

                        EnumBox.SelectionStart = tr + 1;
                    }
                    break;
                }
            }

        }

        private void EnumBox_GotFocus(object sender, RoutedEventArgs e)
        {
            flag = true;
            oldString = EnumBox.Text;
        }

        private void EnumBox_LostFocus(object sender, RoutedEventArgs e)
        {
            flag = false;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            var textBox = new TextBox();
            var box = new CustomMessageBox
            {
                Caption = "Ввод значения",
                Message = "Введите название параметра",
                LeftButtonContent = "Ok",
                RightButtonContent = "Cancel",
                Content = textBox    
            };

            box.Loaded += (o, args) => textBox.Focus();

            box.Dismissed += (s, boxEventArgs) =>
            {
                if(boxEventArgs.Result == CustomMessageBoxResult.LeftButton)
                    EnumBox.Text += textBox.Text + "; ";
            };

            box.Show();
        }
    }
}