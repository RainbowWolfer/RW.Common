using RW.Common.WPF.Helpers;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Drawing = System.Drawing;

namespace RW.Common.WPF.Converters;

public class ContrastBrushConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is SolidColorBrush brush) {
			return ViewColorHelper.GetContrastColorBrush(brush.Color);
		} else if (value is Drawing.Color color1) {
			return ViewColorHelper.GetContrastColorBrush(color1);
		} else if (value is Color color2) {
			return ViewColorHelper.GetContrastColorBrush(color2);
		}
		return Brushes.Black;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}
