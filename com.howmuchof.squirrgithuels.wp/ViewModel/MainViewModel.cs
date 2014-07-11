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
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        private string           _parametr;
        private Tab              _lastActiveTab;
        private Visibility       _flag;
        private DateTime         _minTime;
        private DateTime         _maxTime;

        private readonly ItemDataContext  _mainContext;
        private readonly AppInfoContext   _appInfoContext;

        public MainViewModel() 
        {
            _mainContext    = new ItemDataContext();
            _appInfoContext = new AppInfoContext();
            
            //DataItems = new ObservableCollection<DataItem>();

            //DataItems.CollectionChanged += delegate
            //{
            //    MaxTime = DateTime.Now.Date;
            //    MinTime = MaxTime - new TimeSpan(5, 0, 0, 0);
            //    RaisePropertyChanged(() => GroupItems);
            //};

            MaxTime = DateTime.Now.Date;
            MinTime = MaxTime - new TimeSpan(5, 0, 0, 0);
            
        }

        #region Properties

        public ObservableCollection<DataItem> DataItems  
        {
            get { return new ObservableCollection<DataItem>(ActiveParametr.Items.OrderByDescending(x => x.Time)); }
        }
        public IEnumerable<object> GroupItems 
        {
            get
            {
                return null;
                //var tr = (from item in _mainContext.DataItems
                //         orderby item.Time
                //         group item by item.Date into lol
                //         select new { DateS = lol.Key.ToShortDateString(), Date = lol.Key, Count = lol.Max(x => x.Count) }).ToArray();

                //return tr;
                
                //if (!DataItems.Any()) return null;
                //var tmp = DataItems.OrderBy(x => x.Date).GroupBy(x => x.Date);
                //var items = new ObservableCollection<DataItem>();
                
                //foreach (var l in tmp.Where(l => l.Key.Date >= MinTime.Date && l.Key.Date <= MaxTime.Date))
                //    items.Add(new DataItem(l.Sum(x => int.Parse(x.Count)), l.Key, l.Key));
                
                //return items.Count == 0 ? null : items;
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

                RaisePropertyChanged(() => Parametr);
                RaisePropertyChanged(() => ActiveParametr);
                RaisePropertyChanged(() => DataItems);
            }
        }
        public Parametr ActiveParametr 
        {
            get { return _mainContext.Parametrs.First(x => x.Name == Parametr); }
            set
            {
                Parametr = value.Name;
                RaisePropertyChanged(() => DataItems);
                RaisePropertyChanged(() => ActiveParametr);
            }
        }
        public IEnumerable<Parametr> Parametrs
        {
            get { return new ObservableCollection<Parametr>(_mainContext.Parametrs.ToArray()); }
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

        public void AddItem(DataItem item)    
        {
            _mainContext.DataItems.InsertOnSubmit(item);
            _mainContext.SubmitChanges();
            
            RaisePropertyChanged(() => GroupItems);
            RaisePropertyChanged(() => DataItems);

        }
        public void AddParametr(Parametr p)   
        {
            if (Parametrs.Any(parametr => parametr.Name == p.Name))
                throw new Exception("Элемент с таким именем уже существует");

            _mainContext.Parametrs.InsertOnSubmit(p);
            _mainContext.SubmitChanges();
            RaisePropertyChanged(() => Parametrs);
        }
        public void DeleteItem(DataItem item) 
        {
            var tr = _mainContext.DataItems.First(x => x.ItemId == item.ItemId);
            tr.Parametr.Items.Remove(tr);
            _mainContext.DataItems.DeleteOnSubmit(tr);
            _mainContext.SubmitChanges();
            
            RaisePropertyChanged(() => GroupItems);
            RaisePropertyChanged(() => DataItems);
        }
        public void DeleteParamrtr(Parametr p)
        {
            var pt = _mainContext.Parametrs.First(x => x.Id == p.Id);

            _mainContext.DataItems.DeleteAllOnSubmit(pt.Items); // сначала удалить все элементы
            _mainContext.Parametrs.DeleteOnSubmit(pt);          // потом сам параметр
            _mainContext.SubmitChanges();
            RaisePropertyChanged(() => Parametrs);
        }
        public void DeleteAll(Parametr parametr)
        {
            _mainContext.DataItems.DeleteAllOnSubmit(_mainContext.DataItems.Where(x => x.Parametr.Equals(parametr)));
            _mainContext.SubmitChanges();
            
            RaisePropertyChanged(() => GroupItems);
        }
        public void UpdateItem(DataItem item, object count, DateTime date, DateTime time)
        {
            var oldItem = _mainContext.DataItems.First(x => x.ItemId == item.ItemId);
            oldItem.Count = count.ToString();
            oldItem.Date  = date;
            oldItem.Time  = time;
            _mainContext.SubmitChanges();

            RaisePropertyChanged(() => GroupItems);
        }
        public void UpdateParametr(Parametr parametr, string name, ParametrType type)
        {
            var param = Parametrs.First(x => x.Equals(parametr));
            param.Name = name;
            param.Type = type;
            param.EnumList = null;

            _mainContext.SubmitChanges();
            RaisePropertyChanged(() => Parametrs);
        }

        public void UpdateParametr(Parametr parametr, string name, IEnumerable<string> enumList)
        {
            var param = Parametrs.First(x => x.Equals(parametr));
            param.Name = name;
            param.Type = ParametrType.Enum;
            param.EnumList = enumList;

            _mainContext.SubmitChanges();
            RaisePropertyChanged(() => Parametrs);
        }

        //private int BinarySearch(DataItem item)
        //{
        //    var left = 0;
        //    var right = DataItems.Count;
        //    while (true)
        //    {
        //        var mid = left + (right - left)/2;

        //        if (right <= left) return mid;

        //        if (item.Time > DataItems[mid].Time)
        //        {
        //            right = mid;
        //            continue;
        //        }

        //        left = mid + 1;
        //    }
        //}

        #endregion

        #region SettingsDataBase

        public void ReadSettings()
        {
            var appInfo = (from AppInfo app in _appInfoContext.AppInfo select app).First();

            _parametr = appInfo.Parametr;
            _lastActiveTab = appInfo.LastTab;
        }

        private void ChangeParametr(string parametr)
        {
            var appInfo = _appInfoContext.AppInfo.First();

            appInfo.Parametr = parametr;
            _appInfoContext.SubmitChanges();
        }

        private void ChangeLastTab(Tab tab)
        {
            var appInfo = _appInfoContext.AppInfo.First();

            appInfo.LastTab = tab;
            _appInfoContext.SubmitChanges();
            
        }

        #endregion
        

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}