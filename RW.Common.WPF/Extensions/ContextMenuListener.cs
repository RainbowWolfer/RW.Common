using System.ComponentModel;
using System.Windows.Controls;

namespace RW.Common.WPF.Extensions;

public static class ContextMenuListener {
	private static readonly DependencyPropertyDescriptor isOpen = DependencyPropertyDescriptor.FromProperty(
		ContextMenu.IsOpenProperty,
		typeof(ContextMenu)
	);

	public static void AttachIsOpen(this ContextMenu contextMenu, EventHandler eventHandler) {
		isOpen.AddValueChanged(contextMenu, eventHandler);
	}

	public static void RemoveIsOpen(this ContextMenu contextMenu, EventHandler eventHandler) {
		isOpen.RemoveValueChanged(contextMenu, eventHandler);
	}

}
