using System.Data.Linq;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    public class ItemDataContext : DataContext
    {
        private const string DbConnectionString = "Data Source=isostore:/HowHuchOf.sdf";
        public ItemDataContext() : base(DbConnectionString) { }

        public Table<DataItem> DataItems;

    }

    
}
