using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class Boolean2VisibilityConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		bool flag = false;
		if (value is bool b) {
			flag = b;
		}

		if (flag) {
			return Visibility.Visible;
		} else {
			return Visibility.Collapsed;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		return value switch {
			Visibility visibility => visibility == Visibility.Visible,
			_ => false
		};
	}
}
