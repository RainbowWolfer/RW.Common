﻿using System.Globalization;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class Object2BooleanConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		return value is not null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
