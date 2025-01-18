using System.Globalization;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class BooleanArr2BooleanConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		if (values == null) {
			return false;
		}

		List<bool> arr = [];
		foreach (object item in values) {
			if (item is bool boolValue) {
				arr.Add(boolValue);
			} else {
				return false;
			}
		}

		return arr.All(item => item);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
