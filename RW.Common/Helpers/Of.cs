namespace RW.Common.Helpers;

public static class Of {
	public static T[] ArrayOf<T>(params T[] array) => array;
	public static List<T> ListOf<T>(params T[] array) => [.. array];
	public static HashSet<T> HashSetOf<T>(params T[] array) => [.. array];

	public static IEnumerable<T> EnumerableOf<T>(params T[] array) {
		for (int i = 0; i < array.Length; i++) {
			T item = array[i];
			yield return item;
		}
	}

	public static IEnumerable<T> EnumerableOf<T>(T item) {
		yield return item;
	}

}

