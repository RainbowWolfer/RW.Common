using RW.Common.WPF.Helpers;
using System.Globalization;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class IListToVisibilityConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		return IListToBoolConverter.Handle(value, parameter).ToVisibility();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}

public class IListToVisibilityReConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		return (!IListToBoolConverter.Handle(value, parameter)).ToVisibility();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
