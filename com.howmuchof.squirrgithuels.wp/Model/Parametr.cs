using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    [Table]
    public class Parametr : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _parametrId;
        private string _name;
        private string _type;

        public Parametr()
        {
            _items = new EntitySet<DataItem>(); //TODO возможно стоит добавить обработчики событий
            Type = "int";
            Name = "Белка";
        }

        public Parametr(string name, string type)
        {
            _items = new EntitySet<DataItem>();
            Type = type;
            Name = name;
        }

        //[Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ParametrId
        {
            get
            {
                return _parametrId;
            }
            set
            {
                if (_parametrId == value) return;

                NotifyPropertyChanging("ParametrId");
                _parametrId = value;
                NotifyPropertyChanged("ParametrId");
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


        [Association(Storage = "_items", OtherKey = "ParametrId", ThisKey = "ParametrId")]
        public EntitySet<DataItem> Items
        {
            get { return _items; }
            set { _items.Assign(value); }
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
