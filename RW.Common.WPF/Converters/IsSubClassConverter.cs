using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

[ValueConversion(typeof(Type), typeof(bool), ParameterType = typeof(Type))]
public class IsSubClassConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		return IsSubClass(value, parameter);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}

	public static bool IsSubClass(object value, object parameter) {
		if (value is null || parameter is not Type inType) {
			return false;
		}

		Type type;
		if (value is Type _type) {
			type = _type;
		} else {
			type = value.GetType();
		}

		if (type.IsSubclassOf(inType)) {
			return true;
		} else if (inType.IsAssignableFrom(type)) {
			return true;
		}

		return false;
	}
}

[ValueConversion(typeof(Type), typeof(Visibility), ParameterType = typeof(Type))]
public class IsSubClassToVisibilityConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (IsSubClassConverter.IsSubClass(value, parameter)) {
			return Visibility.Visible;
		} else {
			return Visibility.Collapsed;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}

}
