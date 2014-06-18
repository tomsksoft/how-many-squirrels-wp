using System;
using GalaSoft.MvvmLight;

namespace com.howmuchof.squirrgithuels.wp.ViewModel
{
    public class AddViewModel : ViewModelBase
    {
        public AddViewModel()
        {
            Date = DateTime.Now;
        }

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
    }
}