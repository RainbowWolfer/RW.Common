namespace RW.Common.Handlers;

public delegate void TypedEventHandler<TSender, TResult>(TSender sender, TResult args);