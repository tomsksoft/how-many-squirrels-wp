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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        private ObservableCollection<DataItem> _dataItems;
        private string           _parametr;
        private Tab              _lastActiveTab;
        private Visibility       _flag;
        private DateTime         _minTime;
        private DateTime         _maxTime;

        private readonly ItemDataContext  _context;

        public MainViewModel() 
        {
            DataItems = new ObservableCollection<DataItem>();

            DataItems.CollectionChanged += delegate
            {
                MaxTime = DateTime.Now.Date;
                MinTime = MaxTime - new TimeSpan(5, 0, 0, 0);
                RaisePropertyChanged("GroupItems");
            };

            _context = new ItemDataContext();
            //SetLine();
        }

        #region Properties

        public ObservableCollection<DataItem> DataItems  
        {
            get { return _dataItems; }
            private set
            {
                if (_dataItems == value) return;
                
                _dataItems = new ObservableCollection<DataItem>(value.OrderByDescending(x => x.Time));
                RaisePropertyChanged("DataItems");
                RaisePropertyChanged("GroupItems");
            }
        }

        public IEnumerable<DataItem> GroupItems 
        {
            get
            {
                if (!DataItems.Any()) return null;
                var tmp = DataItems.OrderBy(x => x.Date).GroupBy(x => x.Date);
                var items = new ObservableCollection<DataItem>();
                
                foreach (var l in tmp.Where(l => l.Key.Date >= MinTime.Date && l.Key.Date <= MaxTime.Date))
                    items.Add(new DataItem(l.Sum(x => (int)x.Count), l.Key, l.Key));
                
                return items.Count == 0 ? null : items;
            }
        }

        public string Parametr   
        {
            get { return _parametr; }
            set
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

        //public SeriesCollection SeriesCollection
        //{
        //    get { return _series; }
        //    set
        //    {
        //        if(_series == value)
        //            return;

        //        _series = value;
        //        AddBinding();
        //        RaisePropertyChanged(() => SeriesCollection);
        //        RaisePropertyChanged(() => XAxes);
        //    }
        //}

        //public XAxis XAxes       
        //{
        //    get
        //    {
        //        if(SeriesCollection.Count == 2) return new DateTimeXAxis {Interval = new TimeSpan(1, 0, 0, 0)};
        //        return new CategoryXAxis();
        //    }
        //    set
        //    {
        //    }
        //}

        public DateTime MinTime  
        {
            get { return _minTime; }
            set
            {
                if(value == _minTime)return;

                _minTime = value;
                RaisePropertyChanged("MinTime");
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
                RaisePropertyChanged(() => MaxTime);
                RaisePropertyChanged(() => GroupItems);
            }
        }
        
        #endregion
        
        #region База данных 

        public void ReadDataFromDb()          
        {
            var items = from DataItem item in _context.DataItems select item;
            DataItems = new ObservableCollection<DataItem>(items);

            MaxTime = DateTime.Now.Date;
            MinTime = MaxTime - new TimeSpan(5, 0, 0, 0);
        }
        
        public void AddItem(DataItem item)    
        {
            _context.DataItems.InsertOnSubmit(item);
            _context.SubmitChanges();
            
            var h = BinarySearch(item, 0, DataItems.Count);
            DataItems.Insert(h, item);
            RaisePropertyChanged(() => GroupItems);
        }
        public void DeleteItem(DataItem item) 
        {
            var tr = _context.DataItems.First(x => x.ItemId == item.ItemId);
            _context.DataItems.DeleteOnSubmit(tr);
            _context.SubmitChanges();
            
            DataItems.Remove(item);
            RaisePropertyChanged("GroupItems");
        }
        public void DeleteAll()               
        {
            _context.DataItems.DeleteAllOnSubmit(_context.DataItems);
            _context.SubmitChanges();
            
            DataItems.Clear();
            RaisePropertyChanged("GroupItems");
        }
        public void UpdateItem(DataItem item, int count, DateTime date, DateTime time)
        {
            var oldItem = _context.DataItems.First(x => x.ItemId == item.ItemId);
            oldItem.Count = count;
            oldItem.Date  = date;
            oldItem.Time  = time;
            _context.SubmitChanges();

            var first = DataItems.First(x => x.ItemId == item.ItemId);
            first.Count = count;
            first.Date = date;
            first.Time = time;
            RaisePropertyChanged("GroupItems");
        }

        private int BinarySearch(DataItem item, int left, int right)
        {
            while (true)
            {
                var mid = left + (right - left)/2;

                if (right <= left) return mid;

                if (item.Time > DataItems[mid].Time)
                {
                    right = mid;
                    continue;
                }

                left = mid + 1;
            }
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
                var appInfo = db.AppInfo.First();

                appInfo.Parametr = parametr;
                db.SubmitChanges();
            }
        }

        private void ChangeLastTab(Tab tab)
        {
            using (var db = new AppInfoContext())
            {
                var appInfo = db.AppInfo.First();

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