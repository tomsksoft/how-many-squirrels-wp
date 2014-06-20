using System;
using System.Windows.Input;
using com.howmuchof.squirrgithuels.wp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace com.howmuchof.squirrgithuels.wp.ViewModel
{
    public class AddViewModel : ViewModelBase
    {
        public AddViewModel()
        {
            Date = DateTime.Now;
        }

        private int      _count;
        private DateTime _date;

        public DateTime Date  
        {
            get
            {
                return _date;
            }
            set
            {
                if (value == _date)
                    return;

                _date = value;
                RaisePropertyChanged("Date");
            }
        }
        public int      Count 
        {
            get { return _count; }
            set
            {
                if (_count == value) return;

                _count = value;
                RaisePropertyChanged("Count");
            }
        }

        #region 

        public void AddItem()
        {
            //ItemDataContext.GetDataContext().DataItems.InsertOnSubmit(new DataItem(Count, Date, Date));
        }
        
        #endregion
        
    }
}