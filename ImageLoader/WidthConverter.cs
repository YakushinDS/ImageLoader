using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ImageLoader
{
    public class WidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int count = (int)values[0];
                double totalWidth = (double)values[1];

                if (count != 0)
                    return (totalWidth - SystemParameters.VerticalScrollBarWidth) / count; // Вычитаем ширину скроллбара, чтобы он не появлялся
                else
                    return 300;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message, "WidthConverter");
                return 300;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
