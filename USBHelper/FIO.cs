using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace USBHelper
{
	internal class FIO
	{
		public struct OVERLAPPED
		{
			public int Internal;

			public int InternalHigh;

			public int Offset;

			public int OffsetHigh;

			public int hEvent;
		}

		public struct SECURITY_ATTRIBUTES
		{
			public int nLength;

			public int lpSecurityDescriptor;

			public int bInheritHandle;
		}

		public const uint GENERIC_READ = 2147483648u;

		public const uint GENERIC_WRITE = 1073741824u;

		public const uint FILE_SHARE_READ = 1u;

		public const uint FILE_SHARE_WRITE = 2u;

		public const uint FILE_FLAG_OVERLAPPED = 1073741824u;

		public const int INVALID_HANDLE_VALUE = -1;

		public const uint CREATE_NEW = 1u;

		public const uint CREATE_ALWAYS = 2u;

		public const uint OPEN_EXISTING = 3u;

		public const short FILE_ATTRIBUTE_NORMAL = 128;

		public const int WAIT_TIMEOUT = 258;

		public const short WAIT_OBJECT_0 = 0;

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int CancelIo(IntPtr hFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateEvent(ref FIO.SECURITY_ATTRIBUTES SecurityAttributes, int bManualReset, int bInitialState, string lpName);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, int lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, int lpOverlapped);
	}
}