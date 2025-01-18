using System.Numerics;

namespace RW.Common.Helpers;

public static class NumberHelper {

	public static bool IsNaN(this double d) => double.IsNaN(d);
	public static bool IsPositiveInfinity(this double d) => double.IsPositiveInfinity(d);
	public static bool IsNegativeInfinity(this double d) => double.IsNegativeInfinity(d);
	public static bool IsInfinity(this double d) => double.IsInfinity(d);

	public static bool IsNumberValid(this double d) {
		return !double.IsNaN(d) && !double.IsInfinity(d);
	}

	public static bool IsNumberInvalid(this double d) {
		return double.IsNaN(d) || double.IsInfinity(d);
	}

	public static bool IsComplexValid(this Complex complex) {
		return complex.Real.IsNumberValid() && complex.Imaginary.IsNumberValid();
	}

	public static double ReplaceInvalid(this double value, double defaultValue = 0) {
		if (value.IsNumberValid()) {
			return value;
		} else {
			return defaultValue;
		}
	}

	public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> {
		if (value.CompareTo(min) < 0) {
			return min;
		} else if (value.CompareTo(max) > 0) {
			return max;
		} else {
			return value;
		}
	}

}
