using System;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            //var item = new DataItem(2, DateTime.Now, DateTime.Now);
            //callback(item, null);
        }
    }
}