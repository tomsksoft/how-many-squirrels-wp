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
 * Created by Nadyrshin Stanislav on 26.06.2014
 */

using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    [Table]
    public class AppInfo : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _itemId;
        private string _parametr;
        private Tab _lastTab;
        private GraphView _graphView;

        public AppInfo()
        {
            Parametr  = "Белка";
            LastTab   = Tab.Main;
            GraphView = GraphView.Column;
        }

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
        public string Parametr     
        {
            get { return _parametr; }
            set
            {
                if(value == _parametr) return;

                NotifyPropertyChanging("Parametr");
                _parametr = value;
                NotifyPropertyChanged("Parametr");
            }
        }

        [Column]
        public Tab LastTab         
        {
            get { return _lastTab; }
            set
            {
                if(value == _lastTab) return;

                NotifyPropertyChanging("LastTab");
                _lastTab = value;
                NotifyPropertyChanged("LastTab");
            }
        }
        
        [Column]
        public GraphView GraphView 
        {
            get { return _graphView; }
            set
            {
                if(value == _graphView) return;

                NotifyPropertyChanging("GraphView");
                _graphView = value;
                NotifyPropertyChanged("GraphView");
            }
        }

        [Column(IsVersion = true)]
        private Binary _version;

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

    public enum Tab { Main=0, List=1, Grpah=2 }
    public enum GraphView { Column }

}
