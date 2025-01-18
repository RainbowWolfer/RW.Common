using RW.Common.Helpers;
using System.Diagnostics;
using System.Windows.Media;
using Drawing = System.Drawing;

namespace RW.Common.WPF.Helpers;

public static class ViewColorHelper {
	public static SolidColorBrush ToColorBrush(this string hex) {
		return new SolidColorBrush(hex.ToColor());
	}

	public static Color ToColor(this string hex) {
		Color color;
		hex = hex.Replace("#", string.Empty);
		if (hex.Length == 6) {
			byte r = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
			byte g = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
			byte b = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
			color = Color.FromArgb(byte.MaxValue, r, g, b);
		} else if (hex.Length == 8) {
			byte a = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
			byte r = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
			byte g = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
			byte b = (byte)Convert.ToUInt32(hex.Substring(6, 2), 16);
			color = Color.FromArgb(a, r, g, b);
		} else {
			Debug.WriteLine($"COLOR CONVERT ERROR - {hex}");
			return Colors.Black;
		}
		return color;
	}

	public static Color Lerp(Color a, Color b, double t) {
		t = t.Clamp(0, 1);
		return new Color() {
			R = (byte)(a.R + ((b.R - a.R) * t)),
			G = (byte)(a.G + ((b.G - a.G) * t)),
			B = (byte)(a.B + ((b.B - a.B) * t)),
			A = (byte)(a.A + ((b.A - a.A) * t))
		};
	}

	public static Drawing.Color Lerp(Drawing.Color a, Drawing.Color b, double t) {
		t = t.Clamp(0, 1);
		return Drawing.Color.FromArgb(
			(byte)(a.A + ((b.A - a.A) * t)),
			(byte)(a.R + ((b.R - a.R) * t)),
			(byte)(a.G + ((b.G - a.G) * t)),
			(byte)(a.B + ((b.B - a.B) * t))
		);
	}

	public static Color ToMediaColor(this Drawing.Color color) {
		return new Color() {
			R = color.R,
			G = color.G,
			B = color.B,
			A = color.A,
		};
	}

	public static Drawing.Color ToDrawingColor(this Color color) {
		return Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
	}

	public static double CalculateContrastGray(byte r, byte g, byte b) {
		double gray = (0.299 * r) + (0.587 * g) + (0.114 * b);
		return gray;
	}

	public static Brush GetContrastColorBrush(Color backgroundColor, Brush? dark = null, Brush? light = null) {
		dark ??= Brushes.Black;
		light ??= Brushes.White;

		double gray = CalculateContrastGray(backgroundColor.R, backgroundColor.G, backgroundColor.B);

		if (gray >= 128) {
			return dark;
		} else {
			return light;
		}
	}

	public static Brush GetContrastColorBrush(Drawing.Color backgroundColor, Brush? dark = null, Brush? light = null) {
		dark ??= Brushes.Black;
		light ??= Brushes.White;

		double gray = CalculateContrastGray(backgroundColor.R, backgroundColor.G, backgroundColor.B);

		if (gray >= 128) {
			return dark;
		} else {
			return light;
		}
	}

	public static Drawing.Color GetContrastColor(Drawing.Color backgroundColor, Drawing.Color? dark = null, Drawing.Color? light = null) {
		Drawing.Color _dark = dark ?? Drawing.Color.Black;
		Drawing.Color _light = light ?? Drawing.Color.White;

		double gray = CalculateContrastGray(backgroundColor.R, backgroundColor.G, backgroundColor.B);

		if (gray >= 128) {
			return _dark;
		} else {
			return _light;
		}
	}


	public static Color GetContrastColor(Color backgroundColor, Color? dark = null, Color? light = null) {
		Color _dark = dark ?? Colors.Black;
		Color _light = light ?? Colors.White;

		double gray = CalculateContrastGray(backgroundColor.R, backgroundColor.G, backgroundColor.B);

		if (gray >= 128) {
			return _dark;
		} else {
			return _light;
		}
	}


}
