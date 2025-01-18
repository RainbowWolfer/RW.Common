namespace RW.Common.Events;

public class PayloadEventArgs(object payload) : EventArgs {
	public object Payload { get; } = payload;
}

public class PayloadEventArgs<T>(T payload) : EventArgs {
	public T Payload { get; } = payload;
}
