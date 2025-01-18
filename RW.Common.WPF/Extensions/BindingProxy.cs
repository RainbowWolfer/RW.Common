using System.Windows;

namespace RW.Common.WPF.Extensions;

public abstract class BindingProxy<T> : Freezable {
	protected override Freezable CreateInstanceCore() {
		return new BindingProxy();
	}

	public T Data {
		get => (T)GetValue(DataProperty);
		set => SetValue(DataProperty, value);
	}

	public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
		nameof(Data),
		typeof(T),
		typeof(BindingProxy),
		new UIPropertyMetadata(default(T))
	);

}

public class BindingProxy : BindingProxy<object?>;
public class BooleanProxy : BindingProxy<bool>;
