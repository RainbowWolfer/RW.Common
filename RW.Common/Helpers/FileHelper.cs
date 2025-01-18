using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RW.Common.Helpers;

public static class FileHelper {

	public static string GetUniqueFileName(string fullPath) {
		int count = 1;

		string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
		string extension = Path.GetExtension(fullPath);
		string path = Path.GetDirectoryName(fullPath);
		string newFullPath = fullPath;

		while (File.Exists(newFullPath)) {
			string tempFileName = $"{fileNameOnly}({count++})";
			newFullPath = Path.Combine(path, tempFileName + extension);
		}

		return newFullPath;
	}

	public static string FixInvalidFullFilePath(string fullPath, string replacement = " ") {
		string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
		string extension = Path.GetExtension(fullPath);
		string path = Path.GetDirectoryName(fullPath);

		string newFilename = string.Join(replacement, fileNameOnly.Split(Path.GetInvalidFileNameChars()));
		return Path.Combine(path, newFilename + extension);
	}

	public static bool IsFileNameInvalid(this string name) => name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;

	public static string FixInvalidFileName(string fileName, string replacement = " ") {
		string newFilename = string.Join(replacement, fileName.Split(Path.GetInvalidFileNameChars()));
		return newFilename;
	}

	public static bool IsDirectoryEmpty(this string path) {
		return !Directory.EnumerateFileSystemEntries(path).Any();
	}

	public static DirectoryInfo CreateDirectory(string dir) {
		if (Path.HasExtension(dir)) {
			dir = Path.GetDirectoryName(dir);
		}
		if (!Directory.Exists(dir)) {
			return Directory.CreateDirectory(dir);
		}
		return new DirectoryInfo(dir);
	}

	public static Exception? TryCreateDirectory(string dir) {
		try {
			CreateDirectory(dir);
			return null;
		} catch (Exception ex) {
			return ex;
		}
	}

	public static bool FileNameExist(this string fileName, IEnumerable<string> fileNames) {
		return fileNames.Any(x => {
			if (x.Equals(fileName, StringComparison.InvariantCultureIgnoreCase)) {
				return true;
			}
			if (x == fileName) {
				return true;
			}
			return false;
		});
	}

	public static bool HasWriteAccessToFolder(string folderPath) {
		try {
			CreateDirectory(folderPath);
			string tempFilePath;
			do {
				tempFilePath = Path.Combine(folderPath, Path.GetRandomFileName());
			} while (File.Exists(tempFilePath));
			using FileStream fs = File.Create(tempFilePath, 1, FileOptions.DeleteOnClose);
			return true;
		} catch {
			return false;
		}
	}


	public static Exception? ShowInExplorer(this string filePath) {
		if (filePath.IsBlank()) {
			return new ArgumentException($"{nameof(filePath)} is blank");
		}
		try {
			if (File.Exists(filePath)) {
				using Process explorer = new();
				explorer.StartInfo.FileName = "explorer.exe";
				explorer.StartInfo.Arguments = "/select,\"" + filePath + "\"";
				explorer.Start();
			} else {
				if (Directory.Exists(filePath)) {
					using Process process = Process.Start("explorer.exe", filePath);
				} else {
					string directory = Path.GetDirectoryName(filePath);
					if (Directory.Exists(directory)) {
						using Process process = Process.Start("explorer.exe", directory);
					}
				}
			}
			return null;
		} catch (Exception ex) {
			return ex;
		}
	}

	/// <summary>
	/// 是否是根目录，如 c:/ 或 E:/
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static bool IsRootDirectory(string path) {
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
			return IsWindowsRootDirectory(path);
		} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
			return IsUnixRootDirectory(path);
		} else {
			return false;
		}
	}

	private static bool IsWindowsRootDirectory(string path) {
		// 检查常见的根目录表示方式
		if (Regex.IsMatch(path, @"^[a-zA-Z]:[\\/]{1,2}$", RegexOptions.IgnoreCase)) {
			return true;
		}

		// 检查是否为 URI 格式的根目录
		try {
			Uri uri = new(path);
			if (uri.IsAbsoluteUri && uri.IsUnc) {
				return Regex.IsMatch(uri.LocalPath, @"^[a-zA-Z]:[\\/]{1,2}$", RegexOptions.IgnoreCase);
			}
		} catch {
			// 忽略无效 URI 格式
		}

		return false;
	}

	private static bool IsUnixRootDirectory(string path) => path == "/";

	public static async Task<string> CalculateFileMD5Async(string filePath, int bufferSize = 4096) {
		return await Task.Run(() => {
			return CalculateFileMD5(filePath, bufferSize);
		});
	}

	public static string CalculateFileMD5(string filePath, int bufferSize = 4096) // bufferSize 设置缓冲区大小，默认为 4KB
	{
		if (!File.Exists(filePath)) {
			throw new FileNotFoundException("File not found.", filePath);
		}

		using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		using MD5 md5 = MD5.Create();
		byte[] buffer = new byte[bufferSize];
		int bytesRead;

		while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0) {
			md5.TransformBlock(buffer, 0, bytesRead, buffer, 0); // 逐步计算哈希值
		}

		md5.TransformFinalBlock(buffer, 0, 0); // 完成哈希计算

		byte[] hashBytes = md5.Hash;

		// 将字节数组转换为十六进制字符串
		StringBuilder builder = new();
		for (int i = 0; i < hashBytes.Length; i++) {
			builder.Append(hashBytes[i].ToString("x2")); // "x2" 表示转换为两位十六进制小写字母
		}
		return builder.ToString();

		//使用BitConverter转换(更简洁的写法)
		//return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
	}
}
