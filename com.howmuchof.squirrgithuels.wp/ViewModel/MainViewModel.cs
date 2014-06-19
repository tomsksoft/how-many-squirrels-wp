using System;
using System.Collections.ObjectModel;
using System.Globalization;
using com.howmuchof.squirrgithuels.wp.Resources;
using GalaSoft.MvvmLight;
using com.howmuchof.squirrgithuels.wp.Model;

namespace com.howmuchof.squirrgithuels.wp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<DataItem> DataService { get; private set; }


        public const string DiscriptionPropertyName = "SelectedItem";
        public const string HourPropertyName        = "Hour";
        public const string MinutPropertyName       = "Minute";
        
        private string _hour = string.Empty;


        public string Hour   
        {
            get
            {
                return DateTime.Now.Hour.ToString();
            }
            set
            {
                RaisePropertyChanged(HourPropertyName);
            }
        }
        public string Minute 
        {
            get
            {
                return DateTime.Now.Minute.ToString();
            }
            set
            {
                RaisePropertyChanged(HourPropertyName);
            }
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()//ObservableCollection<IDataService> dataService)
        {
            DataService =
                new ObservableCollection<DataItem>(new DataItem[]
                {
                    new DataItem(4, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now), 
                    new DataItem(5, DateTime.Now, DateTime.Now)
                });

            //DataService.GetData(
            //    (item, error) =>
            //    {
            //        if (error != null)
            //        {
            //            // Report error here
            //            return;
            //        }

            //        //WelcomeTitle = item.Title;
            //    });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}