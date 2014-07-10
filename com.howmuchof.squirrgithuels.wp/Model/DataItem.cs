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
            Count = count.ToString();
            Date = date;
            Time = time;
        }

        public DataItem(string count, DateTime date, DateTime time)
        {
            Count = count;
            Date = date;
            Time = time;
        }

        public DataItem()
        {
            Count = "1";
            Date  = DateTime.Now;
            Time  = DateTime.Now;
        }


        private int      _itemId;
        private string   _count;
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
                if (_itemId == value) return;

                NotifyPropertyChanging("ItemId");
                _itemId = value;
                NotifyPropertyChanged("ItemId");
            }
        }
        [Column]
        public string Count  
        {
            get { return _count; }
            set
            {
                if (_count == value) return;

                NotifyPropertyChanging("Count");
                _count = value;
                NotifyPropertyChanged("Count");
            }
        }
        public object NormalValue 
        {
            get
            {
                switch (Parametr.Type)
                {
                    case ParametrType.Int:
                        return int.Parse(Count);
                    case ParametrType.Float:
                        return float.Parse(Count);
                    case ParametrType.Time:
                        return DateTime.Parse(Count);
                    case ParametrType.Enum:
                        return Parametr.EnumList;
                    case ParametrType.Interval:
                        return TimeSpan.Parse(Count);
                    default:
                        throw new ArgumentOutOfRangeException();
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
                _date = value.Date;
                NotifyPropertyChanged("Date");
            }
        }
        public string DateS  
        { 
            get { return string.Format("{0:00}.{1:00}", Date.Day, Date.Month); } 
            set 
            {
                NotifyPropertyChanging("DateS");
                NotifyPropertyChanged("DateS"); 
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

        [Column(CanBeNull = true)]
        internal int? ParametrId;
        
        private EntityRef<Parametr> _parametr;

        [Association(Storage = "_parametr", ThisKey = "ParametrId", OtherKey = "Id", IsForeignKey = true)]
        public Parametr Parametr
        {
            get { return _parametr.Entity; }
            set
            {
                NotifyPropertyChanging("Parametr");
                _parametr.Entity = value;

                if (value != null)
                {
                    ParametrId = value.Id;
                }

                NotifyPropertyChanging("Parametr");
            }
        }

        #region Overrides

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DataItem) obj);
        }

        private bool Equals(DataItem other)
        {
            return _itemId == other._itemId;
        }

        public override int GetHashCode()
        {
            return _itemId;
        }

        public override string ToString()
        {
            return "{" + Count + " -> " + Date.ToLongDateString() + " / " + Time.ToLongTimeString() + "}";
        }

        #endregion
        
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
