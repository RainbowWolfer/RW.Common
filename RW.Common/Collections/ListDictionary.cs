using System.Collections;
using System.Runtime.Serialization;

namespace RW.Common.Collections;

/// <summary>
/// Represents an ordered list collection of key-value pairs, implemented using an internal list and index dictionary.
/// </summary>
/// <typeparam name="TKey">The type of the keys.</typeparam>
/// <typeparam name="TValue">The type of the values.</typeparam>
[Serializable]
public sealed class ListDictionary<TKey, TValue> :
	IDictionary<TKey, TValue>,
	IReadOnlyDictionary<TKey, TValue>,
	IDictionary,
	ISerializable,
	IComparable<ListDictionary<TKey, TValue>>
	where TKey : notnull {

	private readonly List<KeyValuePair<TKey, TValue>> dictionary;
	private readonly Dictionary<TKey, int> indexLookup;
	private readonly object syncRoot = new();

	public ListDictionary() {
		dictionary = [];
		indexLookup = [];
	}

	public ListDictionary(int capacity) {
		dictionary = new List<KeyValuePair<TKey, TValue>>(capacity);
		indexLookup = new Dictionary<TKey, int>(capacity);
	}

	public ListDictionary(SerializationInfo info, StreamingContext context) {
		dictionary = (List<KeyValuePair<TKey, TValue>>)info.GetValue(nameof(dictionary), typeof(List<KeyValuePair<TKey, TValue>>))!;
		indexLookup = [];
		RebuildIndexLookup();
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
		info.AddValue(nameof(dictionary), dictionary);
	}

	public ICollection<TKey> Keys => GetKeysArray();
	public ICollection<TValue> Values => GetValuesArray();

	IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => GetKeys();
	IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => GetValues();

	ICollection IDictionary.Keys => GetKeysArray();
	ICollection IDictionary.Values => GetValuesArray();

	public int Count => dictionary.Count;

	public bool IsReadOnly => false;
	bool IDictionary.IsFixedSize => false;

	object ICollection.SyncRoot => syncRoot;

	bool ICollection.IsSynchronized => false;

	object? IDictionary.this[object key] {
		get => this[(TKey)key];
		set => this[(TKey)key] = (TValue)value!;
	}

	public TValue this[int index] {
		get => dictionary[index].Value;
		set => dictionary[index] = new KeyValuePair<TKey, TValue>(dictionary[index].Key, value);
	}

	public TValue this[TKey key] {
		get {
			if (indexLookup.TryGetValue(key, out int index)) {
				return dictionary[index].Value;
			}
			throw new KeyNotFoundException();
		}
		set {
			if (indexLookup.TryGetValue(key, out int index)) {
				dictionary[index] = new KeyValuePair<TKey, TValue>(key, value);
			} else {
				Add(key, value);
			}
		}
	}

	public void Clear() {
		dictionary.Clear();
		indexLookup.Clear();
	}

	public void Add(TKey key, TValue value) {
		if (ContainsKey(key)) {
			throw new ArgumentException("Duplicated Key");
		}
		indexLookup[key] = dictionary.Count;
		dictionary.Add(new KeyValuePair<TKey, TValue>(key, value));
	}

	public void Insert(int index, TKey key, TValue value) {
		if (ContainsKey(key)) {
			throw new ArgumentException("Duplicated Key");
		}
		dictionary.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
		RebuildIndexLookup();
	}

	public int IndexOf(TKey key) {
		return indexLookup.TryGetValue(key, out int index) ? index : -1;
	}

	public bool Remove(TKey key) {
		if (indexLookup.TryGetValue(key, out int index)) {
			dictionary.RemoveAt(index);
			RebuildIndexLookup();
			return true;
		}
		return false;
	}

	public bool ContainsKey(TKey key) => indexLookup.ContainsKey(key);

	public TKey[] GetKeysArray() => [.. dictionary.Select(x => x.Key)];
	public TValue[] GetValuesArray() => [.. dictionary.Select(x => x.Value)];

	public IEnumerable<TKey> GetKeys() => dictionary.Select(x => x.Key);
	public IEnumerable<TValue> GetValues() => dictionary.Select(x => x.Value);

	public bool TryGetValue(TKey key, out TValue value) {
		if (indexLookup.TryGetValue(key, out int index)) {
			value = dictionary[index].Value;
			return true;
		}
		value = default!;
		return false;
	}

	public void Add(KeyValuePair<TKey, TValue> item) {
		Add(item.Key, item.Value);
	}

	public bool Contains(KeyValuePair<TKey, TValue> item) {
		return dictionary.Contains(item);
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
		dictionary.CopyTo(array, arrayIndex);
	}

	public bool Remove(KeyValuePair<TKey, TValue> item) {
		return Remove(item.Key);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	IDictionaryEnumerator IDictionary.GetEnumerator() => new DictionaryEnumerator<TKey, TValue>(this);

	bool IDictionary.Contains(object key) => ContainsKey((TKey)key);

	void IDictionary.Add(object key, object? value) => Add((TKey)key, (TValue)value!);

	void IDictionary.Remove(object key) => Remove((TKey)key);

	void ICollection.CopyTo(Array array, int index) {
		if (array == null) {
			throw new ArgumentNullException(nameof(array));
		} else if (array.Rank != 1) {
			throw new ArgumentException("Array must be single-dimensional", nameof(array));
		} else if (array.GetLowerBound(0) != 0) {
			throw new ArgumentException("Array must have zero-based indexing", nameof(array));
		} else if (index < 0 || index > array.Length) {
			throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
		} else if (array.Length - index < dictionary.Count) {
			throw new ArgumentException("Array is too small to hold the items", nameof(array));
		}

		if (array is KeyValuePair<TKey, TValue>[] pairs) {
			dictionary.CopyTo(pairs, index);
		} else if (array is DictionaryEntry[] entries) {
			for (int i = 0; i < dictionary.Count; i++) {
				KeyValuePair<TKey, TValue> kvp = dictionary[i];
				entries[index + i] = new DictionaryEntry(kvp.Key, kvp.Value);
			}
		} else {
			if (array is not object[] objects) {
				throw new ArgumentException("Invalid array type", nameof(array));
			}

			try {
				for (int i = 0; i < dictionary.Count; i++) {
					KeyValuePair<TKey, TValue> kvp = dictionary[i];
					objects[index + i] = new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
				}
			} catch (ArrayTypeMismatchException) {
				throw new ArgumentException("Invalid array type", nameof(array));
			}
		}
	}

	private void RebuildIndexLookup() {
		indexLookup.Clear();
		for (int i = 0; i < dictionary.Count; i++) {
			indexLookup[dictionary[i].Key] = i;
		}
	}

	public int CompareTo(ListDictionary<TKey, TValue>? other) {
		return other == null ? 1 : Count.CompareTo(other.Count);
	}

	private class DictionaryEnumerator<K, V>(IDictionary<K, V> dictionary) : IDictionaryEnumerator {
		private readonly IEnumerator<KeyValuePair<K, V>> enumerator = dictionary.GetEnumerator();

		public object? Key => enumerator.Current.Key;

		public object? Value => enumerator.Current.Value;

		public DictionaryEntry Entry => new(Key, Value);

		public bool MoveNext() => enumerator.MoveNext();

		public void Reset() => enumerator.Reset();

		public object Current => Entry;
	}
}
