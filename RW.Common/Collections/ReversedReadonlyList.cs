using System.Collections;

namespace RW.Common.Collections;

public class ReversedReadonlyList<T>(IReadOnlyList<T> source) : IReadOnlyList<T> {
	public IReadOnlyList<T> Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

	public virtual T this[int index] => Source[Source.Count - index - 1];

	public int Count => Source.Count;

	public IEnumerator<T> GetEnumerator() {
		int length = Count;
		for (int i = 0; i < length; i++) {
			yield return Source[length - i - 1];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
