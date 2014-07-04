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

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    [Table]
    public class Parametr : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _id;
        private string _name;
        private string _type;

        public Parametr()
        {
            _items = new EntitySet<DataItem>(AttachItem, DetachItem); 
            Type = "int";
            Name = "Белка";
        }

        public Parametr(string name, string type)
        {
            _items = new EntitySet<DataItem>(AttachItem, DetachItem); 
            Type = type;
            Name = name;
        }

        public Parametr(string name, IEnumerable<string> enumList)
        {
            _items = new EntitySet<DataItem>(AttachItem, DetachItem);
            Type = "enum[";
            Name = name;

            foreach (var i in enumList)
                Type += i + ";";
            Type += "]";
        }

        //[Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
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
        public string Type 
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


        [Column(IsVersion = true)]
        private Binary _version;

        // Define the entity set for the collection side of the relationship.
        private readonly EntitySet<DataItem> _items;


        [Association(Storage = "_items", OtherKey = "ParametrId", ThisKey = "Id")]
        public EntitySet<DataItem> Items
        {
            get { return _items; }
            set { _items.Assign(value); }
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

        public override string ToString()
        {
            return Type + " " + Name;
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
