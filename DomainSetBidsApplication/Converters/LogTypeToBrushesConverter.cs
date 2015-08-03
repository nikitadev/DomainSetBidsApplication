using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DomainSetBidsApplication.Models;

namespace DomainSetBidsApplication.Converters
{
    [ValueConversion(typeof(LogType), typeof(Brushes))]
    public sealed class LogTypeToBrushesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = (LogType)value;

            switch (type)
            {
                case LogType.Error:
                    return Brushes.DarkRed;
                case LogType.Info:
                    return Brushes.Black;
                case LogType.Success:
                    return Brushes.DarkGreen;
                case LogType.Warning:
                    return Brushes.DarkOrange;
            }

            return Brushes.DarkBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
