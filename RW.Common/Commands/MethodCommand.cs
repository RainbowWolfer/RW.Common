using System.Windows.Input;

namespace RW.Common.Commands;

public class MethodCommand(
	Action execute,
	Func<bool>? canExecute = null
) : ICommand {
	private readonly Action execute = execute ?? throw new ArgumentNullException(nameof(execute));

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object parameter) => canExecute?.Invoke() ?? true;

	public void Execute(object parameter) => execute();

	public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class MethodCommand<T>(
	Action<T?> execute,
	Func<T?, bool>? canExecute = null
) : ICommand {
	private readonly Action<T?> execute = execute ?? throw new ArgumentNullException(nameof(execute));

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object parameter) {
		if (parameter == null && typeof(T).IsValueType) {
			return canExecute == null || canExecute(default);
		}

		return canExecute == null || canExecute((T?)parameter);
	}

	public void Execute(object parameter) {
		if (parameter == null && typeof(T).IsValueType) {
			execute(default);
		} else {
			execute((T?)parameter);
		}
	}

	public void RaiseCanExecuteChanged() {
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}
