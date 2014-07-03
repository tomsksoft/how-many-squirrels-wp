using System;
using System.Collections.Generic;

namespace com.howmuchof.squirrgithuels.wp.Model
{
    class EnumItem
    {
        private string _value;

        public EnumItem(List<string> enumList, string value)
        {
            EnumList = enumList;
            Value = value;
            if(enumList.Contains(value)) throw new Exception("Перечисление не содержит этого эдемента");
        }

        public List<string> EnumList { get; private set; }
        public string Value
        {
            get { return _value; }
            set
            {
                if (!EnumList.Contains(value)) throw new Exception("Перечисление не содержит этого эдемента");
                _value = value;
            }
        }
    }
}
