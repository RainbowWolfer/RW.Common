using RW.Common.Helpers;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class IListToBoolConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		return Handle(value, parameter);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}

	public static bool Handle(object value, object parameter) {
		if (value is null) {
			return false;
		}
		string p = parameter.SafeToString().ToLower().Trim();
		int count = value switch {
			ICollection list => list.Count,
			IEnumerable ie => ie.CountIEnumerable(),
			int _count => _count,
			_ => 0,
		};
		if (Compare(p, IListConverterParameters.Only1)) {
			return count == 1;
		} else if (Compare(p, IListConverterParameters.MoreThan1)) {
			return count > 1;
		} else {
			return count != 0;
		}
	}

	private static bool Compare(string a, string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

}

public class IListToBoolReConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		return !IListToBoolConverter.Handle(value, parameter);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}

public static class IListConverterParameters {
	/// <summary>只在长度为1时触发</summary>
	public static string Only1 { get; } = "Only1";
	/// <summary>只在长度大于1时触发</summary>
	public static string MoreThan1 { get; } = "MoreThan1";
}
