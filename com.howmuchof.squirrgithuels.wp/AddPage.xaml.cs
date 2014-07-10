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
 * Created by Nadyrshin Stanislav on 18.06.2014
 */

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using com.howmuchof.squirrgithuels.wp.Model;
using com.howmuchof.squirrgithuels.wp.ViewModel;

namespace com.howmuchof.squirrgithuels.wp
{
    public partial class AddPage
    {
        private DataItem _item;
        public AddPage()
        {
            InitializeComponent();
        }

        private void Ok(object sender, EventArgs e)
        {
            Box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            var add = ((AddViewModel) DataContext);

            if (_item != null)
                ViewModelLocator.Main.UpdateItem(_item, add.Count, add.Date, add.Date);
            else
                ViewModelLocator.Main.AddItem(new DataItem(add.Count, add.Date, add.Date));

            if(NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Cancel(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if(NavigationContext.QueryString.ContainsKey("Item"))
                using (var db = new ItemDataContext())
                {
                    _item = db.DataItems.FirstOrDefault(
                        x => x.ItemId.ToString() == NavigationContext.QueryString["Item"]);
                    if(_item == null) return;

                    var context = (AddViewModel) DataContext;

                    context.IsSelfTime  = true;
                    Toggle1.Visibility  = Visibility.Collapsed;
                    context.Date        = new DateTime(_item.Date.Year, _item.Date.Month,  _item.Date.Day, 
                                                       _item.Time.Hour, _item.Time.Minute, _item.Time.Second);
                    context.Count       = _item.Count;
                    NavigationContext.QueryString.Clear();
                }
            base.OnNavigatedTo(e);
        }

    }
}