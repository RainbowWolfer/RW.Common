using RW.Common.WPF.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace RW.Common.WPF.Extensions;

public static class ControlExtension {

	public static ICommand? GetLoadedCommand(DependencyObject? obj) => obj?.GetValue(LoadedCommandProperty) as ICommand;

	public static void SetLoadedCommand(DependencyObject obj, ICommand value) => obj.SetValue(LoadedCommandProperty, value);

	public static readonly DependencyProperty LoadedCommandProperty = DependencyProperty.RegisterAttached(
		"LoadedCommand",
		typeof(ICommand),
		typeof(ControlExtension),
		new PropertyMetadata(null, OnLoadedCommandChanged)
	);


	private static void OnLoadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is FrameworkElement frameworkElement) {
			if (e.OldValue is ICommand) {
				frameworkElement.Loaded -= FrameworkElement_Loaded;
			}
			if (e.NewValue is ICommand newCommand) {
				frameworkElement.Loaded += FrameworkElement_Loaded;
			}
		}
	}

	private static void FrameworkElement_Loaded(object sender, RoutedEventArgs e) {
		FrameworkElement? frameworkElement = sender as FrameworkElement;
		GetLoadedCommand(frameworkElement)?.Execute(frameworkElement);
	}



	public static ICommand GetLoadedOnceCommand(DependencyObject obj) => (ICommand)obj.GetValue(LoadedOnceCommandProperty);

	public static void SetLoadedOnceCommand(DependencyObject obj, ICommand value) => obj.SetValue(LoadedOnceCommandProperty, value);

	public static readonly DependencyProperty LoadedOnceCommandProperty = DependencyProperty.RegisterAttached(
		"LoadedOnceCommand",
		typeof(ICommand),
		typeof(ControlExtension),
		new PropertyMetadata(null, OnLoadedOnceCommand)
	);

	private static void OnLoadedOnceCommand(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is FrameworkElement frameworkElement) {
			if (e.OldValue is ICommand) {
				frameworkElement.Loaded -= FrameworkElement_LoadedOnce;
			}
			if (e.NewValue is ICommand newCommand) {
				frameworkElement.Loaded += FrameworkElement_LoadedOnce;
			}
		}
	}

	private static void FrameworkElement_LoadedOnce(object sender, RoutedEventArgs e) {
		if (sender is FrameworkElement frameworkElement) {
			frameworkElement.Loaded -= FrameworkElement_LoadedOnce;
			GetLoadedCommand(frameworkElement)?.Execute(frameworkElement);
		}
	}

	public static bool GetIgnoreButtonBaseInDoubleClick(DependencyObject obj) => (bool)obj.GetValue(IgnoreButtonBaseInDoubleClickProperty);

	public static void SetIgnoreButtonBaseInDoubleClick(DependencyObject? obj, bool value) => obj?.SetValue(IgnoreButtonBaseInDoubleClickProperty, value);

	public static readonly DependencyProperty IgnoreButtonBaseInDoubleClickProperty = DependencyProperty.RegisterAttached(
		"IgnoreButtonBaseInDoubleClick",
		typeof(bool),
		typeof(ControlExtension),
		new PropertyMetadata(true)
	);


	public static ICommand? GetDoubleClickCommand(DependencyObject? obj) {
		return obj?.GetValue(DoubleClickCommandProperty) as ICommand;
	}

	public static void SetDoubleClickCommand(DependencyObject obj, ICommand value) {
		obj.SetValue(DoubleClickCommandProperty, value);
	}

	public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached(
		"DoubleClickCommand",
		typeof(ICommand),
		typeof(ControlExtension),
		new PropertyMetadata(null, OnDoubleClickCommandChanged)
	);

	private static void OnDoubleClickCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is Control control) {
			if (e.NewValue is ICommand command) {
				control.MouseDoubleClick += Control_MouseDoubleClick;
			} else {
				control.MouseDoubleClick -= Control_MouseDoubleClick;
			}
		}
	}

	private static void Control_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
		if (e.ChangedButton != MouseButton.Left) {
			return;
		}
		DependencyObject? obj = sender as DependencyObject;

		// don't trigger Command when click on buttons
		if (obj != null && GetIgnoreButtonBaseInDoubleClick(obj)) {
			if (e.OriginalSource is Visual visual) {
				ButtonBase? button = ViewHelper.FindVisualParent<ButtonBase>(visual);
				if (button != null) {
					return;
				}
			}
		}

		object args = GetDoubleClickCommandParameter(obj) ?? e;
		GetDoubleClickCommand(obj)?.Execute(args);
		e.Handled = true;
	}



	public static object? GetDoubleClickCommandParameter(DependencyObject? obj) {
		return obj?.GetValue(DoubleClickCommandParameterProperty);
	}

	public static void SetDoubleClickCommandParameter(DependencyObject obj, object value) {
		obj.SetValue(DoubleClickCommandParameterProperty, value);
	}

	public static readonly DependencyProperty DoubleClickCommandParameterProperty = DependencyProperty.RegisterAttached(
		"DoubleClickCommandParameter",
		typeof(object),
		typeof(ControlExtension),
		new PropertyMetadata(null)
	);


	public static ICommand? GetPreviewDoubleClickCommand(DependencyObject? obj) {
		return obj?.GetValue(PreviewDoubleClickCommandProperty) as ICommand;
	}

	public static void SetPreviewDoubleClickCommand(DependencyObject obj, ICommand value) {
		obj.SetValue(PreviewDoubleClickCommandProperty, value);
	}

	public static readonly DependencyProperty PreviewDoubleClickCommandProperty = DependencyProperty.RegisterAttached(
		"PreviewDoubleClickCommand",
		typeof(ICommand),
		typeof(ControlExtension),
		new PropertyMetadata(null, OnPreviewDoubleClickCommandChanged)
	);

	private static void OnPreviewDoubleClickCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is Control control) {
			if (e.NewValue is ICommand command) {
				control.PreviewMouseDoubleClick += Control_PreviewMouseDoubleClick;
			} else {
				control.PreviewMouseDoubleClick -= Control_PreviewMouseDoubleClick;
			}
		}
	}

	private static void Control_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) {
		if (e.ChangedButton != MouseButton.Left) {
			return;
		}
		GetPreviewDoubleClickCommand(sender as DependencyObject)?.Execute(e);
	}

	public static ICommand? GetDropCommand(DependencyObject? obj) {
		return obj?.GetValue(DropCommandProperty) as ICommand;
	}

	public static void SetDropCommand(DependencyObject obj, ICommand value) {
		obj.SetValue(DropCommandProperty, value);
	}

	public static readonly DependencyProperty DropCommandProperty = DependencyProperty.RegisterAttached(
		"DropCommand",
		typeof(ICommand),
		typeof(ControlExtension),
		new PropertyMetadata(null, OnDropCommandChanged)
	);

	private static void OnDropCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is UIElement control) {
			if (e.NewValue is ICommand command) {
				control.Drop += Control_Drop;
			} else {
				control.Drop -= Control_Drop;
			}
		}
	}

	private static void Control_Drop(object sender, DragEventArgs e) {
		GetDropCommand(sender as DependencyObject)?.Execute(e);
	}






	public static object GetMouseDownCommandParameter(DependencyObject obj) => obj.GetValue(MouseDownCommandParameterProperty);

	public static void SetMouseDownCommandParameter(DependencyObject obj, object value) => obj.SetValue(MouseDownCommandParameterProperty, value);

	public static readonly DependencyProperty MouseDownCommandParameterProperty = DependencyProperty.RegisterAttached(
		"MouseDownCommandParameter",
		typeof(object),
		typeof(ControlExtension),
		new PropertyMetadata(null)
	);




	public static ICommand GetMouseDownCommand(DependencyObject obj) {
		return (ICommand)obj.GetValue(MouseDownCommandProperty);
	}

	public static void SetMouseDownCommand(DependencyObject obj, ICommand value) {
		obj.SetValue(MouseDownCommandProperty, value);
	}

	public static readonly DependencyProperty MouseDownCommandProperty = DependencyProperty.RegisterAttached(
		"MouseDownCommand",
		typeof(ICommand),
		typeof(ControlExtension),
		new PropertyMetadata(null, OnMouseDownCommandChanged)
	);

	private static void OnMouseDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is UIElement control) {
			if (e.NewValue is ICommand command) {
				control.MouseDown += Control_MouseDown;
			} else {
				control.MouseDown -= Control_MouseDown;
			}
		}
	}

	private static void Control_MouseDown(object sender, MouseButtonEventArgs e) {
		if (sender is DependencyObject obj) {
			object args = GetMouseDownCommandParameter(obj) ?? e;
			GetMouseDownCommand(obj)?.Execute(args);
			e.Handled = true;
		}
	}


	public static ICommand GetKeyDownCommand(DependencyObject obj) {
		return (ICommand)obj.GetValue(KeyDownCommandProperty);
	}

	public static void SetKeyDownCommand(DependencyObject obj, ICommand value) {
		obj.SetValue(KeyDownCommandProperty, value);
	}

	public static readonly DependencyProperty KeyDownCommandProperty = DependencyProperty.RegisterAttached(
		"KeyDownCommand",
		typeof(ICommand),
		typeof(ControlExtension),
		new PropertyMetadata(null, OnKeyDownCommandChanged)
	);

	private static void OnKeyDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is UIElement control) {
			if (e.NewValue is ICommand command) {
				control.KeyDown += Control_KeyDown;
			} else {
				control.KeyDown -= Control_KeyDown;
			}
		}
	}

	private static void Control_KeyDown(object sender, KeyEventArgs e) {
		//object parameter = GetKeyDownCommandParameter(sender as DependencyObject);
		if (sender is DependencyObject obj) {
			GetKeyDownCommand(obj)?.Execute(/*parameter ?? */e);
		}
	}

	//public static object GetKeyDownCommandParameter(DependencyObject obj) {
	//	return obj.GetValue(KeyDownCommandParameterProperty);
	//}

	//public static void SetKeyDownCommandParameter(DependencyObject obj, object value) {
	//	obj.SetValue(KeyDownCommandParameterProperty, value);
	//}

	//public static readonly DependencyProperty KeyDownCommandParameterProperty = DependencyProperty.RegisterAttached(
	//	"KeyDownCommandParameter",
	//	typeof(object),
	//	typeof(ControlExtensions),
	//	new PropertyMetadata(null)
	//);




	public static ICommand GetSizeChangedCommand(DependencyObject obj) => (ICommand)obj.GetValue(SizeChangedCommandProperty);

	public static void SetSizeChangedCommand(DependencyObject obj, ICommand value) => obj.SetValue(SizeChangedCommandProperty, value);

	public static readonly DependencyProperty SizeChangedCommandProperty = DependencyProperty.RegisterAttached(
		"SizeChangedCommand",
		typeof(ICommand),
		typeof(ControlExtension),
		new PropertyMetadata(null, OnSizeChangedCommandChanged)
	);

	private static void OnSizeChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is FrameworkElement element) {
			if (e.OldValue is ICommand) {
				element.SizeChanged -= Element_SizeChanged;
			}
			if (e.NewValue is ICommand) {
				element.SizeChanged += Element_SizeChanged;
			}
		}
	}

	private static void Element_SizeChanged(object sender, SizeChangedEventArgs e) {
		if (sender is DependencyObject obj) {
			GetSizeChangedCommand(obj)?.Execute(e);
		}
	}
}

