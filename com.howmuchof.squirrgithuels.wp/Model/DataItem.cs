using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    public class DataItem
    {
        public DataItem(int count, DateTime date)
        {
            Count = count;
            Date = date;
        }

        public int Count
        {
            get;
            private set;
        }

        public DateTime Date
        {
            get; 
            private set;
        }
    }
}
