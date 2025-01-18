using RW.Common.Helpers;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class DescriptionConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		try {
			return value.GetDescription();
		} catch (Exception ex) {
			Debug.WriteLine(ex);
			return value;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
