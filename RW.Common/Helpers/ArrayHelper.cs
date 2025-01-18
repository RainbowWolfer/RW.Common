using System.Collections;

namespace RW.Common.Helpers;

public static class ArrayHelper {
	// Array

	public static void ClearArray(this Array array) {
		if (array != null) {
			Array.Clear(array, 0, array.Length);
		}
	}

	public static int IndexOf<T>(this T[] array, T item) {
		return Array.IndexOf(array, item, 0, array.Length);
	}

	// IList

	public static void AddIfNotContains<T>(this IList<T> list, T item) {
		if (!list.Contains(item)) {
			list.Add(item);
		}
	}

	public static void InsertIfNotContains<T>(this IList<T> list, int index, T item) {
		if (!list.Contains(item)) {
			list.Insert(index, item);
		}
	}

	// IReadOnlyList

	public static double[] Offset(this IReadOnlyList<double> array, double offset) {
		double[] result = new double[array.Count];
		for (int i = 0; i < result.Length; i++) {
			result[i] = array[i] + offset;
		}
		return result;
	}

	public static double[] FixValuesLength(this IReadOnlyList<double> array, int requiredLength) {
		double[] result = new double[requiredLength];

		for (int i = 0; i < result.Length; i++) {
			if (i < array.Count) {
				result[i] = array[i];
			}
		}

		return result;
	}

	public static bool IsIndexValid<T>(this IReadOnlyList<T> list, int index) {
		return index >= 0 && index < list.Count;
	}

	public static bool IsIndexValid(this double[,] array, int row, int column) {
		return row >= 0 && row < array.GetLength(0) && column >= 0 && column < array.GetLength(1);
	}

	public static bool IsOffsetLengthValid<T>(this IReadOnlyList<T> list, int offset, int length) {
		if (offset < 0 || length < 0) {
			return false;
		}
		if (offset >= list.Count || offset + length > list.Count) {
			return false;
		}
		return true;
	}

	public static T[] Fill<T>(this T[] array, T value) {
		for (int i = 0; i < array.Length; i++) {
			array[i] = value;
		}
		return array;
	}

	public static int FindIndex<T>(IReadOnlyList<T> list, int startIndex, Predicate<T> match) {
		if (list == null) {
			throw new ArgumentNullException(nameof(list));
		}

		return FindIndex(list, startIndex, list.Count - startIndex, match);
	}

	public static int FindIndex<T>(IReadOnlyList<T> list, int startIndex, int count, Predicate<T> match) {
		if (list == null) {
			throw new ArgumentNullException(nameof(list));
		}

		if (startIndex < 0 || startIndex > list.Count) {
			throw new ArgumentOutOfRangeException(nameof(startIndex));
		}

		if (count < 0 || startIndex > list.Count - count) {
			throw new ArgumentOutOfRangeException(nameof(count));
		}

		if (match == null) {
			throw new ArgumentNullException(nameof(match));
		}

		int num = startIndex + count;
		for (int i = startIndex; i < num; i++) {
			if (match(list[i])) {
				return i;
			}
		}

		return -1;
	}


	public static T MinBy<T, TKey>(this IReadOnlyList<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey> {
		if (source == null) {
			throw new ArgumentNullException(nameof(source));
		}

		if (selector == null) {
			throw new ArgumentNullException(nameof(selector));
		}

		if (source.Count == 0) {
			throw new InvalidOperationException("Sequence contains no elements");
		}

		T min = source[0];
		TKey minKey = selector(min);

		for (int i = 1; i < source.Count; i++) {
			T candidate = source[i];
			TKey candidateKey = selector(candidate);

			if (candidateKey.CompareTo(minKey) < 0) {
				min = candidate;
				minKey = candidateKey;
			}
		}

		return min;
	}

	public static T MaxBy<T, TKey>(this IReadOnlyList<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey> {
		if (source == null) {
			throw new ArgumentNullException(nameof(source));
		}

		if (selector == null) {
			throw new ArgumentNullException(nameof(selector));
		}

		if (source.Count == 0) {
			throw new InvalidOperationException("Sequence contains no elements");
		}

		T max = source[0];
		TKey maxKey = selector(max);

		for (int i = 1; i < source.Count; i++) {
			T candidate = source[i];
			TKey candidateKey = selector(candidate);

			if (candidateKey.CompareTo(maxKey) > 0) {
				max = candidate;
				maxKey = candidateKey;
			}
		}

		return max;
	}



	// IEnumerable

	public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey> {
		if (source == null) {
			throw new ArgumentNullException(nameof(source));
		}

		if (selector == null) {
			throw new ArgumentNullException(nameof(selector));
		}

		using IEnumerator<T> enumerator = source.GetEnumerator();
		if (!enumerator.MoveNext()) {
			throw new InvalidOperationException("Sequence contains no elements");
		}

		T min = enumerator.Current;
		TKey minKey = selector(min);

		while (enumerator.MoveNext()) {
			T candidate = enumerator.Current;
			TKey candidateProjected = selector(candidate);

			if (candidateProjected.CompareTo(minKey) < 0) {
				min = candidate;
				minKey = candidateProjected;
			}
		}
		return min;
	}

	public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey> {
		if (source == null) {
			throw new ArgumentNullException(nameof(source));
		}

		if (selector == null) {
			throw new ArgumentNullException(nameof(selector));
		}

		using IEnumerator<T> enumerator = source.GetEnumerator();
		if (!enumerator.MoveNext()) {
			throw new InvalidOperationException("Sequence contains no elements");
		}

		T max = enumerator.Current;
		TKey maxKey = selector(max);

		while (enumerator.MoveNext()) {
			T candidate = enumerator.Current;
			TKey candidateProjected = selector(candidate);

			if (candidateProjected.CompareTo(maxKey) > 0) {
				max = candidate;
				maxKey = candidateProjected;
			}
		}
		return max;
	}

	public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action) {
		foreach (T item in ie) {
			action.Invoke(item);
		}
	}

	public static int CountIEnumerable(this IEnumerable enumerable) {
		if (enumerable == null) {
			throw new ArgumentNullException(nameof(enumerable));
		}

		int count = 0;
		IEnumerator enumerator = enumerable.GetEnumerator();
		try {
			checked {
				while (enumerator.MoveNext()) {
					count++;
				}
			}
		} finally {
			if (enumerator is IDisposable disposable) {
				disposable.Dispose(); // 释放枚举器资源
			}
		}
		return count;
	}


	public static bool IsEmpty(this IEnumerable ie) {
		if (ie == null) {
			return true;
		}

		if (ie is ICollection c1) {
			return c1.Count == 0;
		}

		IEnumerator enumerator = ie.GetEnumerator();
		return !enumerator.MoveNext();
	}

	public static bool IsNotEmpty(this IEnumerable ie) => !ie.IsEmpty();

	public static bool HasOnlyOne<T>(this IEnumerable<T> ie) => ie != null && ie.Count() == 1;

	public static bool IsEmpty<T>(this IEnumerable<T> ie) {
		if (ie == null) {
			return true;
		}

		if (ie is ICollection c1) {
			return c1.Count == 0;
		} else if (ie is ICollection<T> c2) {
			return c2.Count == 0;
		} else if (ie is IReadOnlyCollection<T> c3) {
			return c3.Count == 0;
		}

		return !ie.Any();
	}

	public static bool IsNotEmpty<T>(this IEnumerable<T> ie) => !ie.IsEmpty();

}

