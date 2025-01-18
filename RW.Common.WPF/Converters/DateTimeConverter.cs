using RW.Common.Helpers;
using System.Globalization;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class DateTimeConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is DateTime dateTime) {
			if (parameter is string format && format.IsNotBlank()) {
				return dateTime.ToString(format);
			} else {
				return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
			}
		}

		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
