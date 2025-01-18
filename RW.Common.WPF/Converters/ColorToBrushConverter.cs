using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Drawing = System.Drawing;

namespace RW.Common.WPF.Converters;

public class ColorToBrushConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		Color defC = Colors.White;
		if (value is Color c1) {
			defC = Color.FromRgb(c1.R, c1.G, c1.B);
		} else if (value is Drawing.Color c2) {
			defC = Color.FromRgb(c2.R, c2.G, c2.B);
		}
		return new SolidColorBrush(defC);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is SolidColorBrush brush) {
			if (parameter?.ToString() == "Drawing") {
				return Drawing.Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
			} else {
				return brush.Color;
			}
		} else {
			return Colors.Black;
		}
	}
}
