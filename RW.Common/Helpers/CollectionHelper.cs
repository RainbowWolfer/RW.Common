using System.Collections;
using System.Collections.Specialized;

namespace RW.Common.Helpers;

public static class CollectionHelper {

	public static void RemoveAll(this IList list) {
		while (list.Count > 0) {
			list.RemoveAt(list.Count - 1);
		}
	}

	public static void RemoveAll<T>(this IList<T> collection, Func<T, bool> condition) {
		for (int i = collection.Count - 1; i >= 0; i--) {
			if (condition(collection[i])) {
				collection.RemoveAt(i);
			}
		}
	}

	public static T? GetFromIndexOrDefault<T>(this IList<T?> array, int index, T? d = default) {
		if (index < 0 || index >= array.Count) {
			return d;
		} else {
			return array[index];
		}
	}

	public static void HandleCollectionChanged<T>(this NotifyCollectionChangedEventArgs args, Action<T?> handleNewItem, Action<T?> handleOldItem) {
		if (args.NewItems != null) {
			foreach (T? item in args.NewItems.OfType<T?>()) {
				handleNewItem?.Invoke(item);
			}
		}
		if (args.OldItems != null) {
			foreach (T? item in args.OldItems.OfType<T?>()) {
				handleOldItem?.Invoke(item);
			}
		}
	}

}
