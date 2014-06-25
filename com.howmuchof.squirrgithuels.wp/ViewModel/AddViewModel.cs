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
 * Created by Nadyrshin Stanislav on 18.04.2014
 */

using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace com.howmuchof.squirrgithuels.wp.ViewModel
{
    public class AddViewModel : ViewModelBase
    {
        public AddViewModel()
        {
            Count = 1;
            Date = DateTime.Now;
            var t = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            t.Tick += delegate { if (!IsSelfTime) Date = DateTime.Now; };
            t.Start();
        }

        private bool     _isSelfTime;
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

                _count = value < 1 ? 1 : value;
                RaisePropertyChanged("Count");
            }
        }

        public bool IsSelfTime
        {
            get { return _isSelfTime; }
            set
            {
                if(value == _isSelfTime)
                    return;

                _isSelfTime = value;
                RaisePropertyChanged("IsSelfTime");
            }
        }

        #region MinusCommand

        private RelayCommand _minusCommand;

        public ICommand MinusCommand
        {
            get
            {
                if (_minusCommand == null)
                    return (_minusCommand = new RelayCommand(Minus));
                return _minusCommand;
            }
        }

        private void Minus()
        {
            Count--;
        }

        #endregion

        #region PlusCommand

        private RelayCommand _plusCommand;

        public ICommand PlusCommand
        {
            get
            {
                if (_plusCommand == null)
                    return (_plusCommand = new RelayCommand(Plus));
                return _plusCommand;
            }
        }

        private void Plus()
        {
            Count++;
        }

        #endregion

    }
}