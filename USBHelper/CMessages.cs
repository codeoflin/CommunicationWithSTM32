using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace USBHelper
{
    public class CMessages
    {
        private static string[] SMBUS_ERRCODE_TABLE = new string[]
        {
        "done",
        "address error",
        "no ack",
        "arbitration",
        "error command",
        "timeout",
        "busy",
        "unknown error"
        };

        private static string[] STRING_TABLE = new string[]
        {
        "Start to verification !",
        "Write Block",
        "Read Block",
        "Verify Block",
        "Verify Block Done !",
        "Invalid Buffer !",
        "The Port is Unavailable !",
        "USB has been connected ",
        "USB has been removed !",
        "Invalid Report ID !",
        "Invalid Command !",
        "To Intialize all Ports !"
        };

        public static string Get(int id)
        {
            if (id < CMessages.STRING_TABLE.Length)
            {
                return CMessages.STRING_TABLE[id];
            }
            return "Unknow Message - " + string.Format("ID = {}", id);
        }

        public static string GetSmbusErrCode(int id)
        {
            if (id <= 16)
            {
                switch (id)
                {
                    case 0:
                        return CMessages.SMBUS_ERRCODE_TABLE[0];
                    case 1:
                        return CMessages.SMBUS_ERRCODE_TABLE[1];
                    case 2:
                        return CMessages.SMBUS_ERRCODE_TABLE[2];
                    case 3:
                    case 5:
                    case 6:
                    case 7:
                        break;
                    case 4:
                        return CMessages.SMBUS_ERRCODE_TABLE[3];
                    case 8:
                        return CMessages.SMBUS_ERRCODE_TABLE[4];
                    default:
                        if (id == 16)
                        {
                            return CMessages.SMBUS_ERRCODE_TABLE[5];
                        }
                        break;
                }
            }
            else
            {
                if (id == 32)
                {
                    return CMessages.SMBUS_ERRCODE_TABLE[6];
                }
                if (id != 255)
                {
                }
            }
            return string.Format("SMBUS ERRCODE={0:X2}", id);
        }

        public static string GetLastWin32ErrorToString()
        {
            int lastWin32Error = Marshal.GetLastWin32Error();
            string message = new Win32Exception(lastWin32Error).Message;
            return string.Format("{0}(Error Code : {1})", message, lastWin32Error);
        }
    }
}