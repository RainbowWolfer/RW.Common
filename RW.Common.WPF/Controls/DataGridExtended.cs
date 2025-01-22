using RW.Common.WPF.Helpers;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;

namespace RW.Common.WPF.Controls;

public class DataGridExtended : DataGrid {
	private static readonly FieldInfo s_isDraggingSelectionField = typeof(DataGrid).GetField("_isDraggingSelection", BindingFlags.Instance | BindingFlags.NonPublic)!;

	private static readonly MethodInfo s_endDraggingMethod = typeof(DataGrid).GetMethod("EndDragging", BindingFlags.Instance | BindingFlags.NonPublic)!;

	// DataGrid.OnMouseMove() serves no other purpose than to execute click-drag-selection.
	// Bypass that, and stop 'is dragging selection' mode for DataGrid
	protected override void OnMouseMove(MouseEventArgs e) {
		if ((bool)(s_isDraggingSelectionField?.GetValue(this) ?? false)) {
			s_endDraggingMethod.Invoke(this, []);
		}
	}

	public DataGridExtended() {
		this.FixDataGridClearingLeak();
	}
}
