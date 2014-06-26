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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using GalaSoft.MvvmLight;
using com.howmuchof.squirrgithuels.wp.Model;
using Microsoft.Phone.Controls;

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
        private ObservableCollection<DataItem> _dataItems;
        private string _parametr;
        private Tab _lastActiveTab;
        private Visibility   _flag;
        private DateTime _minTime;
        private DateTime _maxTime;

        public MainViewModel()
        {
            DataItems = new ObservableCollection<DataItem>();
        }

        #region Properties

        public ObservableCollection<DataItem> DataItems  
        {
            get { return _dataItems; }
            private set
            {
                if (_dataItems == value) return;
                ;
                _dataItems = new ObservableCollection<DataItem>(value.OrderByDescending(x => x.Time));
                RaisePropertyChanged("DataItems");
                RaisePropertyChanged("GroupItems");
            }
        }

        public ObservableCollection<DataItem> GroupItems 
        {
            get
            {
                if (!DataItems.Any()) return null;
                var items = new ObservableCollection<DataItem>();
                var count = 0;
                var sum = 0;
                var tmp = DataItems.Where(x => x.Date >= MinTime && x.Date <= MaxTime).ToArray();
                if(!tmp.Any()) return null;
                var date = tmp.First().Date;  

                foreach (var item in tmp)
                {
                    if (item.Date.ToShortDateString() != date.ToShortDateString())
                    {
                        items.Add(new DataItem(sum/count, new DateTime(date.Year, date.Month, date.Day, 0, 0, 0), date));
                        sum = 0;
                        count = 0;
                        date = item.Date;
                    }
                    count++;
                    sum += item.Count;
                }
                items.Add(new DataItem(sum/count, new DateTime(date.Year, date.Month, date.Day, 0, 0, 0), date));

                return items;
            }
        }

        public string Parametr   
        {
            get { return _parametr; }
            private set
            {
                if (value == _parametr)
                    return;

                _parametr = value;

                ChangeParametr(value);

                Flag = Visibility.Collapsed;

                RaisePropertyChanged("Parametr");
            }
        }

        public Visibility Flag   
        {
            get { return _parametr != "Белка" ? Visibility.Collapsed : _flag; }
            set
            {
                if (value == _flag) return;

                _flag = value;
                RaisePropertyChanged("Flag");
            }
        }

        public Tab LastActiveTab 
        {
            get { return _lastActiveTab; }
            set
            {
                if(value == _lastActiveTab) return;

                ChangeLastTab(value);

                _lastActiveTab = value;
                RaisePropertyChanged("LastActiveTab");
            }
        }

        public DateTime MinTime  
        {
            get { return _minTime; }
            set
            {
                if(value == _minTime)return;

                _minTime = value;
                RaisePropertyChanged("MinTime");
                RaisePropertyChanged("Minlol");
                RaisePropertyChanged("GroupItems");
            }
        }

        public DateTime MaxTime  
        {
            get { return _maxTime; }
            set
            {
                if(value == _maxTime) return;

                _maxTime = value;
                RaisePropertyChanged("MaxTime");
                RaisePropertyChanged("Maxlol");
                RaisePropertyChanged("GroupItems");
            }
        }

        public double Maxlol { get { return new DateTime(_maxTime.Year, _maxTime.Month, _maxTime.Day, 0, 0, 0, 0).ToOADate(); } }

        public double Minlol { get { return new DateTime(_minTime.Year, _minTime.Month, _minTime.Day, 0, 0, 0, 0).ToOADate(); } }
        #endregion
        
        #region База данных 

        public void ReadDataFromDb()          
        {
            using (var db = new ItemDataContext())
            {
                var items = from DataItem item in db.DataItems select item;
                DataItems = new ObservableCollection<DataItem>(items);
            }

            DataItems.CollectionChanged += delegate
            {
                MaxTime = _dataItems.Max(x => x.Date);
                MinTime = MaxTime - new TimeSpan(5, 00, 00, 00);
                RaisePropertyChanged("GroupItems");
            };

            MaxTime = _dataItems.Max(x => x.Date);
            MinTime = MaxTime - new TimeSpan(5, 0, 0, 0);
        }
        
        public void AddItem(DataItem item)    
        {
            using (var db = new ItemDataContext())
            {
                db.DataItems.InsertOnSubmit(item);
                db.SubmitChanges();
            }
            var h = BinarySearch(item, 0, DataItems.Count);
            DataItems.Insert(h, item);
            RaisePropertyChanged("GroupItems");
        }
        public void DeleteItem(DataItem item) 
        {
            using (var db = new ItemDataContext())
            {
                var tr = db.DataItems.First(x => x.ItemId == item.ItemId);
                db.DataItems.DeleteOnSubmit(tr);
                db.SubmitChanges();
            }
            DataItems.Remove(item);
            RaisePropertyChanged("GroupItems");
        }
        public void DeleteAll()               
        {
            using (var db = new ItemDataContext())
            {
                db.DataItems.DeleteAllOnSubmit(db.DataItems);
                db.SubmitChanges();
            }
            DataItems.Clear();
            RaisePropertyChanged("GroupItems");
        }
        public void UpdateItem(DataItem item, int count, DateTime date, DateTime time)
        {
            using (var db = new ItemDataContext())
            {
                var oldItem = db.DataItems.First(x => x.ItemId == item.ItemId);
                oldItem.Count = count;
                oldItem.Date  = date;
                oldItem.Time  = time;
                db.SubmitChanges();
            }

            var first = DataItems.First(x => x.ItemId == item.ItemId);
            first.Count = count;
            first.Date = date;
            first.Time = time;
            RaisePropertyChanged("GroupItems");
        }
        private int BinarySearch(DataItem item, int left, int right)
        {
            int mid = left + (right - left) / 2;

            if (right <= left) return mid;

            if (item.Time > DataItems[mid].Time) return BinarySearch(item, left, mid);
            
            return BinarySearch(item, mid + 1, right);
        }

        #endregion

        #region SettingsDataBase

        public void ReadSettings()
        {
            using (var db = new AppInfoContext())
            {
                var appInfo = (from AppInfo app in db.AppInfo select app).First();

                _parametr = appInfo.Parametr;
                _lastActiveTab = appInfo.LastTab;
            }
        }

        private void ChangeParametr(string parametr)
        {
            using (var db = new AppInfoContext())
            {
                var appInfo = db.AppInfo.ToArray()[0];

                appInfo.Parametr = parametr;
                db.SubmitChanges();
            }
        }

        private void ChangeLastTab(Tab tab)
        {
            using (var db = new AppInfoContext())
            {
                var appInfo = db.AppInfo.ToArray()[0];

                appInfo.LastTab = tab;
                db.SubmitChanges();
            }
        }

        #endregion

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}