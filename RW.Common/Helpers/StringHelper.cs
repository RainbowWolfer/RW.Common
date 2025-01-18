using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RW.Common.Helpers;

public static class StringHelper {

	public static bool IsBlank([NotNullWhen(false)] this string? text) => string.IsNullOrWhiteSpace(text);

	public static bool IsNotBlank([NotNullWhen(true)] this string? text) => !text.IsBlank();

	public static string? NotBlankCheck(this string? text) => text.IsBlank() ? null : text;

	public static string SafeString(this string? text) => text ?? string.Empty;
	public static string SafeToString(this object? text) => text?.ToString() ?? string.Empty;

	public static string IndexToColumn(this int index) {
		const int COLUMN_BASE = 26;
		const int DIGIT_MAX = 7; // ceil(log26(Int32.Max))
		const string DIGITS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		if (index <= 0) {
			throw new IndexOutOfRangeException("index must be a positive number");
		}

		if (index <= COLUMN_BASE) {
			return DIGITS[index - 1].ToString();
		}

		StringBuilder sb = new StringBuilder().Append(' ', DIGIT_MAX);
		int current = index;
		int offset = DIGIT_MAX;
		while (current > 0) {
			sb[--offset] = DIGITS[--current % COLUMN_BASE];
			current /= COLUMN_BASE;
		}
		return sb.ToString(offset, DIGIT_MAX - offset);
	}

	public static bool SearchFor(this string searchKey, string? targetName) {
		if (targetName.IsBlank()) {
			return false;
		}
		return targetName.ToLower().Trim().Contains(searchKey.ToLower().Trim());
	}



	//private string IndexToColumn(int index) {
	//	string column = string.Empty;
	//	while (index > 0) {
	//		index--;
	//		column = (char)('A' + index % 26) + column;
	//		index /= 26;
	//	}
	//	return column;
	//}

}
