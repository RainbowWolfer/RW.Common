using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace RW.Common.WPF.Helpers;

public static class DataGridHelper {
	private static readonly FieldInfo _selectionAnchorField = typeof(DataGrid).GetField("_selectionAnchor", BindingFlags.Instance | BindingFlags.NonPublic)!;
	private static readonly FieldInfo _focusedInfoField = typeof(ItemsControl).GetField("_focusedInfo", BindingFlags.Instance | BindingFlags.NonPublic)!;
	private static readonly PropertyInfo FocusedCellProperty = typeof(DataGrid).GetProperty("FocusedCell", BindingFlags.Instance | BindingFlags.NonPublic)!;

	/// <summary>
	/// https://github.com/dotnet/wpf/issues/6983
	/// </summary>
	public static void FixDataGridClearingLeak(this DataGrid dataGrid) {
		IEnumerable items = dataGrid.ItemsSource;
		if (items is not null) {
			ICollectionView view = CollectionViewSource.GetDefaultView(items);
			view.CollectionChanged += (s, e) => {
				if (view.IsEmpty) {
					_selectionAnchorField.SetValue(dataGrid, null);
					_focusedInfoField.SetValue(dataGrid, null);
					FocusedCellProperty.SetValue(dataGrid, null);

					dataGrid.CurrentItem = null;
					dataGrid.CurrentCell = default;
					dataGrid.CurrentColumn = null;
				}
			};
		}
	}
}
