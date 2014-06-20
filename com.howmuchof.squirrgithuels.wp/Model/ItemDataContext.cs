using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    public class ItemDataContext : DataContext
    {
        private const string DbConnectionString = "Data Source=isostore:/ToDo.sdf";
        public ItemDataContext() : base(DbConnectionString) { }

        public Table<DataItem> DataItems;

    }

    
}
