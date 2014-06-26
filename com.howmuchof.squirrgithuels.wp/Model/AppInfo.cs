using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    [Table]
    public class AppInfo : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _parametr;
        private Tab _lastTab;
        private GraphView _graphView;

        public AppInfo()
        {
            Parametr = "Белка";
            LastTab = Tab.Main;
            GraphView = GraphView.Column;
        }

        [Column(IsPrimaryKey = true)]
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
