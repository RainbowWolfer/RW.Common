using System.Collections;

namespace RW.Common.Collections;

/// <summary>
/// Represents a read-only list that transforms elements from the source list to another type using a selector function.
/// </summary>
/// <typeparam name="TSource">The type of elements in the source list.</typeparam>
/// <typeparam name="TResult">The type of elements in the target list.</typeparam>
/// <param name="source">The source data list.</param>
/// <param name="selector">The transformation function.</param>
public class SelectorReadonlyList<TSource, TResult>(IReadOnlyList<TSource> source, Func<TSource, TResult> selector) : IReadOnlyList<TResult> {
	/// <summary>
	/// Gets the transformed element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The transformed element.</returns>
	public TResult this[int index] => selector(source[index]);

	/// <summary>
	/// Gets the number of elements in the list.
	/// </summary>
	public int Count => source.Count;

	/// <summary>
	/// The source data list.
	/// </summary>
	private readonly IReadOnlyList<TSource> source = source ?? throw new ArgumentNullException(nameof(source));

	/// <summary>
	/// The transformation function used to convert source elements to target elements.
	/// </summary>
	private readonly Func<TSource, TResult> selector = selector ?? throw new ArgumentNullException(nameof(selector));

	/// <summary>
	/// Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	public IEnumerator<TResult> GetEnumerator() {
		for (int i = 0; i < Count; i++) {
			yield return this[i];
		}
	}

	/// <summary>
	/// Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Provides extension methods for <see cref="SelectorReadonlyList{TSource, TResult}"/>.
/// </summary>
public static class SelectorReadonlyListExtensions {
	/// <summary>
	/// Creates a read-only list that transforms elements from the source list to another type using a selector function.
	/// </summary>
	/// <typeparam name="TSource">The type of elements in the source list.</typeparam>
	/// <typeparam name="TResult">The type of elements in the target list.</typeparam>
	/// <param name="source">The source data list.</param>
	/// <param name="selector">The transformation function.</param>
	/// <returns>A new <see cref="SelectorReadonlyList{TSource, TResult}"/> instance representing the transformed read-only list.</returns>
	public static IReadOnlyList<TResult> SelectAsReadonlyList<TSource, TResult>(this IReadOnlyList<TSource> source, Func<TSource, TResult> selector) {
		return new SelectorReadonlyList<TSource, TResult>(source, selector);
	}
}
