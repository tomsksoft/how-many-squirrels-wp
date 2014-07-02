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
            _value = value;
            if(enumList.Contains(value)) throw new Exception("Перечисление не содержит этого эдемента");
        }

        public List<string> EnumList { get; set; }
    }
}
