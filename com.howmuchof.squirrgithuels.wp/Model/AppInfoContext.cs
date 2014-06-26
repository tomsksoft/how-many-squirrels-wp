using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    class AppInfoContext : DataContext
    {
        private const string DbConnectionString = "Data Source=isostore:/Settings.sdf";
        public AppInfoContext() : base(DbConnectionString) { }

        public Table<AppInfo> AppInfo;
    }
}
