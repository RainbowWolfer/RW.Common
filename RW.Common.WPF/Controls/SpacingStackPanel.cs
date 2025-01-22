using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RW.Common.WPF.Controls;

public class SpacingStackPanel : Panel {
	private Orientation orientation = Orientation.Horizontal;

	public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(
		typeof(SpacingStackPanel),
		new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, OnOrientationChanged)
	);

	private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		SpacingStackPanel p = (SpacingStackPanel)d;
		p.orientation = (Orientation)e.NewValue;
	}

	public Orientation Orientation {
		get => (Orientation)GetValue(OrientationProperty);
		set => SetValue(OrientationProperty, value);
	}

	public static readonly DependencyProperty ChildWrappingProperty = DependencyProperty.Register(
		nameof(ChildWrapping),
		typeof(bool),
		typeof(SpacingStackPanel),
		new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure)
	);

	public bool ChildWrapping {
		get => (bool)GetValue(ChildWrappingProperty);
		set => SetValue(ChildWrappingProperty, value);
	}

	public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
		nameof(Spacing),
		typeof(double),
		typeof(SpacingStackPanel),
		new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure),
		IsSpacingValid
	);

	public double Spacing {
		get => (double)GetValue(SpacingProperty);
		set => SetValue(SpacingProperty, value);
	}

	public static readonly DependencyProperty HorizontalSpacingProperty = DependencyProperty.Register(
		nameof(HorizontalSpacing),
		typeof(double),
		typeof(SpacingStackPanel),
		new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure),
		IsSpacingValid
	);

	public double HorizontalSpacing {
		get => (double)GetValue(HorizontalSpacingProperty);
		set => SetValue(HorizontalSpacingProperty, value);
	}

	public static readonly DependencyProperty VerticalSpacingProperty = DependencyProperty.Register(
		nameof(VerticalSpacing),
		typeof(double),
		typeof(SpacingStackPanel),
		new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure),
		IsSpacingValid
	);

	public double VerticalSpacing {
		get => (double)GetValue(VerticalSpacingProperty);
		set => SetValue(VerticalSpacingProperty, value);
	}

	public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
		nameof(ItemWidth),
		typeof(double),
		typeof(SpacingStackPanel),
		new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure),
		IsWidthHeightValid
	);

	[TypeConverter(typeof(LengthConverter))]
	public double ItemWidth {
		get => (double)GetValue(ItemWidthProperty);
		set => SetValue(ItemWidthProperty, value);
	}

	public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
		nameof(ItemHeight),
		typeof(double),
		typeof(SpacingStackPanel),
		new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure),
		IsWidthHeightValid
	);

	[TypeConverter(typeof(LengthConverter))]
	public double ItemHeight {
		get => (double)GetValue(ItemHeightProperty);
		set => SetValue(ItemHeightProperty, value);
	}

	public static readonly DependencyProperty ItemHorizontalAlignmentProperty = DependencyProperty.Register(
		nameof(ItemHorizontalAlignment),
		typeof(HorizontalAlignment?),
		typeof(SpacingStackPanel),
		new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure)
	);

	public HorizontalAlignment? ItemHorizontalAlignment {
		get => (HorizontalAlignment?)GetValue(ItemHorizontalAlignmentProperty);
		set => SetValue(ItemHorizontalAlignmentProperty, value);
	}

	public static readonly DependencyProperty ItemVerticalAlignmentProperty = DependencyProperty.Register(
		nameof(ItemVerticalAlignment),
		typeof(VerticalAlignment?),
		typeof(SpacingStackPanel),
		new FrameworkPropertyMetadata(VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure)
	);

	public VerticalAlignment? ItemVerticalAlignment {
		get => (VerticalAlignment?)GetValue(ItemVerticalAlignmentProperty);
		set => SetValue(ItemVerticalAlignmentProperty, value);
	}

	private static bool IsWidthHeightValid(object value) {
		double v = (double)value;
		return double.IsNaN(v) || (v >= 0.0d && !double.IsPositiveInfinity(v));
	}

	private static bool IsSpacingValid(object value) {
		if (value is double spacing) {
			return double.IsNaN(spacing) || spacing > 0;
		}

		return false;
	}

	private void ArrangeWrapLine(double v, double lineV, int start, int end, bool useItemU, double itemU, double spacing) {
		double u = 0;
		bool isHorizontal = orientation == Orientation.Horizontal;

		UIElementCollection children = InternalChildren;
		for (int i = start; i < end; i++) {
			UIElement child = children[i];
			if (child == null) {
				continue;
			}

			PanelUvSize childSize = new(orientation, child.DesiredSize);
			double layoutSlotU = useItemU ? itemU : childSize.U;

			child.Arrange(isHorizontal ? new Rect(u, v, layoutSlotU, lineV) : new Rect(v, u, lineV, layoutSlotU));

			if (layoutSlotU > 0) {
				u += layoutSlotU + spacing;
			}
		}
	}

	private void ArrangeLine(double lineV, bool useItemU, double itemU, double spacing) {
		double u = 0;
		bool isHorizontal = orientation == Orientation.Horizontal;

		UIElementCollection children = InternalChildren;
		for (int i = 0; i < children.Count; i++) {
			UIElement child = children[i];
			if (child == null) {
				continue;
			}

			PanelUvSize childSize = new(orientation, child.DesiredSize);
			double layoutSlotU = useItemU ? itemU : childSize.U;

			child.Arrange(isHorizontal ? new Rect(u, 0, layoutSlotU, lineV) : new Rect(0, u, lineV, layoutSlotU));

			if (layoutSlotU > 0) {
				u += layoutSlotU + spacing;
			}
		}
	}

	protected override Size MeasureOverride(Size constraint) {
		PanelUvSize curLineSize = new(orientation);
		PanelUvSize panelSize = new(orientation);
		PanelUvSize uvConstraint = new(orientation, constraint);
		double itemWidth = ItemWidth;
		double itemHeight = ItemHeight;
		bool itemWidthSet = !double.IsNaN(itemWidth);
		bool itemHeightSet = !double.IsNaN(itemHeight);
		bool childWrapping = ChildWrapping;
		HorizontalAlignment? itemHorizontalAlignment = ItemHorizontalAlignment;
		VerticalAlignment? itemVerticalAlignment = ItemVerticalAlignment;
		bool itemHorizontalAlignmentSet = itemHorizontalAlignment != null;
		bool itemVerticalAlignmentSet = itemVerticalAlignment != null;
		PanelUvSize spacingSize = GetSpacingSize();

		Size childConstraint = new(
			itemWidthSet ? itemWidth : constraint.Width,
			itemHeightSet ? itemHeight : constraint.Height
		);

		UIElementCollection children = InternalChildren;
		bool isFirst = true;


		if (childWrapping == false) // no wrap
		{
			Size layoutSlotSize = constraint;

			if (orientation == Orientation.Horizontal) {
				layoutSlotSize.Width = double.PositiveInfinity;
			} else {
				layoutSlotSize.Height = double.PositiveInfinity;
			}

			for (int i = 0, count = children.Count; i < count; ++i) {
				UIElement child = children[i];
				if (child == null) {
					continue;
				}

				if (itemHorizontalAlignmentSet) {
					child.SetCurrentValue(HorizontalAlignmentProperty, itemHorizontalAlignment);
				}

				if (itemVerticalAlignmentSet) {
					child.SetCurrentValue(VerticalAlignmentProperty, itemVerticalAlignment);
				}

				child.Measure(layoutSlotSize);

				PanelUvSize sz = new(
					orientation,
					itemWidthSet ? itemWidth : child.DesiredSize.Width,
					itemHeightSet ? itemHeight : child.DesiredSize.Height
				);

				curLineSize.U += isFirst ? sz.U : sz.U + spacingSize.U;
				curLineSize.V = Math.Max(sz.V, curLineSize.V);

				isFirst = false;
			}
		} else {
			for (int i = 0, count = children.Count; i < count; i++) {
				UIElement child = children[i];
				if (child == null) {
					continue;
				}

				if (itemHorizontalAlignmentSet) {
					child.SetCurrentValue(HorizontalAlignmentProperty, itemHorizontalAlignment);
				}

				if (itemVerticalAlignmentSet) {
					child.SetCurrentValue(VerticalAlignmentProperty, itemVerticalAlignment);
				}

				child.Measure(childConstraint);

				PanelUvSize sz = new(
					orientation,
					itemWidthSet ? itemWidth : child.DesiredSize.Width,
					itemHeightSet ? itemHeight : child.DesiredSize.Height
				);

				if (!isFirst && MathHelper.GreaterThan(curLineSize.U + sz.U + spacingSize.U, uvConstraint.U)) {
					panelSize.U = Math.Max(curLineSize.U, panelSize.U);
					panelSize.V += curLineSize.V + spacingSize.V;
					curLineSize = sz;
				} else {
					curLineSize.U += isFirst ? sz.U : sz.U + spacingSize.U;
					curLineSize.V = Math.Max(sz.V, curLineSize.V);
				}

				isFirst = false;
			}
		}

		panelSize.U = Math.Max(curLineSize.U, panelSize.U);
		panelSize.V += curLineSize.V;

		return new Size(panelSize.Width, panelSize.Height);
	}

	private PanelUvSize GetSpacingSize() {
		double spacing = Spacing;

		if (!double.IsNaN(spacing)) {
			return new PanelUvSize(orientation, spacing, spacing);
		}

		double horizontalSpacing = HorizontalSpacing;
		if (double.IsNaN(horizontalSpacing)) {
			horizontalSpacing = 0;
		}

		double verticalSpacing = VerticalSpacing;
		if (double.IsNaN(verticalSpacing)) {
			verticalSpacing = 0;
		}

		return new PanelUvSize(orientation, horizontalSpacing, verticalSpacing);
	}

	protected override Size ArrangeOverride(Size finalSize) {
		int firstInLine = 0;
		double itemWidth = ItemWidth;
		double itemHeight = ItemHeight;
		double accumulatedV = 0;
		double itemU = orientation == Orientation.Horizontal ? itemWidth : itemHeight;
		PanelUvSize curLineSize = new(orientation);
		PanelUvSize uvFinalSize = new(orientation, finalSize);
		bool itemWidthSet = !double.IsNaN(itemWidth);
		bool itemHeightSet = !double.IsNaN(itemHeight);
		bool useItemU = orientation == Orientation.Horizontal ? itemWidthSet : itemHeightSet;
		bool childWrapping = ChildWrapping;
		PanelUvSize spacingSize = GetSpacingSize();

		UIElementCollection children = InternalChildren;
		bool isFirst = true;

		if (childWrapping == false) // no wrap
		{
			ArrangeLine(uvFinalSize.V, useItemU, itemU, spacingSize.U);
		} else {
			for (int i = 0, count = children.Count; i < count; i++) {
				UIElement child = children[i];
				if (child == null) {
					continue;
				}

				PanelUvSize sz = new(
					orientation,
					itemWidthSet ? itemWidth : child.DesiredSize.Width,
					itemHeightSet ? itemHeight : child.DesiredSize.Height
				);

				if (!isFirst && MathHelper.GreaterThan(curLineSize.U + sz.U + spacingSize.U, uvFinalSize.U)) {
					ArrangeWrapLine(accumulatedV, curLineSize.V, firstInLine, i, useItemU, itemU, spacingSize.U);

					accumulatedV += curLineSize.V + spacingSize.V;
					curLineSize = sz;

					firstInLine = i;
				} else {
					curLineSize.U += isFirst ? sz.U : sz.U + spacingSize.U;
					curLineSize.V = Math.Max(sz.V, curLineSize.V);
				}

				isFirst = false;
			}

			if (firstInLine < children.Count) {
				ArrangeWrapLine(accumulatedV, curLineSize.V, firstInLine, children.Count, useItemU, itemU, spacingSize.U);
			}
		}

		return finalSize;
	}

	private struct PanelUvSize {
		private readonly Orientation orientation;

		public readonly Size ScreenSize => new(U, V);

		public double U { get; set; }

		public double V { get; set; }

		public double Width {
			readonly get => orientation == Orientation.Horizontal ? U : V;
			private set {
				if (orientation == Orientation.Horizontal) {
					U = value;
				} else {
					V = value;
				}
			}
		}

		public double Height {
			readonly get => orientation == Orientation.Horizontal ? V : U;
			private set {
				if (orientation == Orientation.Horizontal) {
					V = value;
				} else {
					U = value;
				}
			}
		}

		public PanelUvSize(Orientation orientation, double width, double height) {
			U = V = 0d;
			this.orientation = orientation;
			Width = width;
			Height = height;
		}

		public PanelUvSize(Orientation orientation, Size size) {
			U = V = 0d;
			this.orientation = orientation;
			Width = size.Width;
			Height = size.Height;
		}

		public PanelUvSize(Orientation orientation) {
			U = V = 0d;
			this.orientation = orientation;
		}
	}

	private static class MathHelper {

		public static bool AreClose(double value1, double value2) => value1 == value2 || IsVerySmall(value1 - value2);

		public static bool IsVerySmall(double value) => Math.Abs(value) < 1E-06;

		public static bool GreaterThan(double value1, double value2) => value1 > value2 && !AreClose(value1, value2);

	}


}
