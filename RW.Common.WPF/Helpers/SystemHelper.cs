using System.Diagnostics;
using System.Security.Principal;

namespace RW.Common.WPF.Helpers;

public class SystemHelper {
	public static bool IsAdministratorSafe {
		get {
			try {
				return IsAdministrator;
			} catch (Exception ex) {
				Debug.WriteLine(ex);
				return false;
			}
		}
	}


	public static bool IsAdministrator {
		get {
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			WindowsPrincipal windowsPrincipal = new(current);
			return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}
