using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using RW.Common.Helpers;

namespace RW.Common.WPF.Converters;

public class TreeViewItemMarginConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		double interval;

		if (parameter is double d) {
			interval = d;
		} else if (parameter is string s && s.IsNotBlank() && double.TryParse(s, out d)) {
			interval = d;
		} else {
			interval = 19;
		}

		double left = 0.0;
		UIElement? element = value as TreeViewItem;
		while (element != null && element.GetType() != typeof(TreeView)) {
			element = (UIElement)VisualTreeHelper.GetParent(element);
			if (element is TreeViewItem) {
				left += interval;
			}
		}
		return new Thickness(left, 0, 0, 0);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
