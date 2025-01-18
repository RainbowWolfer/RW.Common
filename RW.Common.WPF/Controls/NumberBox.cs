using RW.Common.WPF.Commands;
using RW.Common.WPF.Events;
using RW.Common.WPF.Helpers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace RW.Common.WPF.Controls;

[TemplatePart(Name = Part_UpButton, Type = typeof(Button))]
[TemplatePart(Name = Part_DownButton, Type = typeof(Button))]
public partial class NumberBox : TextBox {
	private const string Part_UpButton = "Part_UpButton";
	private const string Part_DownButton = "Part_DownButton";

	static NumberBox() {
		DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(typeof(NumberBox)));
		MaxLengthProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(17));
		AllowDropProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(false));
	}

	public NumberBox() {
		CommandBindings.Add(new CommandBinding(WpfCommands.Next, (s, e) => {
			Focus();
			if (IsReadOnly) {
				return;
			}
			Value += Increment;
			SetText(true);
			e.Handled = true;
		}));

		CommandBindings.Add(new CommandBinding(WpfCommands.Previous, (s, e) => {
			Focus();
			if (IsReadOnly) {
				return;
			}
			Value -= Increment;
			SetText(true);
			e.Handled = true;
		}));

		CommandBindings.Add(new CommandBinding(WpfCommands.Clear, (s, e) => {
			Focus();
			if (IsReadOnly) {
				return;
			}
			SetCurrentValue(ValueProperty, 0d);
			SetText(true);
			e.Handled = true;
		}));

		DataObject.AddPastingHandler(this, DataObjectPasting);
		DataObject.AddCopyingHandler(this, DataObjectCopying);
	}

	~NumberBox() {
		DataObject.RemovePastingHandler(this, DataObjectPasting);
		DataObject.RemoveCopyingHandler(this, DataObjectCopying);
	}

	protected Button? UpButton { get; private set; }
	protected Button? DownButton { get; private set; }

	public override void OnApplyTemplate() {
		base.OnApplyTemplate();

		UpButton = GetTemplateChild(Part_UpButton) as Button;
		DownButton = GetTemplateChild(Part_DownButton) as Button;

		Text = CurrentText;
	}

	protected override void OnPreviewKeyDown(KeyEventArgs e) {
		base.OnPreviewKeyDown(e);
		if (e.Key == Key.Space) {
			e.Handled = true;
		}

		if (IsReadOnly) {
			return;
		}

		if (e.Key == Key.Up) {
			Value += Increment;
			SetText(true);
			e.Handled = true;
		} else if (e.Key == Key.Down) {
			Value -= Increment;
			SetText(true);
			e.Handled = true;
		}
	}

	//override drag

	protected override void OnTextChanged(TextChangedEventArgs e) {
		base.OnTextChanged(e);
		if (double.TryParse(Text, out double value)) {
			if (value >= Minimum && value <= Maximum) {
				SetCurrentValue(ValueProperty, value);
			}
		}
	}

	protected override void OnLostFocus(RoutedEventArgs e) {
		base.OnLostFocus(e);
		if (string.IsNullOrWhiteSpace(Text)) {
			SetCurrentValue(ValueProperty, 0d);
		} else if (double.TryParse(Text, out double value)) {
			SetCurrentValue(ValueProperty, value);
		} else {
			SetText(true);
		}
	}

	protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e) {
		base.OnLostKeyboardFocus(e);
		if (string.IsNullOrWhiteSpace(Text)) {
			SetCurrentValue(ValueProperty, 0d);
		} else if (double.TryParse(Text, out double value)) {
			SetCurrentValue(ValueProperty, value);
		} else {
			SetText(true);
		}
	}

	[GeneratedRegex("[^0-9.-]+")]
	private static partial Regex DisallowedTextRegex();

	private static readonly Regex _regex = DisallowedTextRegex(); //regex that matches disallowed text
	protected override void OnPreviewTextInput(TextCompositionEventArgs e) {
		base.OnPreviewTextInput(e);
		e.Handled = _regex.IsMatch(e.Text) /*&& double.TryParse(wholeText, out double _)*/;
	}

	protected override void OnTextInput(TextCompositionEventArgs e) {
		base.OnTextInput(e);
	}

	protected override void OnMouseDoubleClick(MouseButtonEventArgs e) {
		base.OnMouseDoubleClick(e);

		if (e.Source is Visual visual && ViewHelper.FindVisualParent<ButtonBase>(visual) != null) {
			return;
		}
		SelectAll();
		e.Handled = true;
	}

	protected virtual void DataObjectPasting(object sender, DataObjectPastingEventArgs e) {
		if (e.IsDragDrop) {
			e.CancelCommand();
			return;
		}
		if (e.DataObject.GetDataPresent(typeof(string))) {
			string text = (string)e.DataObject.GetData(typeof(string));
			text = text.Trim();
			if (_regex.IsMatch(text)) {
				e.CancelCommand();
			}
		} else {
			e.CancelCommand();
		}
	}

	private void DataObjectCopying(object sender, DataObjectCopyingEventArgs e) {
		if (e.IsDragDrop) {
			e.CancelCommand();
		}
	}

	protected override void OnMouseWheel(MouseWheelEventArgs e) {
		base.OnMouseWheel(e);
		if (IsFocused && !IsReadOnly) {
			Value += e.Delta > 0 ? Increment : -Increment;
			SetText(true);
			e.Handled = true;
		}
	}


	private string CurrentText {
		get {
			if (string.IsNullOrWhiteSpace(ValueFormat)) {
				if (DecimalPlaces.HasValue) {
					return Value.ToString($"#0.{new string('0', DecimalPlaces.Value)}");
				} else {
					return Value.ToString("R");
				}
			} else {
				return Value.ToString(ValueFormat);
			}
		}
	}


	protected virtual void OnValueChanged(PayloadRoutedEventArgs<double> e) => RaiseEvent(e);

	/// <summary>
	///     值改变事件
	/// </summary>
	public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
		"ValueChanged",
		RoutingStrategy.Bubble,
		typeof(EventHandler<PayloadRoutedEventArgs<double>>),
		typeof(NumberBox)
	);

	/// <summary>
	///     值改变事件
	/// </summary>
	public event EventHandler<PayloadRoutedEventArgs<double>> ValueChanged {
		add => AddHandler(ValueChangedEvent, value);
		remove => RemoveHandler(ValueChangedEvent, value);
	}

	/// <summary>
	///     当前值
	/// </summary>
	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
		nameof(Value),
		typeof(double),
		typeof(NumberBox),
		new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, CoerceValue),
		IsInRangeOfDouble
	);

	private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		NumberBox ctl = (NumberBox)d;
		double v = (double)e.NewValue;
		ctl.SetText();

		ctl.OnValueChanged(new PayloadRoutedEventArgs<double>(ValueChangedEvent, ctl, v));
	}

	private void SetText(bool force = false) {
		if (!IsFocused || force) {
			Text = CurrentText;
			Select(Text.Length, 0);
		}
	}

	private static object CoerceValue(DependencyObject d, object baseValue) {
		NumberBox ctl = (NumberBox)d;
		double minimum = ctl.Minimum;
		double num = (double)baseValue;
		if (num < minimum) {
			ctl.Value = minimum;
			return minimum;
		}
		double maximum = ctl.Maximum;
		if (num > maximum) {
			ctl.Value = maximum;
		}
		ctl.SetText();
		return num > maximum ? maximum : num;
	}

	/// <summary>
	///     当前值
	/// </summary>
	public double Value {
		get => (double)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	/// <summary>
	///     最大值
	/// </summary>
	public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
		nameof(Maximum),
		typeof(double),
		typeof(NumberBox),
		new PropertyMetadata(double.MaxValue, OnMaximumChanged, CoerceMaximum),
		IsInRangeOfDouble
	);

	private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		NumberBox ctl = (NumberBox)d;
		ctl.CoerceValue(MinimumProperty);
		ctl.CoerceValue(ValueProperty);
	}

	private static object CoerceMaximum(DependencyObject d, object baseValue) {
		double minimum = ((NumberBox)d).Minimum;
		return (double)baseValue < minimum ? minimum : baseValue;
	}

	/// <summary>
	///     最大值
	/// </summary>
	public double Maximum {
		get => (double)GetValue(MaximumProperty);
		set => SetValue(MaximumProperty, value);
	}

	/// <summary>
	///     最小值
	/// </summary>
	public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
		nameof(Minimum),
		typeof(double),
		typeof(NumberBox),
		new PropertyMetadata(double.MinValue, OnMinimumChanged, CoerceMinimum),
		IsInRangeOfDouble
	);

	private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		NumberBox ctl = (NumberBox)d;
		ctl.CoerceValue(MaximumProperty);
		ctl.CoerceValue(ValueProperty);
	}

	private static object CoerceMinimum(DependencyObject d, object baseValue) {
		double maximum = ((NumberBox)d).Maximum;
		return (double)baseValue > maximum ? maximum : baseValue;
	}

	/// <summary>
	///     最小值
	/// </summary>
	public double Minimum {
		get => (double)GetValue(MinimumProperty);
		set => SetValue(MinimumProperty, value);
	}

	/// <summary>
	///     指示每单击一下按钮时增加或减少的数量
	/// </summary>
	public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
		nameof(Increment),
		typeof(double),
		typeof(NumberBox),
		new PropertyMetadata(1d)
	);

	/// <summary>
	///     指示每单击一下按钮时增加或减少的数量
	/// </summary>
	public double Increment {
		get => (double)GetValue(IncrementProperty);
		set => SetValue(IncrementProperty, value);
	}

	/// <summary>
	///     指示要显示的小数位数
	/// </summary>
	public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(
		nameof(DecimalPlaces),
		typeof(int?),
		typeof(NumberBox),
		new PropertyMetadata(default(int?))
	);

	/// <summary>
	///     指示要显示的小数位数
	/// </summary>
	public int? DecimalPlaces {
		get => (int?)GetValue(DecimalPlacesProperty);
		set => SetValue(DecimalPlacesProperty, value);
	}

	/// <summary>
	///     指示要显示的数字的格式
	/// </summary>
	public static readonly DependencyProperty ValueFormatProperty = DependencyProperty.Register(
		nameof(ValueFormat), typeof(string), typeof(NumberBox), new PropertyMetadata(default(string)));

	/// <summary>
	///     指示要显示的数字的格式，这将会覆盖 <see cref="DecimalPlaces"/> 属性
	/// </summary>
	public string ValueFormat {
		get => (string)GetValue(ValueFormatProperty);
		set => SetValue(ValueFormatProperty, value);
	}

	/// <summary>
	///     是否显示上下调值按钮
	/// </summary>
	public static readonly DependencyProperty ShowUpDownButtonProperty = DependencyProperty.Register(
		nameof(ShowUpDownButton),
		typeof(bool),
		typeof(NumberBox),
		new PropertyMetadata(true)
	);

	/// <summary>
	///     是否显示上下调值按钮
	/// </summary>
	public bool ShowUpDownButton {
		get => (bool)GetValue(ShowUpDownButtonProperty);
		set => SetValue(ShowUpDownButtonProperty, value);
	}


	private static bool IsInRangeOfDouble(object value) {
		double v = (double)value;
		return !(double.IsNaN(v) || double.IsInfinity(v));
	}

}
