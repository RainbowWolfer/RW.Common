using System.Text;

namespace RW.Common.Helpers;

public static class RandomHelper {
	public static Random Shared { get; } = new Random();

	/// <summary>
	/// Generates a random string of the specified length using ASCII characters from 48 to 90.
	/// </summary>
	/// <param name="length">The length of the string.</param>
	/// <param name="random">Optional random number generator. Defaults to the shared instance.</param>
	/// <returns>A random string.</returns>
	public static string GetRandomString(int length, Random? random = null) {
		random ??= Shared;
		StringBuilder builder = new();
		for (int i = 0; i < length; i++) {
			int r = random.Next(48, 91); // ASCII code 48 to 90
			char c = (char)r;
			builder.Append(c);
		}
		return builder.ToString();
	}

	/// <summary>
	/// Generates a random integer within the specified range.
	/// </summary>
	/// <param name="minValue">Inclusive lower bound.</param>
	/// <param name="maxValue">Exclusive upper bound.</param>
	/// <param name="random">Optional random number generator. Defaults to the shared instance.</param>
	/// <returns>A random integer.</returns>
	public static int GetRandomInt(int minValue, int maxValue, Random? random = null) {
		random ??= Shared;
		return random.Next(minValue, maxValue); // maxValue is exclusive
	}

	/// <summary>
	/// Generates a random double within the specified range.
	/// </summary>
	/// <param name="minValue">Inclusive lower bound.</param>
	/// <param name="maxValue">Inclusive upper bound.</param>
	/// <param name="random">Optional random number generator. Defaults to the shared instance.</param>
	/// <returns>A random double.</returns>
	public static double GetRandomDouble(double minValue, double maxValue, Random? random = null) {
		random ??= Shared;
		return random.NextDouble() * (maxValue - minValue) + minValue; // minValue and maxValue are inclusive
	}

	/// <summary>
	/// Generates a random boolean value.
	/// </summary>
	/// <param name="random">Optional random number generator. Defaults to the shared instance.</param>
	/// <returns>A random boolean value.</returns>
	public static bool GetRandomBool(Random? random = null) {
		random ??= Shared;
		return random.Next(2) == 1;
	}

	/// <summary>
	/// Generates a random date within the specified range.
	/// </summary>
	/// <param name="startDate">Inclusive start date.</param>
	/// <param name="endDate">Exclusive end date.</param>
	/// <param name="random">Optional random number generator. Defaults to the shared instance.</param>
	/// <returns>A random date.</returns>
	public static DateTime GetRandomDate(DateTime startDate, DateTime endDate, Random? random = null) {
		random ??= Shared;
		int range = (endDate - startDate).Days;
		return startDate.AddDays(random.Next(range)); // endDate is exclusive
	}

	/// <summary>
	/// Generates a random byte array of the specified length.
	/// </summary>
	/// <param name="length">The length of the byte array.</param>
	/// <param name="random">Optional random number generator. Defaults to the shared instance.</param>
	/// <returns>A random byte array.</returns>
	public static byte[] GetRandomBytes(int length, Random? random = null) {
		random ??= Shared;
		byte[] bytes = new byte[length];
		random.NextBytes(bytes);
		return bytes;
	}
}
