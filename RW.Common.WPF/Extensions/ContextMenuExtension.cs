using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RW.Common.WPF.Extensions;

public static class ContextMenuExtension {


	public static ICommand? GetOpenedCommand(DependencyObject? obj) {
		return obj?.GetValue(OpenedCommandProperty) as ICommand;
	}

	public static void SetOpenedCommand(DependencyObject obj, ICommand value) {
		obj.SetValue(OpenedCommandProperty, value);
	}

	public static readonly DependencyProperty OpenedCommandProperty = DependencyProperty.RegisterAttached(
		"OpenedCommand",
		typeof(ICommand),
		typeof(ContextMenuExtension),
		new PropertyMetadata(null, OnOpenedCommandChanged)
	);

	private static void OnOpenedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is not ContextMenu contextMenu) {
			throw new Exception("Attached property must be used with ContextMenu.");
		}
		if (e.NewValue is ICommand command) {
			contextMenu.Opened += ContextMenu_Opened;
		} else {
			contextMenu.Opened -= ContextMenu_Opened;
		}
	}

	private static void ContextMenu_Opened(object sender, RoutedEventArgs e) {
		GetOpenedCommand(sender as DependencyObject)?.Execute(sender);
	}



	public static ICommand? GetClosedCommand(DependencyObject? obj) {
		return obj?.GetValue(ClosedCommandProperty) as ICommand;
	}

	public static void SetClosedCommand(DependencyObject obj, ICommand value) {
		obj.SetValue(ClosedCommandProperty, value);
	}

	public static readonly DependencyProperty ClosedCommandProperty = DependencyProperty.RegisterAttached(
		"ClosedCommand",
		typeof(ICommand),
		typeof(ContextMenuExtension),
		new PropertyMetadata(null, OnClosedCommandChanged)
	);

	private static void OnClosedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is not ContextMenu contextMenu) {
			throw new Exception("Attached property must be used with ContextMenu.");
		}
		if (e.NewValue is ICommand command) {
			contextMenu.Closed += ContextMenu_Closed;
		} else {
			contextMenu.Closed -= ContextMenu_Closed;
		}
	}

	private static void ContextMenu_Closed(object sender, RoutedEventArgs e) {
		GetClosedCommand(sender as DependencyObject)?.Execute(sender);
	}








	private static DependencyObject? GetAssociatedContextMenuTarget(DependencyObject? obj) {
		return obj?.GetValue(AssociatedContextMenuTargetProperty) as DependencyObject;
	}

	private static void SetAssociatedContextMenuTarget(DependencyObject obj, DependencyObject? value) {
		obj.SetValue(AssociatedContextMenuTargetProperty, value);
	}

	// 设置 AssociatedContextMenu 的那个
	private static readonly DependencyProperty AssociatedContextMenuTargetProperty = DependencyProperty.RegisterAttached(
		"AssociatedContextMenuTarget",
		typeof(DependencyObject),
		typeof(ContextMenuExtension),
		new PropertyMetadata(null)
	);



	public static ContextMenu GetAssociatedContextMenu(DependencyObject obj) => (ContextMenu)obj.GetValue(AssociatedContextMenuProperty);

	public static void SetAssociatedContextMenu(DependencyObject obj, ContextMenu value) => obj.SetValue(AssociatedContextMenuProperty, value);

	// 关联 ContextMenu IsOpen 状态，从 IsAssociatedMenuOpen 获取
	public static readonly DependencyProperty AssociatedContextMenuProperty = DependencyProperty.RegisterAttached(
		"AssociatedContextMenu",
		typeof(ContextMenu),
		typeof(ContextMenuExtension),
		new PropertyMetadata(null, OnAssociatedContextMenuChanged)
	);

	private static void OnAssociatedContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (e.OldValue is ContextMenu oldMenu) {
			SetAssociatedContextMenuTarget(oldMenu, null);
			oldMenu.RemoveIsOpen(HandleContextMenuIsOpen);
		}

		if (e.NewValue is ContextMenu newMenu) {
			newMenu.AttachIsOpen(HandleContextMenuIsOpen);
			if (d is FrameworkElement fe) {
				SetAssociatedContextMenuTarget(newMenu, fe);
			}

		}

	}

	private static void HandleContextMenuIsOpen(object? sender, EventArgs e) {
		if (sender is ContextMenu contextMenu) {
			DependencyObject? obj = GetAssociatedContextMenuTarget(contextMenu);
			SetIsAssociatedMenuOpen(obj, contextMenu.IsOpen);
		}
	}



	public static bool GetIsAssociatedMenuOpen(DependencyObject obj) => (bool)obj.GetValue(IsAssociatedMenuOpenProperty);

	private static void SetIsAssociatedMenuOpen(DependencyObject? obj, bool value) => obj?.SetValue(IsAssociatedMenuOpenPropertyKey, value);

	private static readonly DependencyPropertyKey IsAssociatedMenuOpenPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
		"IsAssociatedMenuOpen",
		typeof(bool),
		typeof(ContextMenuExtension),
		new PropertyMetadata(false)
	);

	public static readonly DependencyProperty IsAssociatedMenuOpenProperty = IsAssociatedMenuOpenPropertyKey.DependencyProperty;


}