using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RoutedNavigation.Converters
{
    public class FontConverters
    {
        #region Converters

        /// <summary>
        /// Gives a Normal <see cref="FontWeight"/> for true and an ExtraLight <see cref="FontWeight"/> for false
        /// </summary>
        [ValueConversion(typeof(bool), typeof(FontWeight))]
        public class BoolToFontWeightConverter : DependencyObject, IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return ((bool)value) ? FontWeights.DemiBold : FontWeights.ExtraLight;
            }

            public object ConvertBack(object value, Type targetType,
                                      object parameter, CultureInfo culture)
            {
                return Binding.DoNothing;
            }
        }

        #endregion

        /// <summary>
        /// Get a bool to FontWeight converter
        /// </summary>
        public static BoolToFontWeightConverter FontWeight => new BoolToFontWeightConverter();
    }
}
