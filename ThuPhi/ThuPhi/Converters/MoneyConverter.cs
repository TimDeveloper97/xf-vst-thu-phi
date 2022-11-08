using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ThuPhi.Converters
{
    class MoneyConverter : IValueConverter
    {
        CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");

        public string DatetimeToString(string money)
        {
            var result = double.Parse(money).ToString("#,###", cul.NumberFormat);
            return result;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return DatetimeToString(value.ToString());
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
