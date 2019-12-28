using System;
using System.Runtime.InteropServices;
using System.Text;

namespace USBHelper
{
	internal sealed class DM
	{
		[StructLayout(LayoutKind.Sequential)]
		public class DEV_BROADCAST_DEVICEINTERFACE
		{
			public int dbcc_size;

			public int dbcc_devicetype;

			public int dbcc_reserved;

			public Guid dbcc_classguid;

			public short dbcc_name;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class DEV_BROADCAST_DEVICEINTERFACE_1
		{
			public int dbcc_size;

			public int dbcc_devicetype;

			public int dbcc_reserved;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U1)]
			public byte[] dbcc_classguid;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
			public char[] dbcc_name;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class DEV_BROADCAST_HANDLE
		{
			public int dbch_size;

			public int dbch_devicetype;

			public int dbch_reserved;

			public IntPtr dbch_handle;

			public IntPtr dbch_hdevnotify;

			public Guid dbch_eventguid;

			public uint dbch_nameoffset;

			public byte[] dbch_data = new byte[1];
		}

		[StructLayout(LayoutKind.Sequential)]
		public class DEV_BROADCAST_HDR
		{
			public int dbch_size;

			public int dbch_devicetype;

			public int dbch_reserved;
		}

		public struct SP_DEVICE_INTERFACE_DATA
		{
			public int cbSize;

			public Guid InterfaceClassGuid;

			public int Flags;

			public IntPtr Reserved;
		}

		public struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			public uint cbSize;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public string DevicePath;
		}

		public struct SP_DEVINFO_DATA
		{
			public int cbSize;

			public Guid ClassGuid;

			public int DevInst;

			public IntPtr Reserved;
		}

		public const int DBT_DEVICEARRIVAL = 32768;

		public const int DBT_DEVICEREMOVECOMPLETE = 32772;

		public const int DBT_DEVTYP_DEVICEINTERFACE = 5;

		public const int DBT_DEVTYP_HANDLE = 6;

		public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

		public const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;

		public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;

		public const int WM_DEVICECHANGE = 537;

		public const int DIGCF_PRESENT = 2;

		public const int DIGCF_ALLCLASSES = 4;

		public const int DIGCF_PROFILE = 8;

		public const int DIGCF_DEVICEINTERFACE = 16;

		public const int ERROR_INSUFFICIENT_BUFFER = 122;

		public const int SP_MAX_MACHINENAME_LENGTH = 256;

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, uint Flags);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnregisterDeviceNotification(IntPtr Handle);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern int SetupDiCreateDeviceInfoList(ref Guid ClassGuid, int hwndParent);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, ref DM.SP_DEVINFO_DATA DeviceInfoData, ref Guid InterfaceClassGuid, uint MemberIndex, ref DM.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string Enumerator, int hwndParent, uint Flags);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref DM.SP_DEVINFO_DATA DeviceInfoData);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref DM.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, ref DM.SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int DeviceInterfacedetaildataSize, IntPtr DeviceInfoData);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref DM.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int DeviceInterfacedetaildataSize, IntPtr DeviceInfoData);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet, ref DM.SP_DEVINFO_DATA DeviceInfoData, StringBuilder DeviceInstanceId, int DeviceInstanceIdSize, out int RequiredSize);

		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref DM.SP_DEVINFO_DATA DeviceInfoData, int Property, ref int PrropertyRegDataType, ref char[] PropertyBuffer, int PropertyBufferSize, ref int RequireSize);
	}
}