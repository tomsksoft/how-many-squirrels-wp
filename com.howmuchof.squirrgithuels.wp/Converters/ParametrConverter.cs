using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using com.howmuchof.squirrgithuels.wp.Model;

namespace com.howmuchof.squirrgithuels.wp.Converters
{
    public class ParametrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "Текущий параметр: " + ((Parametr) value).Name;// + " тип:" + ((Parametr)value).TypeName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
