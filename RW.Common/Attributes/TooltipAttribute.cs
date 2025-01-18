namespace RW.Common.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public sealed class TooltipAttribute(string toolTip) : Attribute {
	public string ToolTip { get; } = toolTip;
}