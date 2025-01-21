using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RW.Common.WPF.Extensions;

public static class ScrollViewerExtension {

	public static bool GetDefaultWheelScrollHorizontally(DependencyObject obj) {
		return (bool)obj.GetValue(DefaultWheelScrollHorizontallyProperty);
	}

	public static void SetDefaultWheelScrollHorizontally(DependencyObject obj, bool value) {
		obj.SetValue(DefaultWheelScrollHorizontallyProperty, value);
	}

	public static readonly DependencyProperty DefaultWheelScrollHorizontallyProperty = DependencyProperty.RegisterAttached(
		"DefaultWheelScrollHorizontally",
		typeof(bool),
		typeof(ScrollViewerExtension),
		new PropertyMetadata(false, OnDefaultWheelScrollHorizontallyChanged)
	);

	private static void OnDefaultWheelScrollHorizontallyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (d is not UIElement element) {
			throw new Exception("Attached property must be used with UIElement.");
		}

		if ((bool)e.NewValue) {
			element.PreviewMouseWheel += OnPreviewMouseWheel2;
		} else {
			element.PreviewMouseWheel -= OnPreviewMouseWheel2;
		}
	}

	private static void OnPreviewMouseWheel2(object sender, MouseWheelEventArgs args) {
		ScrollViewer? scrollViewer = (sender as UIElement)?.FindDescendant<ScrollViewer>();

		if (scrollViewer == null) {
			return;
		}

		int count = 2;

		for (int i = 0; i < count; i++) {
			if (args.Delta < 0) {
				scrollViewer.LineRight();
			} else {
				scrollViewer.LineLeft();
			}
		}

		args.Handled = true;
	}

	private static T? FindDescendant<T>(this DependencyObject d) where T : DependencyObject {
		if (d == null) {
			return null;
		}

		if (d is T t) {
			return t;
		}

		int childCount = VisualTreeHelper.GetChildrenCount(d);

		for (int i = 0; i < childCount; i++) {
			DependencyObject child = VisualTreeHelper.GetChild(d, i);

			T? result = child as T ?? FindDescendant<T>(child);

			if (result != null) {
				return result;
			}
		}

		return null;
	}


}
