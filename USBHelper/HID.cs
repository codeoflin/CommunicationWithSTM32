using System;
using System.Runtime.InteropServices;

namespace USBHelper
{
	internal sealed class HID
	{
		public enum Report_Type
		{
			HidP_Input,
			HidP_Output,
			HidP_Feature
		}

		public struct HIDD_ATTRIBUTES
		{
			public uint Size;

			public ushort VendorID;

			public ushort ProductID;

			public ushort VersionNumber;
		}

		public struct HIDP_CAPS
		{
			public ushort Usage;

			public ushort UsagePage;

			public ushort InputReportByteLength;

			public ushort OutputReportByteLength;

			public ushort FeatureReportByteLength;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
			public ushort[] Reserved;

			public ushort NumberLinkCollectionNodes;

			public ushort NumberInputButtonCaps;

			public ushort NumberInputValueCaps;

			public ushort NumberInputDataIndices;

			public ushort NumberOutputButtonCaps;

			public ushort NumberOutputValueCaps;

			public ushort NumberOutputDataIndices;

			public ushort NumberFeatureButtonCaps;

			public ushort NumberFeatureValueCaps;

			public ushort NumberFeatureDataIndices;
		}

		public struct HIDP_VALUE_CAPS
		{
			public ushort UsagePage;

			public byte ReportID;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsAlias;

			public ushort BitField;

			public ushort LinkCollection;

			public ushort LinkUsage;

			public ushort LinkUsagePage;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsRange;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsStringRange;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsDesignatorRange;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsAbsolute;

			[MarshalAs(UnmanagedType.U1)]
			public bool HasNull;

			public byte Reserved;

			public ushort BitSize;

			public ushort ReportCount;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
			public ushort[] Reserved2;

			public uint UnitsExp;

			public uint Units;

			public int LogicalMin;

			public int LogicalMax;

			public int PhysicalMin;

			public int PhysicalMax;

			public ushort UsageMin_or_Usage;

			public ushort UsageMax_or_Reserved1;

			public ushort StringMin_or_StringIndex;

			public ushort StringMax_or_Reserved2;

			public ushort DesignatorMin_or_DesignatorIndex;

			public ushort DesignatorMax_or_Reserved3;

			public ushort DataIndexMin_or_DataIndex;

			public ushort DataIndexMax_or_Reserved4;
		}

		public struct HIDP_BUTTON_CAPS
		{
			public ushort UsagePage;

			public byte ReportID;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsAlias;

			public ushort BitField;

			public ushort LinkCollection;

			public ushort LinkUsage;

			public ushort LinkUsagePage;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsRange;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsStringRange;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsDesignatorRange;

			[MarshalAs(UnmanagedType.U1)]
			public bool IsAbsolute;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
			public uint[] Reserved;

			public ushort UsageMin_or_Usage;

			public ushort UsageMax_or_Reserved1;

			public ushort StringMin_or_StringIndex;

			public ushort StringMax_or_Reserved2;

			public ushort DesignatorMin_or_DesignatorIndex;

			public ushort DesignatorMax_or_Reserved3;

			public ushort DataIndexMin_or_DataIndex;

			public ushort DataIndexMax_or_Reserved4;
		}

		public struct HIDP_LINK_COLLECTION_NODE
		{
			public ushort LinkUsage;

			public ushort LinkUsagePage;

			public ushort Parent;

			public ushort NumberOfChildren;

			public ushort NextSibling;

			public ushort FirstChild;

			public uint CollectionType_IsAlias_Reserved;

			public IntPtr UserContext;
		}

		public static int HIDP_STATUS_SUCCESS
		{
			get
			{
				return HID.HIDP_ERROR_CODES(0, 0);
			}
		}

		public static int HIDP_STATUS_NULL
		{
			get
			{
				return HID.HIDP_ERROR_CODES(8, 1);
			}
		}

