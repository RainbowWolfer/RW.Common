using System.Windows.Input;

namespace RW.Common.WPF.Commands;

public static class WpfCommands {

	public static RoutedCommand Search { get; } = new(nameof(Search), typeof(WpfCommands));

	public static RoutedCommand Clear { get; } = new(nameof(Clear), typeof(WpfCommands));

	public static RoutedCommand Previous { get; } = new(nameof(Previous), typeof(WpfCommands));

	public static RoutedCommand Next { get; } = new(nameof(Next), typeof(WpfCommands));

}
