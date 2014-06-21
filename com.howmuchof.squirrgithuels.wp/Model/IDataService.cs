using System;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}
