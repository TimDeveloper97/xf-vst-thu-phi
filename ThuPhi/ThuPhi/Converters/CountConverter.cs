using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ThuPhi.Model.Receive;
using Xamarin.Forms;

namespace ThuPhi.Converters
{
    class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isPay = bool.Parse((string)parameter);
            var users = value as List<Info>;

            if (users == null || parameter == null) return null;

            if(isPay)
            {
                return users.Where(x => long.Parse(x.Pay.Replace(",","")) != 0).Count();
            }   
            else
            {
                return users.Where(x => long.Parse(x.Pay.Replace(",", "")) == 0).Count();
            }    
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
