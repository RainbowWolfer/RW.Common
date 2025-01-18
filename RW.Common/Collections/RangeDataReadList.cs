using System.Collections;

namespace RW.Common.Collections;

/// <summary>
/// Represents a read-only list that extracts data from a source list based on a given offset and length.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class RangeDataReadList<T> : IReadOnlyList<T> {

	/// <summary>
	/// Gets the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The element at the specified index.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
	public virtual T this[int index] {
		get {
			if (index < 0 || index >= length) {
				throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
			}
			return source[offset + index];
		}
	}

	/// <summary>
	/// Gets the number of elements in the list.
	/// </summary>
	public int Count => length;

	protected readonly IReadOnlyList<T> source;
	protected readonly int offset;
	protected readonly int length;

	/// <summary>
	/// Initializes a new instance of the <see cref="RangeDataReadList{T}"/> class.
	/// </summary>
	/// <param name="source">The source data list.</param>
	/// <param name="offset">The starting offset of the data.</param>
	/// <param name="length">The length of the list.</param>
	/// <exception cref="ArgumentNullException">Thrown when the source is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the offset or length is out of range.</exception>
	public RangeDataReadList(IReadOnlyList<T> source, int offset, int length) {
		this.source = source ?? throw new ArgumentNullException(nameof(source));
		if (offset < 0 || offset >= source.Count) {
			throw new ArgumentOutOfRangeException(nameof(offset), "Offset is out of range.");
		} else if (length < 0 || offset + length > source.Count) {
			throw new ArgumentOutOfRangeException(nameof(length), "Length is out of range.");
		}

		this.offset = offset;
		this.length = length;
	}

	/// <summary>
	/// Returns an enumerator that iterates through the list.
	/// </summary>
	/// <returns>An enumerator for the list.</returns>
	public IEnumerator<T> GetEnumerator() {
		for (int i = 0; i < Count; i++) {
			yield return this[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class RangeDoubleReadList(
	IReadOnlyList<double> source,
	int offset,
	int length
) : RangeDataReadList<double>(source, offset, length);

public class RangeFloatReadList(
	IReadOnlyList<float> source,
	int offset,
	int length
) : RangeDataReadList<float>(source, offset, length);

public class RangeIntReadList(
	IReadOnlyList<int> source,
	int offset,
	int length
) : RangeDataReadList<int>(source, offset, length);

public class RangeByteReadList(
	IReadOnlyList<byte> source,
	int offset,
	int length
) : RangeDataReadList<byte>(source, offset, length);

public class RangeShortReadList(
	IReadOnlyList<short> source,
	int offset,
	int length
) : RangeDataReadList<short>(source, offset, length);

public static class RangeDataReadListExtension {
	/// <summary>
	/// Creates a sublist from the given list with the specified offset and length.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <param name="list">The source list.</param>
	/// <param name="offset">The starting offset of the sublist.</param>
	/// <param name="length">The length of the sublist.</param>
	/// <returns>A read-only sublist.</returns>
	public static IReadOnlyList<T> SubList<T>(this IReadOnlyList<T> list, int offset, int length) {
		return new RangeDataReadList<T>(list, offset, length);
	}

	/// <summary>
	/// Creates a sublist from the given list with the specified length, starting from the beginning.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <param name="list">The source list.</param>
	/// <param name="length">The length of the sublist.</param>
	/// <returns>A read-only sublist.</returns>
	public static IReadOnlyList<T> SubList<T>(this IReadOnlyList<T> list, int length) {
		return new RangeDataReadList<T>(list, 0, length);
	}
}
