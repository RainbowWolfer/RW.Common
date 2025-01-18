using System.Collections;

namespace RW.Common.Collections;

/// <summary>
/// Represents a read-only list that extends the source list to a specified target length, 
/// filling out-of-range elements with a default value.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
/// <param name="source">The source list providing initial elements.</param>
/// <param name="targetLength">The total number of elements in the extended list.</param>
/// <param name="defaultValue">The default value for out-of-range elements.</param>
public class ExtendReadonlyList<T>(
	IReadOnlyList<T?> source,
	int targetLength,
	T? defaultValue = default
) : IReadOnlyList<T?> {

	/// <summary>
	/// Gets the value at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the value to get.</param>
	/// <returns>The value at the specified index if within range; otherwise, the default value.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the index is negative.</exception>
	public T? this[int index] {
		get {
			if (index < 0) {
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (index >= Source.Count) {
				return DefaultValue;
			}

			return Source[index];
		}
	}

	/// <summary>
	/// Gets the source list providing initial elements.
	/// </summary>
	public IReadOnlyList<T?> Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

	/// <summary>
	/// Gets the total number of elements in the extended list.
	/// </summary>
	public int Count { get; } = targetLength;

	/// <summary>
	/// Gets the default value for out-of-range elements.
	/// </summary>
	public T? DefaultValue { get; } = defaultValue;

	/// <summary>
	/// Returns an enumerator that iterates through the extended list.
	/// </summary>
	/// <returns>An enumerator for the extended list.</returns>
	public IEnumerator<T?> GetEnumerator() {
		for (int i = 0; i < Count; i++) {
			yield return this[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