		public static int HIDP_STATUS_INVALID_PREPARSED_DATA
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 1);
			}
		}

		public static int HIDP_STATUS_INVALID_REPORT_TYPE
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 2);
			}
		}

		public static int HIDP_STATUS_INVALID_REPORT_LENGTH
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 3);
			}
		}

		public static int HIDP_STATUS_USAGE_NOT_FOUND
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 4);
			}
		}

		public static int HIDP_STATUS_VALUE_OUT_OF_RANGE
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 5);
			}
		}

		public static int HIDP_STATUS_BAD_LOG_PHY_VALUES
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 6);
			}
		}

		public static int HIDP_STATUS_BUFFER_TOO_SMALL
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 7);
			}
		}

		public static int HIDP_STATUS_INTERNAL_ERROR
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 8);
			}
		}

		public static int HIDP_STATUS_I8042_TRANS_UNKNOWN
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 9);
			}
		}

		public static int HIDP_STATUS_INCOMPATIBLE_REPORT_ID
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 10);
			}
		}

		public static int HIDP_STATUS_NOT_VALUE_ARRAY
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 11);
			}
		}

		public static int HIDP_STATUS_IS_VALUE_ARRAY
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 12);
			}
		}

		public static int HIDP_STATUS_DATA_INDEX_NOT_FOUND
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 13);
			}
		}

		public static int HIDP_STATUS_DATA_INDEX_OUT_OF_RANGE
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 14);
			}
		}

		public static int HIDP_STATUS_BUTTON_NOT_PRESSED
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 15);
			}
		}

		public static int HIDP_STATUS_REPORT_DOES_NOT_EXIST
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 16);
			}
		}

		public static int HIDP_STATUS_NOT_IMPLEMENTED
		{
			get
			{
				return HID.HIDP_ERROR_CODES(12, 32);
			}
		}

		private static int HIDP_ERROR_CODES(int SEV, int CODE)
		{
			return SEV << 28 | 1114112 | CODE;
		}

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_FlushQueue(IntPtr HidDeviceObject);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_FreePreparsedData(ref IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetAttributes(IntPtr HidDeviceObject, ref HID.HIDD_ATTRIBUTES Attributes);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetFeature(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetInputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern void HidD_GetHidGuid(ref Guid HidGuid);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetNumInputBuffers(IntPtr HidDeviceObject, ref int NumberBuffers);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetPreparsedData(IntPtr HidDeviceObject, ref IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_SetFeature(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_SetNumInputBuffers(IntPtr HidDeviceObject, int NumberBuffers);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_SetOutputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);

		[DllImport("hid.dll")]
		public static extern int HidP_GetCaps(IntPtr PreparsedData, ref HID.HIDP_CAPS Capabilities);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern int HidP_GetValueCaps(HID.Report_Type ReportType, [In] [Out] HID.HIDP_VALUE_CAPS[] ValueCaps, ref uint ValueCapsLength, IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern int HidP_GetButtonCaps(HID.Report_Type ReportType, [In] [Out] HID.HIDP_BUTTON_CAPS[] ValueCaps, ref uint ValueCapsLength, IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern int HidP_GetSpecificValueCaps(HID.Report_Type ReportType, ushort UsagePage, ushort LinkCollection, ushort Usage, [In] [Out] HID.HIDP_VALUE_CAPS[] ValueCaps, ref uint ValueCapsLength, IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetManufacturerString(IntPtr HidDeviceObject, IntPtr Buffer, uint BufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetProductString(IntPtr HidDeviceObject, IntPtr Buffer, uint BufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetSerialNumberString(IntPtr HidDeviceObject, IntPtr Buffer, uint BufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetIndexedString(IntPtr HidDeviceObject, uint StringIndex, IntPtr Buffer, uint BufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern int HidP_GetLinkCollectionNodes([In] [Out] HID.HIDP_LINK_COLLECTION_NODE[] LinkCollectionNodes, ref uint LinkCollectionNodesLength, IntPtr PreparsedData);

	}
}
