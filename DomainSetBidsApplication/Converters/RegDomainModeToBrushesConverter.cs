using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DomainSetBidsApplication.Models;

namespace DomainSetBidsApplication.Converters
{
    [ValueConversion(typeof(RegDomainMode), typeof(Brushes))]
    public sealed class RegDomainModeToBrushesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = (RegDomainMode)value;

            switch (type)
            {
                case RegDomainMode.Cancel:
                    return Brushes.Red;
                case RegDomainMode.Draft:
                    return Brushes.DarkSlateGray;
                case RegDomainMode.Done:
                    return Brushes.Green;
                case RegDomainMode.Pending:
                    return Brushes.SandyBrown;
            }

            return Brushes.Blue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
