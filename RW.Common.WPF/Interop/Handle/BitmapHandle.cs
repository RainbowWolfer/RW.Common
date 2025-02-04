﻿using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace RW.Common.WPF.Interop.Handle;

#pragma warning disable SYSLIB0004 // Type or member is obsolete
internal sealed class BitmapHandle : WpfSafeHandle {
	[SecurityCritical]
	private BitmapHandle() : this(true) {
		//请不要删除此构造函数，否则当使用自定义ico文件时会报错
	}

	[SecurityCritical]
	private BitmapHandle(bool ownsHandle) : base(ownsHandle, CommonHandles.GDI) {
	}

	[SecurityCritical]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	protected override bool ReleaseHandle() {
		return InteropMethods.DeleteObject(handle);
	}

	[SecurityCritical]
	internal HandleRef MakeHandleRef(object obj) {
		return new(obj, handle);
	}

	[SecurityCritical]
	internal static BitmapHandle CreateFromHandle(IntPtr hbitmap, bool ownsHandle = true) {
		return new(ownsHandle) {
			handle = hbitmap,
		};
	}
}
#pragma warning restore SYSLIB0004 // Type or member is obsolete
