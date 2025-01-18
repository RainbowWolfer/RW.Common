using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class VisibilityToVisibilityReConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is Visibility visibility) {
			return visibility switch {
				Visibility.Visible => Visibility.Collapsed,
				Visibility.Hidden => Visibility.Visible,
				Visibility.Collapsed => Visibility.Visible,
				_ => throw new NotImplementedException(),
			};
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
