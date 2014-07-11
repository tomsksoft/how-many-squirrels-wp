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
 * Created by Nadyrshin Stanislav on 02.07.2014
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    public enum ParametrType { Int, Float, Time, Enum, Interval }
    [Table]
    public class Parametr : INotifyPropertyChanged, INotifyPropertyChanging
    {

        private int _id;
        private string _name;
        private string _enum;
        private ParametrType _type;
        
        public Parametr()
        {
            _items = new EntitySet<DataItem>(AttachItem, DetachItem); 
            Type = ParametrType.Int;
            Name = "Белка";
        }

        public Parametr(string name, ParametrType type)
        {
            _items = new EntitySet<DataItem>(AttachItem, DetachItem); 
            Type = type;
            Name = name;
        }

        public Parametr(string name, IEnumerable<string> enumList)
        {
            _items = new EntitySet<DataItem>(AttachItem, DetachItem);
            Type = ParametrType.Enum;
            Name = name;
            EnumList = enumList;
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id      
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value) return;

                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }
        [Column]
        public ParametrType Type 
        {
            get { return _type; }
            set
            {
                if(_type == value) return;

                NotifyPropertyChanging("Type");
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }
        [Column(CanBeNull = true)]
        private string Enum 
        {
            get { return _enum; }
            set
            {
                if(_enum == value) return;

                NotifyPropertyChanging("Enum");
                _enum = value;
                NotifyPropertyChanged("Enum");
            }
        }
        [Column]
        public string Name 
        {
            get { return _name; }
            set
            {
                if(value == _name) return;

                NotifyPropertyChanging("Name");
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        #region Methods

        public bool IsEnum     
        {
            get { return Type == ParametrType.Enum; }
        }

        public string TypeName 
        {
            get
            {
                switch (Type)
                {
                    case ParametrType.Int:
                        return "Целое число";
                    case ParametrType.Float:
                        return "Вещественное число";
                    case ParametrType.Time:
                        return "Момент времени";
                    case ParametrType.Enum:
                        return "Перечислимый параметр";
                    case ParametrType.Interval:
                        return "Интервал времени";
                }
                throw new ArgumentException();
            }

        }

        public static ParametrType TypeFromName(string name)
        {
            switch (name)
            {
                case "Целое число":
                    return ParametrType.Int;
                case "Вещественное число":
                    return ParametrType.Float;
                case "Момент времени":
                    return ParametrType.Time;
                case "Перечислимый параметр":
                    return ParametrType.Enum;
                case "Интервал времени":
                    return ParametrType.Interval;
            }
            throw new ArgumentException();
        }

        public IEnumerable<string> EnumList  
        {
            get
            {
                return IsEnum && Enum != null
                    ? Enum.Split(';').Where(x => x != "")
                    : null;
            }
            set
            {
                if (value == null)
                {
                    Enum = ""; 
                    return;
                }

                Type = ParametrType.Enum;
                Enum = "";
                foreach (var i in value.Where(x => x != ""))
                    Enum += i + ";";
            }
        }

        #endregion
        
        [Column(IsVersion = true)]
        private Binary _version;

        // Define the entity set for the collection side of the relationship.
        private EntitySet<DataItem> _items = new EntitySet<DataItem>();
        
        [Association(Storage = "_items", OtherKey = "ParametrId"/*, ThisKey = "Id"*/)]
        public EntitySet<DataItem> Items
        {
            get { return _items; }
            set
            {
                _items.Assign(value);
                NotifyPropertyChanged("Items");
            }
        }

        // Called during an add operation
        private void AttachItem(DataItem item)
        {
            NotifyPropertyChanging("DataItem");
            item.Parametr = this;
        }

        // Called during a remove operation
        private void DetachItem(DataItem item)
        {
            NotifyPropertyChanging("DataItem");
            item.Parametr = null;
        }

        #region Overrides

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Parametr) obj);
        }
        
        private bool Equals(Parametr other)
        {
            return _name == other._name;
        }
        
        public override int GetHashCode()
        {
            return _id;
        }

        public override string ToString()
        {
            return Type + " " + Name;
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
