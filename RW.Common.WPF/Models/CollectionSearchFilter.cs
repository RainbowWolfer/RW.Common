using RW.Common.Helpers;
using RW.Common.Models;
using RW.Common.WPF.Helpers;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace RW.Common.WPF.Models;

public class CollectionSearchFilter<T> : BindModelBase {
	public event Action? BeforeSearchAction;
	public event Action? AfterSearchAction;

	protected string searchKey = string.Empty;

	protected CollectionViewSource CollectionSource { get; } = new();

	protected Func<T, string>? GetNameFunc { get; set; }

	protected Predicate<T>? Predicate { get; set; }

	protected CollectionSearchFilter(IEnumerable<T> collection) {
		CollectionSource.Source = collection;
		CollectionSource.Filter += CollectionSource_Filter;
		if (collection is INotifyCollectionChanged notifyCollectionChanged) {
			notifyCollectionChanged.CollectionChanged += CollectionChanged;
		}
	}

	protected virtual void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
		RaisePropertyChanged(nameof(AfterFilterIsEmpty));
	}

	public CollectionSearchFilter(IEnumerable<T> collection, Func<T, string> predicate) : this(collection) {
		GetNameFunc = predicate;
	}

	public CollectionSearchFilter(IEnumerable<T> collection, Predicate<T> predicate) : this(collection) {
		Predicate = predicate;
	}

	public virtual void AddGroup(PropertyGroupDescription group) {
		CollectionSource.GroupDescriptions.Add(group);
	}

	public virtual void ClearSort() {
		CollectionSource.SortDescriptions.Clear();
		RefreshFilter();
	}

	public virtual void AddSort(SortDescription sortDescription) {
		CollectionSource.SortDescriptions.Add(sortDescription);
		RefreshFilter();
	}

	protected virtual void CollectionSource_Filter(object sender, FilterEventArgs e) {
		if (e.Item is T item) {
			if (GetNameFunc != null) {
				if (SearchKey.IsBlank()) {
					e.Accepted = true;
				} else {
					e.Accepted = SearchKey.SearchFor(GetNameFunc.Invoke(item));
				}
			} else if (Predicate != null) {
				e.Accepted = Predicate.Invoke(item);
			} else {
				e.Accepted = true;
			}
		}
		if (SearchKey.IsNotBlank() && e.Item == null) {
			e.Accepted = false;
		}
	}

	public virtual ICollectionView View => CollectionSource.View;

	public virtual bool AfterFilterIsEmpty => View.Count() == 0;

	public virtual string SearchKey {
		get => searchKey;
		set => SetProperty(ref searchKey, value, OnSearchKeyChanged);
	}

	protected virtual void OnSearchKeyChanged() {
		BeforeSearchAction?.Invoke();
		RefreshFilter();
		AfterSearchAction?.Invoke();
	}

	public virtual void RefreshFilter() {
		CollectionSource.View.Refresh();
		RaisePropertyChanged(nameof(AfterFilterIsEmpty));
	}

}

