using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace USBHelper
{
    internal class CUSB
    {
        private const ushort __KORYO_VENDOR_ID__ = 6000;

        private const ushort __KORYO_PRODUCT_ID__ = 65280;

        public const ushort __REPORT_FEATURE_COMMAND_SIZE__ = 8;

        public const ushort __REPORT_FEATURE_BLOCK_SIZE__ = 256;

        private const string __USB_IMPLEMENT_DEMO_VERSION__ = "UF1.0";

        public const byte USER_DEFINE_READ = 1;

        public const byte USER_DEFINE_WRITE = 2;

        public const byte USER_DEFINE_VERSION = 16;

        public const byte USER_DEFINE_BLC_READ = 64;

        public const byte USER_DEFINE_BLC_WRITE = 128;

        public const byte USB_ID_COMMAND = 1;

        public const byte USB_ID_DATA = 2;

        public const byte USB_ID_BIOS_DATA = 3;

        private const byte __STATUS_BYTE__ = 7;

        private static int __USB_RETRY_COUNT__ = 500;

        private IntPtr m_intPtrUSB;

        public void Open(string strPortName)
        {
            m_intPtrUSB = FIO.CreateFile(strPortName, 0, 3, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (-1 == m_intPtrUSB.ToInt32()) throw new Exception(CMessages.GetLastWin32ErrorToString());
            HID.HidD_SetNumInputBuffers(m_intPtrUSB, 0x40);
        }


        public bool SetReport(byte[] buff)
        {
            return HID.HidD_SetOutputReport(m_intPtrUSB, buff, (uint)buff.Length);
        }

        public byte[] SwitchReport(byte[] report)
        {
            var buff = new byte[] { 0x00,0xAA,0xAA,0xAA, 0x96,0x69,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,
                                                            0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,
                                                            0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,
                                                            0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,
                                                            0x00
                                                            };
            var chk_sum = (byte)(report.Length + 1);
            buff[7] = (byte)(report.Length + 1);
            for (int i = 0; i < report.Length; i++)
            {
                chk_sum ^= report[i];
                buff[8 + i] = report[i];
            }
            buff[8 + report.Length] = chk_sum;
            HID.HidD_SetOutputReport(m_intPtrUSB, buff, (uint)buff.Length);

            HID.HidD_GetInputReport(m_intPtrUSB, buff, (uint)buff.Length);
            var retbufflen = (UInt16)((buff[6] * 0x100) + buff[7]);

            var returnbuff = new List<byte>();
            returnbuff.AddRange(buff.Skip(8).ToArray());
            while (returnbuff.Count() < retbufflen)
            {
                HID.HidD_GetInputReport(m_intPtrUSB, buff, (uint)buff.Length);
                returnbuff.AddRange(buff.Skip(1));
            }

            return returnbuff.Take(retbufflen - 1).ToArray();
        }

        public byte[] SendMysteriousCMD()
        {
            /*
			关于Report ID。在Windows环境下通过ReadFile和WriteFile访问HID设备时，必须在数据开头附加1字节的Report ID（一般为0）。在Linux环境下，如果使用HID驱动的ioctl接口，那么需要在hiddev_usage_ref结构中指定Report ID；如果使用自己编写的USB驱动程序，则不需要考虑Report ID，直接发送数据就得了。
			// */
            var buff = new byte[] { 0x00,0x55,0xAA,0x04, 0x01,0xFF,0xFF,0xFA, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,
                                                            0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,
                                                            0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,
                                                            0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,
                                                            0x00
                                                            };

            HID.HidD_SetOutputReport(m_intPtrUSB, buff, (uint)buff.Length);

            HID.HidD_GetInputReport(m_intPtrUSB, buff, (uint)buff.Length);
            var returnbuff = buff.Skip(8).Take((buff[6] * 0x100) + buff[7] - 1).ToArray();
            return returnbuff;
        }

        public void Close()
        {
            FIO.CloseHandle(m_intPtrUSB);
        }

        public bool WriteBlock(byte[] blk, ref byte[] dataByte, ref byte status)
        {
            byte[] array = new byte[8], array2 = new byte[8], array3 = new byte[blk.Length + 1];
            array[0] = 1;
            array[1] = 128;
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i] = 0;
            }
            array2[0] = 1;
            array2[1] = 128;
            array[2] = dataByte[0];
            array[3] = dataByte[1];
            array[4] = dataByte[2];
            array[5] = dataByte[3];
            array[6] = dataByte[4];
            array[7] = dataByte[5];
            Array.Copy(blk, 0, array3, 1, blk.Length);
            array3[0] = 2;
            if (!HID.HidD_SetFeature(m_intPtrUSB, array, (uint)array.Length) || !HID.HidD_SetFeature(m_intPtrUSB, array3, (uint)array3.Length))
            {
                return false;
            }
            if (HID.HidD_GetFeature(m_intPtrUSB, array2, (uint)array2.Length))
            {
                status = array2[7];
                return true;
            }
            return false;
        }

        public bool ReadBlock(ref byte[] blk, ref byte[] dataByte, ref byte status)
        {
            byte[] array = new byte[8];
            byte[] array2 = new byte[8];
            byte[] array3 = new byte[blk.Length + 1];
            array[0] = 1;
            array[1] = 64;
            array[2] = dataByte[0];
            array[3] = dataByte[1];
            array[4] = dataByte[2];
            array[5] = dataByte[3];
            array[6] = dataByte[4];
            array[7] = dataByte[5];
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i] = 0;
            }
            array2[0] = 1;
            array2[1] = 64;
            for (int j = 0; j < array3.Length; j++)
            {
                array3[j] = 0;
            }
            array3[0] = 2;
            if (!HID.HidD_SetFeature(m_intPtrUSB, array, (uint)array.Length))
            {
                return false;
            }
            Thread.Sleep(1);
            if (HID.HidD_GetFeature(m_intPtrUSB, array3, (uint)array3.Length))
            {
                Array.Copy(array3, 0, blk, 0, blk.Length);
                return true;
            }
            return false;
        }

        public bool CmdWrite(byte[] dat, ref byte status)
        {
            byte[] array = new byte[dat.Length + 2];
            array[0] = 0x21;
            array[1] = 0x09;
            Array.Copy(dat, 0, array, 1, dat.Length);
            if (!HID.HidD_SetFeature(m_intPtrUSB, array, (uint)array.Length)) return false;
            Thread.Sleep(5);
            /*
			var  array2 = new byte[dat.Length + 2]
            array2[0] = 0x21;
            array2[1] = 0x09;
            for (int i = 0; i < array2.Length; i++) array2[i] = 0;
			int j;
            for (j = 0; j < CUSB.__USB_RETRY_COUNT__; j++)
            {
                if (HID.HidD_GetFeature(m_intPtrUSB, array2, (uint)array2.Length))
                {
                    status = array2[7];
                    return true;
                }
                HID.HidD_FlushQueue(m_intPtrUSB);
                Thread.Sleep(5);
            }
            if (j == CUSB.__USB_RETRY_COUNT__) throw new Exception(CMessages.GetLastWin32ErrorToString());
			
			// */
            return true;
        }

        public bool DataWrite(byte[] dat)
        {
            //Kernel32.wri
            byte[] array = new byte[8], array2 = new byte[8];
            array[0] = 1;
            array[1] = 2;
            Array.Copy(dat, 0, array, 2, dat.Length);
            for (int i = 0; i < array2.Length; i++) array2[i] = 0;
            array2[0] = 1;
            array2[1] = 2;
            if (!HID.HidD_SetFeature(m_intPtrUSB, array, (uint)array.Length)) return false;
            Thread.Sleep(5);
            int j;
            for (j = 0; j < CUSB.__USB_RETRY_COUNT__; j++)
            {
                if (HID.HidD_GetFeature(m_intPtrUSB, array2, (uint)array2.Length))
                {
                    return true;
                }
                HID.HidD_FlushQueue(m_intPtrUSB);
                Thread.Sleep(5);
            }
            if (j == CUSB.__USB_RETRY_COUNT__) throw new Exception(CMessages.GetLastWin32ErrorToString());
            return false;
        }

        public void CmdRead(ref byte[] dat)
        {
            byte[] array = new byte[8];
            byte[] array2 = new byte[8];
            array[0] = 1;
            array[1] = 1;
            Array.Copy(dat, 0, array, 2, dat.Length);
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i] = 0;
            }
            array2[0] = 1;
            array2[1] = 1;
            int j;
            for (j = 0; j < CUSB.__USB_RETRY_COUNT__; j++)
            {
                if (HID.HidD_SetFeature(m_intPtrUSB, array, (uint)array.Length) && HID.HidD_GetFeature(m_intPtrUSB, array2, (uint)array2.Length))
                {
                    Array.Copy(array2, 2, dat, 0, dat.Length);
                    break;
                }
                HID.HidD_FlushQueue(m_intPtrUSB);
            }
            if (j == CUSB.__USB_RETRY_COUNT__) throw new Exception(CMessages.GetLastWin32ErrorToString());
        }

        private static bool GetVersion(IntPtr intPtrUSB, ref string strVersion)
        {
            var result = false;
            byte[] array = new byte[8], array2 = new byte[8];
            array[0] = 1;
            array[1] = 16;
            array2[0] = 1;
            array2[1] = 16;
            for (var i = 0; i < CUSB.__USB_RETRY_COUNT__; i++)
            {
                if (HID.HidD_SetFeature(intPtrUSB, array, (uint)array.Length) && HID.HidD_GetFeature(intPtrUSB, array2, (uint)array2.Length))
                {
                    strVersion = string.Format("{0:c}{1:c}{2:c}{3:c}{4:c}", new object[]
                    {
                                        (char)array2[2],
                                        (char)array2[3],
                                        (char)array2[4],
                                        (char)array2[5],
                                        (char)array2[6]
                    });
                    result = true;
                    break;
                }
                HID.HidD_FlushQueue(intPtrUSB);
                Thread.Sleep(1);
            }
            return result;
        }

        private static string[] GetHidDevInterface(ushort uVendorId, ushort uProductId)
        {
            string[] allHIDInterFace;
            try
            {
                allHIDInterFace = CUSB.GetAllHIDInterFace();
                if (allHIDInterFace == null || allHIDInterFace.Length == 0) return null;
            }
            catch
            {
                return null;
            }
            var arrayList = new List<string>();
            for (int i = 0; i < allHIDInterFace.Length; i++)
            {
                var intPtr = FIO.CreateFile(allHIDInterFace[i], FIO.GENERIC_READ, 3u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
                if (-1 == intPtr.ToInt32())
                {
                    var err = CMessages.GetLastWin32ErrorToString();
                    //Console.WriteLine(err);
                    continue;
                }

                var hIDD_ATTRIBUTES = default(HID.HIDD_ATTRIBUTES);
                hIDD_ATTRIBUTES.Size = (uint)Marshal.SizeOf(hIDD_ATTRIBUTES);
                if (HID.HidD_GetAttributes(intPtr, ref hIDD_ATTRIBUTES) && uVendorId == hIDD_ATTRIBUTES.VendorID && uProductId == hIDD_ATTRIBUTES.ProductID) arrayList.Add(allHIDInterFace[i]);
                FIO.CloseHandle(intPtr);

            }
            return arrayList.ToArray();
        }

        private static string[] GetAllHIDInterFace()
        {

            var arrayList = new List<string>();
            try
            {
                var sYSTEM_INFO = default(Kernel32.SYSTEM_INFO);
                Kernel32.GetSystemInfo(out sYSTEM_INFO);
                bool flag = sYSTEM_INFO.sysInfoEx.wProcessorArchitecture == 9 || sYSTEM_INFO.sysInfoEx.wProcessorArchitecture == 6;
                Guid guid = default(Guid);
                HID.HidD_GetHidGuid(ref guid);
                var sP_DEVINFO_DATA = default(DM.SP_DEVINFO_DATA);
                sP_DEVINFO_DATA.cbSize = Marshal.SizeOf(sP_DEVINFO_DATA);
                var deviceInfoSet = DM.SetupDiGetClassDevs(ref guid, null, 0, 18u);
                if (!Environment.Is64BitOperatingSystem)
                {
                    throw new Exception(CMessages.GetLastWin32ErrorToString());
                }
                else if (-1L == deviceInfoSet.ToInt64())
                {
                    throw new Exception(CMessages.GetLastWin32ErrorToString());
                }
                uint num = 0;
                while (DM.SetupDiEnumDeviceInfo(deviceInfoSet, num, ref sP_DEVINFO_DATA))
                {
                    var sP_DEVICE_INTERFACE_DATA = default(DM.SP_DEVICE_INTERFACE_DATA);
                    sP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(sP_DEVICE_INTERFACE_DATA);
                    uint num2 = 0;
                    while (DM.SetupDiEnumDeviceInterfaces(deviceInfoSet, ref sP_DEVINFO_DATA, ref guid, num2++, ref sP_DEVICE_INTERFACE_DATA))
                    {
                        int num3 = 0;
                        if (!DM.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref sP_DEVICE_INTERFACE_DATA, IntPtr.Zero, 0, ref num3, IntPtr.Zero) && 122 != Marshal.GetLastWin32Error()) throw new Exception(CMessages.GetLastWin32ErrorToString());
                        int num4 = 0;
                        var intPtr = Marshal.AllocHGlobal(num3);
                        Marshal.WriteInt32(intPtr, flag ? 8 : (Marshal.SizeOf(0u) + Marshal.SystemDefaultCharSize));
                        if (!DM.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref sP_DEVICE_INTERFACE_DATA, intPtr, num3, ref num4, IntPtr.Zero)) throw new Exception(CMessages.GetLastWin32ErrorToString());
                        var ptr = new IntPtr(Environment.Is64BitOperatingSystem ? intPtr.ToInt64() + (long)Marshal.SizeOf(0u) : intPtr.ToInt32() + Marshal.SizeOf(0u));
                        arrayList.Add(Marshal.PtrToStringAuto(ptr));
                        Marshal.FreeHGlobal(intPtr);
                    }
                    num += 1;
                }
                DM.SetupDiDestroyDeviceInfoList(deviceInfoSet);
            }
            catch (Exception e)
            {
                throw e;
            }
            return arrayList.ToArray();
        }

        public static string[] _EnumPorts_(UInt16 vid, UInt16 pid)
        {
            var arrayList = new List<string>();
            var hidDevInterface = CUSB.GetHidDevInterface(vid, pid);
            if (hidDevInterface == null) return arrayList.ToArray();
            for (int i = 0; i < hidDevInterface.Length; i++)
            {
                var intPtr = FIO.CreateFile(hidDevInterface[i].Trim(), FIO.GENERIC_READ, 3u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
                if (intPtr.ToInt32() == -1) continue;
                //var a = ""; 
                //if (CUSB.GetVersion(intPtr, ref a)) if (a == "UF1.0") 
                arrayList.Add(hidDevInterface[i]);

                FIO.CloseHandle(intPtr);
            }
            return arrayList.ToArray();
        }
    }
}