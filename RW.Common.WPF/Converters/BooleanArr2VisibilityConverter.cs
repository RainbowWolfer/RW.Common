﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class BooleanArr2VisibilityConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		if (values == null) {
			return Visibility.Collapsed;
		}

		var arr = new List<bool>();
		foreach (var item in values) {
			if (item is bool boolValue) {
				arr.Add(boolValue);
			} else {
				return Visibility.Collapsed;
			}
		}
		return arr.All(item => item) ? Visibility.Visible : Visibility.Collapsed;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
