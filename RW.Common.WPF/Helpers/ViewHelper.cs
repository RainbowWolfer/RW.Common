using RW.Common.Helpers;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RW.Common.WPF.Helpers;

public static class ViewHelper {
	public static bool IsInDesignerMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

	public static Visibility ToVisibility(this bool b, bool reverse = false) {
		if (reverse) {
			return b ? Visibility.Collapsed : Visibility.Visible;
		} else {
			return b ? Visibility.Visible : Visibility.Collapsed;
		}
	}

	public static Visibility ToVisibilityByHidden(this bool b, bool reverse = false) {
		if (reverse) {
			return b ? Visibility.Hidden : Visibility.Visible;
		} else {
			return b ? Visibility.Visible : Visibility.Hidden;
		}
	}

	public static void CopyToClipboard(this string? text) {
		if (text.IsBlank()) {
			return;
		}
		try {
			Clipboard.SetText(text);
		} catch (Exception e1) {
			Debug.WriteLine(e1);
			try {
				Clipboard.SetDataObject(text, true);
			} catch (Exception e2) {
				Debug.WriteLine(e2);
			}
		}
	}

	public static int Count(this ICollectionView view) {
		if (view is CollectionView cv) {
			return cv.Count;
		}
		if (view is ICollection collection) {
			return collection.Count;
		}
		return view.Cast<object>().Count();
	}

	public static DependencyObject? FindVisualParent(DependencyObject? child, Predicate<DependencyObject> predicate) {
		if (child == null) {
			return null;
		}
		DependencyObject parent = child;
		while (parent != null) {
			if (predicate(parent)) {
				return parent;
			}
			parent = VisualTreeHelper.GetParent(parent);
		}
		return null;
	}

	public static T? FindVisualParent<T>(this DependencyObject? obj) where T : DependencyObject {
		return FindVisualParent(obj, o => o is T) as T;
	}

	public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject? depObj) where T : DependencyObject {
		if (depObj == null) {
			yield break;
		}
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
			DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

			if (child is not null and T t) {
				yield return t;
			}

			foreach (T childOfChild in FindVisualChildren<T>(child)) {
				yield return childOfChild;
			}
		}
	}

}
