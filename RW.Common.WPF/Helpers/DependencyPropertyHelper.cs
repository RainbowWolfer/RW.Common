using System.Windows;

namespace RW.Common.WPF.Helpers;

public static class DependencyPropertyHelper {

	public static void InitializeValue(
		this DependencyObject sender,
		DependencyProperty dependencyProperty,
		Action<DependencyObject, DependencyPropertyChangedEventArgs> func
	) {
		func.Invoke(sender, new DependencyPropertyChangedEventArgs(
			dependencyProperty,
			DependencyProperty.UnsetValue,
			sender.GetValue(dependencyProperty)
		));
	}

}
