using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RW.Common.WPF.Converters;

public abstract class BoolToValueConverter<T> : DependencyObject, IValueConverter {
	public T TrueValue {
		get => (T)GetValue(TrueValueProperty);
		set => SetValue(TrueValueProperty, value);
	}

	public T FalseValue {
		get => (T)GetValue(FalseValueProperty);
		set => SetValue(FalseValueProperty, value);
	}

	public static readonly DependencyProperty TrueValueProperty = DependencyProperty.Register(
		nameof(TrueValue),
		typeof(T),
		typeof(BoolToValueConverter<T>),
		new PropertyMetadata(default(T))
	);

	public static readonly DependencyProperty FalseValueProperty = DependencyProperty.Register(
		nameof(FalseValue),
		typeof(T),
		typeof(BoolToValueConverter<T>),
		new PropertyMetadata(default(T))
	);

	public BoolToValueConverter() {

	}

	public BoolToValueConverter(T trueValue, T falseValue) {
		TrueValue = trueValue;
		FalseValue = falseValue;
	}

	public virtual object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is bool b) {
			return b ? TrueValue : FalseValue;
		}
		return value;
	}

	public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}
}


public class BoolToStringConverter : BoolToValueConverter<string> { }
public class BoolToNumberConverter : BoolToValueConverter<double> { }
public class BoolToBrushConverter : BoolToValueConverter<Brush> { }
public class BoolToDoubleCollectionConverter : BoolToValueConverter<DoubleCollection> { }
public class BoolToThicknessConverter : BoolToValueConverter<Thickness> { }
public class BoolToCornerRadiusConverter : BoolToValueConverter<CornerRadius> { }
