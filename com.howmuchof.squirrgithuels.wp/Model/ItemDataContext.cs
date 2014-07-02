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
 * Created by Nadyrshin Stanislav on 20.04.2014
 */

using System.Data.Linq;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    public class ItemDataContext : DataContext
    {
        private const string DbConnectionString = "Data Source=isostore:/HowHuchOf.sdf";
        public ItemDataContext() : base(DbConnectionString) { }

        public Table<DataItem> DataItems;
        public Table<Parametr> Parametrs;
    }

    
}
