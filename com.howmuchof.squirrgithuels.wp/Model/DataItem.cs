using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace com.howmuchof.squirrgithuels.wp.Model
{
    [Table]
    public class DataItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public DataItem(int count, DateTime date, DateTime time)
        {
            Count = count;
            Date = date;
            Time = time;
        }

        public DataItem()
        {
            Count = 1;
            Date  = DateTime.Now;
            Time  = DateTime.Now;
        }


        private int _itemId;
        private int _count;
        private DateTime _date;
        private DateTime _time;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                if (_itemId != value)
                {
                    NotifyPropertyChanging("ItemId");
                    _itemId = value;
                    NotifyPropertyChanged("ItemId");
                }
            }
        }
        [Column]
        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    NotifyPropertyChanging("Count");
                    _count = value;
                    NotifyPropertyChanged("Count");
                }
            }
        }
        [Column]
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date == value) return;
                NotifyPropertyChanging("Date");
                _date = value;
                NotifyPropertyChanged("Date");
            }
        }
        [Column]
        public DateTime Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    NotifyPropertyChanging("Time");
                    _time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        public override bool Equals(object obj)
        {
            var t = (DataItem) obj;
            return _itemId == t._itemId;
        }

        #region INotifyPropertyChanged AND INotifyPropertyChanging MEMBERS

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

    }
}
