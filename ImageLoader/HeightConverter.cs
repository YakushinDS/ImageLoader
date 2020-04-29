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
    public class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {                
                double height = (double)value;

                if (height != 0)
                    return height - SystemParameters.HorizontalScrollBarHeight; // Вычитаем ширину скроллбара, чтобы он не появлялся
                else
                    return 300;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message, "WidthConverter");
                return 300;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
