using RW.Common.Helpers;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class IListToCountConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		return value switch {
			ICollection list => list.Count,
			IEnumerable ie => ie.CountIEnumerable(),
			int _count => _count,
			_ => -1
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
