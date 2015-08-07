using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DomainSetBidsApplication.Models;

namespace DomainSetBidsApplication.Converters
{
    [ValueConversion(typeof(RegDomainState), typeof(Brushes))]
    public sealed class RegDomainModeToBrushesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = (RegDomainState)value;

            switch (type)
            {
                case RegDomainState.Cancel:
                    return Brushes.Red;
                case RegDomainState.Draft:
                    return Brushes.DarkSlateGray;
                case RegDomainState.Done:
                    return Brushes.Green;
                case RegDomainState.Pending:
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
