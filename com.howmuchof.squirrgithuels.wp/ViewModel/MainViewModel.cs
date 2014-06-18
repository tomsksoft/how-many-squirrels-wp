using System;
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
        private readonly IDataService _dataService;


        public const string DiscriptionPropertyName = "SelectedItem";
        public const string HourPropertyName = "Hour";
        public const string MinutPropertyName = "Minute";
        
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
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    //WelcomeTitle = item.Title;
                });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}