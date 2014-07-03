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

using System.Data.Linq;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    class AppInfoContext : DataContext
    {
        private const string DbConnectionString = "Data Source=isostore:/Settings.sdf";
        public AppInfoContext() : base(DbConnectionString) { }

        public Table<AppInfo> AppInfo;
    }
}
