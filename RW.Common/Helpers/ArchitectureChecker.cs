using System.Runtime.InteropServices;

namespace RW.Common.Helpers;

public static class ArchitectureChecker {
	public static string GetArchitectureString() {
		return GetArchitecture() switch {
			Architecture.Arm => "ARM",
			Architecture.Arm64 => "ARM64",
			Architecture.X64 => "X64",
			Architecture.X86 => "X86",
			_ => "Unknown",
		};
	}

	public static Architecture GetArchitecture() => RuntimeInformation.OSArchitecture;

	public static bool Is64BitProcess() => IntPtr.Size == 8;

	public static bool Is32BitProcess() => IntPtr.Size == 4;
}

