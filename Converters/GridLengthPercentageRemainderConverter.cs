using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TeslaCamTheater.Converters
{
    [ValueConversion(typeof(double), typeof(GridLength))]
    public class GridLengthPercentageRemainderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            return new GridLength((int)(Math.Abs(1 - val) * 100), GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (GridLength)value;
        }
    }
}
