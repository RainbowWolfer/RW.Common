using System.Runtime.ConstrainedExecution;
using System.Security;

namespace RW.Common.WPF.Interop.Handle;

#pragma warning disable SYSLIB0004 // Type or member is obsolete
internal sealed class IconHandle : WpfSafeHandle {
	[SecurityCritical]
	private IconHandle() : base(true, CommonHandles.Icon) {
	}

	[SecurityCritical]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	protected override bool ReleaseHandle() {
		return InteropMethods.DestroyIcon(handle);
	}

	[SecurityCritical, SecuritySafeCritical]
	internal static IconHandle GetInvalidIcon() {
		return new();
	}

	[SecurityCritical]
	internal IntPtr CriticalGetHandle() {
		return handle;
	}
}
#pragma warning restore SYSLIB0004 // Type or member is obsolete
