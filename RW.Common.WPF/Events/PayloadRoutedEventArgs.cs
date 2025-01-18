using System.Windows;

namespace RW.Common.WPF.Events;

public class PayloadRoutedEventArgs(RoutedEvent routedEvent, object source, object payload) : RoutedEventArgs(routedEvent, source) {
	public object Payload { get; } = payload;
}

public class PayloadRoutedEventArgs<T>(RoutedEvent routedEvent, object source, T payload) : RoutedEventArgs(routedEvent, source) {
	public T Payload { get; } = payload;
}
