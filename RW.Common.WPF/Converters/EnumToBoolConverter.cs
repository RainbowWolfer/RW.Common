using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class EnumToBoolConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is Enum e && parameter is Enum p) {
			if (e.Equals(p)) {
				return true;
			} else {
				return false;
			}
		}

		return false;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		if (parameter is not Enum @enum) {
			return DependencyProperty.UnsetValue;
		}
		return @enum;
	}
}

public class EnumToBoolReConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is Enum e && parameter is Enum p) {
			if (e.Equals(p)) {
				return false;
			} else {
				return true;
			}
		}

		return true;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		if (parameter is not Enum @enum) {
			return DependencyProperty.UnsetValue;
		}
		return @enum;
	}
}