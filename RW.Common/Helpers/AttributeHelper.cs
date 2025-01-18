using RW.Common.Attributes;
using System.ComponentModel;
using System.Reflection;

namespace RW.Common.Helpers;

public static class AttributeHelper {
	public static string GetDescription<T>(this T? source) {
		if (source is null) {
			return string.Empty;
		}

		MemberInfo? field = source is MemberInfo mi ? mi : source.GetType().GetField(source.ToString());
		if (field == null) {
			return source.ToString();
		}

		if (field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).FirstOrDefault() is DescriptionAttribute attribute) {
			return attribute.Description;
		} else {
			return field.Name;
		}
	}

	public static string GetDisplayName<T>(this T? source) {
		if (source is null) {
			return string.Empty;
		}

		MemberInfo? field = source is MemberInfo mi ? mi : source.GetType().GetField(source.ToString());
		if (field == null) {
			return source.ToString();
		}

		if (field.GetCustomAttributes(typeof(DisplayNameAttribute), inherit: false).FirstOrDefault() is DisplayNameAttribute attribute) {
			return attribute.DisplayName;
		} else {
			return field.Name;
		}
	}

	public static string GetTooltip<T>(this T? source) {
		if (source is null) {
			return string.Empty;
		}

		MemberInfo field = source is MemberInfo mi ? mi : source.GetType().GetField(source.ToString());
		if (field == null) {
			return source.ToString();
		}

		if (field.GetCustomAttributes(typeof(TooltipAttribute), inherit: false).FirstOrDefault() is TooltipAttribute attribute) {
			return attribute.ToolTip;
		} else {
			return field.Name;
		}
	}
}
