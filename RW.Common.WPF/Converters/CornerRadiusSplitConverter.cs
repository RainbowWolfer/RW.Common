﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class CornerRadiusSplitConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is CornerRadius cornerRadius) {
			if (parameter is string str) {
				string[] arr = str.Split(',');
				if (arr.Length != 4) {
					return cornerRadius;
				}

				return new CornerRadius(
					arr[0].Equals("1") ? cornerRadius.TopLeft : 0,
					arr[1].Equals("1") ? cornerRadius.TopRight : 0,
					arr[2].Equals("1") ? cornerRadius.BottomRight : 0,
					arr[3].Equals("1") ? cornerRadius.BottomLeft : 0
				);
			}
		}
		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
