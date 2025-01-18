using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;


[ValueConversion(sourceType: typeof(bool), targetType: typeof(Visibility))]
public class Boolean2VisibilityReConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is bool b && b) {
			return Visibility.Collapsed;
		} else {
			return Visibility.Visible;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
