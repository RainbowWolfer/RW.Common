using RW.Common.Helpers;
using System.Globalization;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class String2BooleanConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value == null) {
			return false;
		}
		return value.ToString().IsNotBlank();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}

public class String2BooleanReConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value == null) {
			return true;
		}
		return value.ToString().IsBlank();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
