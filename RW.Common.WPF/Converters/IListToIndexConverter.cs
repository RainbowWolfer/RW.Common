using RW.Common.Helpers;
using System.Collections;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace RW.Common.WPF.Converters;

public class IListToIndexConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		object? collection = values.ElementAtOrDefault(0);
		object? item = values.ElementAtOrDefault(1);

		int resultIndex = -1;

		if (collection is ItemCollection itemCollection) {
			int index = itemCollection.IndexOf(item);
			resultIndex = index + 1;
		}

		if (collection is IList list) {
			int index = list.IndexOf(item);
			resultIndex = index + 1;
		}

		if (resultIndex > 0 && parameter?.ToString() == "A") {
			return resultIndex.IndexToColumn();
		} else {
			return resultIndex.ToString();
		}

	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}