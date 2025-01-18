using System.ComponentModel;

namespace RW.Common.WPF.Enums;

[Flags]
public enum WindowsVersion {
	[Description("未知")] Unknown = 1 << 0,
	[Description("Windows 7")] Windows7 = 1 << 1,
	[Description("Windows 8")] Windows8 = 1 << 2,
	[Description("Windows 8.1")] Windows8_1 = 1 << 3,
	[Description("Windows 10")] Windows10 = 1 << 4,
	[Description("Windows 11")] Windows11 = 1 << 5,
}

public class SystemVersionChecker {
	public static WindowsVersion GetWindowsVersion() {
		OperatingSystem os = Environment.OSVersion;
		Version version = os.Version;

		if (os.Platform == PlatformID.Win32NT) {
			switch (version.Major) {
				case 6:
					switch (version.Minor) {
						case 1:
							return WindowsVersion.Windows7;
						case 2:
							return WindowsVersion.Windows8;
						case 3:
							return WindowsVersion.Windows8_1;
					}
					break;
				case 10:
					if (version.Build >= 22000) {
						return WindowsVersion.Windows11;
					}
					return WindowsVersion.Windows10;
			}
		}

		return WindowsVersion.Unknown;
	}
}
