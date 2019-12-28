using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace USBHelper
{
	internal sealed class Kernel32
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct SYSTEM_INFO_EX
		{
			[FieldOffset(0)]
			public uint dwOemId;

			[FieldOffset(0)]
			public ushort wProcessorArchitecture;

			[FieldOffset(2)]
			public ushort wReserved;
		}

		public struct SYSTEM_INFO
		{
			public Kernel32.SYSTEM_INFO_EX sysInfoEx;

			public uint dwPageSize;

			public IntPtr lpMinimumApplicationAddress;

			public IntPtr lpMaximumApplicationAddress;

			public IntPtr dwActiveProcessorMask;

			public uint dwNumberOfProcessors;

			public uint dwProcessorType;

			public uint dwAllocationGranularity;

			public ushort wProcessorLevel;

			public ushort wProcessorRevision;
		}

		public const int PROCESSOR_ARCHITECTURE_AMD64 = 9;

		public const int PROCESSOR_ARCHITECTURE_IA64 = 6;

		public const int PROCESSOR_ARCHITECTURE_INTEL = 0;

		public const int PROCESSOR_ARCHITECTURE_UNKNOWN = 65535;

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWow64Process(IntPtr hProcess, ref bool lpSystemInfo);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern void GetSystemInfo(out Kernel32.SYSTEM_INFO lpSystemInfo);

		[DllImport("kernel32", SetLastError = true)]
		public static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

		[DllImport("kernel32", SetLastError = true)]
		public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

		[DllImport("kernel32", SetLastError = true)]
		public static extern int GetPrivateProfileSection(string lpAppName, StringBuilder lpReturnedString, int nSize, string lpFileName);

		[DllImport("kernel32", SetLastError = true)]
		public static extern int GetProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize);

		[DllImport("kernel32", SetLastError = true)]
		public static extern int GetProfileSection(string lpAppName, StringBuilder lpReturnedString, int nSize);

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool WriteProfileSection(string lpAppName, string lpString);

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool WriteProfileString(string lpAppName, string lpKeyName, string lpString);

		//[DllImport("kernel32.dll", SetLastError = true)]
		//public static extern unsafe int WriteFile(IntPtr handle, IntPtr buffer, int numBytesToWrite, IntPtr numBytesWritten, NativeOverlapped* lpOverlapped);

		public static bool IsWow64()
		{
			bool result = false;
			Process currentProcess = Process.GetCurrentProcess();
			IntPtr arg_0E_0 = currentProcess.Handle;
			if (!Kernel32.IsWow64Process(Process.GetCurrentProcess().Handle, ref result))
			{
				throw new Win32Exception();
			}
			return result;
		}
	}
}