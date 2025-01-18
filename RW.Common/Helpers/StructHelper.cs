using System.Runtime.InteropServices;

namespace RW.Common.Helpers;

public static class StructHelper {

	/// <summary>
	/// Converts a structure to a byte array.
	/// </summary>
	/// <typeparam name="T">The type of the structure.</typeparam>
	/// <param name="structObj">The structure to convert.</param>
	/// <returns>A byte array representing the structure.</returns>
	public static byte[] StructToBytes<T>(T structObj) where T : struct {
		int size = Marshal.SizeOf(structObj);
		byte[] bytes = new byte[size];
		IntPtr ptr = Marshal.AllocHGlobal(size);

		try {
			// Copy the structure to the allocated memory.
			Marshal.StructureToPtr(structObj, ptr, true);
			// Copy the memory content to the byte array.
			Marshal.Copy(ptr, bytes, 0, size);
		} finally {
			// Free the allocated memory.
			Marshal.FreeHGlobal(ptr);
		}

		return bytes;
	}

	/// <summary>
	/// Converts a byte array to a structure.
	/// </summary>
	/// <typeparam name="T">The type of the structure.</typeparam>
	/// <param name="bytes">The byte array to convert.</param>
	/// <returns>The structure represented by the byte array.</returns>
	/// <exception cref="ArgumentException">Thrown when the byte array is too small for the given structure.</exception>
	public static T BytesToStruct<T>(byte[] bytes) where T : struct {
		int size = Marshal.SizeOf(typeof(T));
		if (bytes.Length < size) {
			throw new ArgumentException("Byte array is too small for the given structure.");
		}

		IntPtr ptr = Marshal.AllocHGlobal(size);
		try {
			// Copy the byte array to the allocated memory.
			Marshal.Copy(bytes, 0, ptr, size);
			// Convert the memory content to the structure.
			return Marshal.PtrToStructure<T>(ptr);
		} finally {
			// Free the allocated memory.
			Marshal.FreeHGlobal(ptr);
		}
	}

	/// <summary>
	/// Checks if the byte array size is valid for the given structure type.
	/// </summary>
	/// <typeparam name="T">The type of the structure.</typeparam>
	/// <param name="bytes">The byte array to check.</param>
	/// <returns>True if the byte array size is valid, otherwise false.</returns>
	public static bool IsByteArraySizeValid<T>(byte[] bytes) where T : struct {
		int size = Marshal.SizeOf(typeof(T));
		return bytes.Length >= size;
	}

	/// <summary>
	/// Converts an array of structures to a byte array.
	/// </summary>
	/// <typeparam name="T">The type of the structures.</typeparam>
	/// <param name="structArray">The array of structures to convert.</param>
	/// <returns>A byte array representing the array of structures.</returns>
	public static byte[] StructsToBytes<T>(T[] structArray) where T : struct {
		int size = Marshal.SizeOf(typeof(T));
		byte[] bytes = new byte[size * structArray.Length];
		IntPtr ptr = Marshal.AllocHGlobal(size);

		try {
			for (int i = 0; i < structArray.Length; i++) {
				// Copy each structure to the allocated memory.
				Marshal.StructureToPtr(structArray[i], ptr, true);
				// Copy the memory content to the byte array.
				Marshal.Copy(ptr, bytes, i * size, size);
			}
		} finally {
			// Free the allocated memory.
			Marshal.FreeHGlobal(ptr);
		}

		return bytes;
	}

	/// <summary>
	/// Converts a byte array to an array of structures.
	/// </summary>
	/// <typeparam name="T">The type of the structures.</typeparam>
	/// <param name="bytes">The byte array to convert.</param>
	/// <returns>An array of structures represented by the byte array.</returns>
	/// <exception cref="ArgumentException">Thrown when the byte array size is not a multiple of the structure size.</exception>
	public static T[] BytesToStructs<T>(byte[] bytes) where T : struct {
		int size = Marshal.SizeOf(typeof(T));
		if (bytes.Length % size != 0) {
			throw new ArgumentException("Byte array size is not a multiple of the structure size.");
		}

		int structCount = bytes.Length / size;
		T[] structArray = new T[structCount];
		IntPtr ptr = Marshal.AllocHGlobal(size);

		try {
			for (int i = 0; i < structCount; i++) {
				// Copy the byte array to the allocated memory.
				Marshal.Copy(bytes, i * size, ptr, size);
				// Convert the memory content to the structure.
				structArray[i] = Marshal.PtrToStructure<T>(ptr);
			}
		} finally {
			// Free the allocated memory.
			Marshal.FreeHGlobal(ptr);
		}

		return structArray;
	}
}
