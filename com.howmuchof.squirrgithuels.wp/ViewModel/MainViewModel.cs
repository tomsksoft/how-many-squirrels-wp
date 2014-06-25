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
using System.IO;
using System.Linq;
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
        private string _lastActiveTab;
        private bool   _flag;

        public ObservableCollection<DataItem> DataItems
        {
            get { return _dataItems; }
            private set
            {
                if(_dataItems == value) return;
;
                _dataItems = new ObservableCollection<DataItem>(value.OrderByDescending(x => x.Time));
                RaisePropertyChanged("DataItems");
            }
        }
        public string Parametr 
        {
            get { return _parametr; }
            private set
            {
                if(value == _parametr)
                    return;

                _parametr = value;

                var fileStream = new FileStream("Settings.xml", FileMode.Create);
                var doc = new XDocument(new XElement("settings",
                    new XElement("param", _parametr),
                    new XElement("lastTab", _lastActiveTab)));
                doc.Save(fileStream);
                fileStream.Close();

                Flag = true;

                RaisePropertyChanged("Parametr");
            }
        }
        public bool Flag
        {
            get
            {
                return _flag;
            }
            set
            {
                if(value == _flag)return;

                _flag = value;
                RaisePropertyChanged("Flag");
            }
        }    //////////////////////TODO доделать эту ерунду

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
        public void DeleteAll()               
        {
            using (var db = new ItemDataContext())
            {
                db.DataItems.DeleteAllOnSubmit(db.DataItems);
                db.SubmitChanges();
            }
            DataItems.Clear();
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

            if (File.Exists("Settings.xml"))
            {
                var document = XDocument.Load("Settings.xml");
                _parametr = document.Root.Element("param").Value;
                _lastActiveTab = document.Root.Element("lastTab").Value;
            }
            else
            {
                _parametr = "Белка";
                _lastActiveTab = "default";
            }

        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}