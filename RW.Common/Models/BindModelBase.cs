using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RW.Common.Models;

public abstract class BindModelBase : INotifyPropertyChanged {
	public event PropertyChangedEventHandler? PropertyChanged;

	public virtual bool ShouldCheckValueMatching<T>(string? propertyName, T from, T to) => true;

	protected virtual bool SetProperty<T>(ref T storage, T value, Action? onChanged = null, [CallerMemberName] string? propertyName = null) {
		if (ShouldCheckValueMatching(propertyName, storage, value) && EqualityComparer<T>.Default.Equals(storage, value)) {
			return false;
		}
		CancelEventArgs cancelEventArgs = new();
		OnPropertyChanging(propertyName, storage, value, cancelEventArgs);
		if (cancelEventArgs.Cancel) {
			return false;
		}

		storage = value;
		onChanged?.Invoke();
		RaisePropertyChanged(propertyName);

		return true;
	}

	protected virtual bool SetPropertyNoCheck<T>(ref T storage, T value, Action? onChanged = null, [CallerMemberName] string? propertyName = null) {
		CancelEventArgs cancelEventArgs = new();
		OnPropertyChanging(propertyName, storage, value, cancelEventArgs);
		if (cancelEventArgs.Cancel) {
			return false;
		}
		storage = value;
		onChanged?.Invoke();
		RaisePropertyChanged(propertyName);

		return true;
	}

	protected virtual void OnPropertyChanging<T>(string? propertyName, T from, T to, CancelEventArgs cancelEventArgs) {
		return;
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) {
		PropertyChanged?.Invoke(this, args);
	}

	protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null) {
		OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
	}

}

