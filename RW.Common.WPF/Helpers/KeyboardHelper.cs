using System.Windows.Input;

namespace RW.Common.WPF.Helpers;

public static class KeyboardHelper {
	public static bool ControlPressed => (Keyboard.Modifiers & ModifierKeys.Control) != 0;
	public static bool ShiftPressed => (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
	public static bool AltPressed => (Keyboard.Modifiers & ModifierKeys.Alt) != 0;
	public static bool WindowsPressed => (Keyboard.Modifiers & ModifierKeys.Windows) != 0;

	public static bool NoModifiersPressed => Keyboard.Modifiers == ModifierKeys.None;

}
