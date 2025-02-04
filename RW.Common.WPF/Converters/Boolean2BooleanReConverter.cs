﻿using System.Globalization;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class Boolean2BooleanReConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is bool boolValue) {
			return !boolValue;
		}
		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is bool boolValue) {
			return !boolValue;
		}
		return value;
	}
}
