﻿using System;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
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
        private string _parametr;

        public ObservableCollection<DataItem> DataItems
        {
            get { return _dataItems; }
            private set
            {
                if(_dataItems == value) return;
;
                _dataItems = new ObservableCollection<DataItem>(value.OrderByDescending(x => x.Date));
                RaisePropertyChanged("DataItems");
            }
        }

        

        #region База данных 

        public void ReadDataFromDb()
        {
            using (var db = new ItemDataContext())
            {
                var items = from DataItem item in db.DataItems select item;
                DataItems = new ObservableCollection<DataItem>(items);
            }

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
        }

        public void UpdateItem(DataItem item)
        {
            using (var db = new ItemDataContext())
            {
                //db.DataItems.FirstOrDefault(x => x.ItemId == item.ItemId);
            }
        }

        private int BinarySearch(DataItem item, int left, int right)
        {
            int mid = left + (right - left) / 2;

            if (right <= left) return mid;

            if (item.Time > DataItems[mid].Time) return BinarySearch(item, left, mid);
            
            return BinarySearch(item, mid + 1, right);
        }

        #endregion


        public MainViewModel()
        {
            DataItems = new ObservableCollection<DataItem>();
            //var root = XElement.Load("/Resources/Settings.xml");
            
            //_parametr = root.Element("Parametr").Value;
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}