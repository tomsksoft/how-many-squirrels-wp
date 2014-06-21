using System.Collections.ObjectModel;
using System.Linq;
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
            DataItems.Add(item);
            DataItems = DataItems;
        }

        public void DeleteItem(DataItem item)
        {
            using (var db = new ItemDataContext())
            {
                db.DataItems.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            DataItems.Remove(item);
        }

        public MainViewModel()
        {
            DataItems = new ObservableCollection<DataItem>();   
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}