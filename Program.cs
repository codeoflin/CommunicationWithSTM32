using System;
using System.Drawing;
using USBHelper;

namespace Hasee_z7m_kp7gc_LEDConfig
{
	class Program
	{
		static void Main(string[] args)
		{
			var te = CUSB._EnumPorts_(0x0001, 0x5753);
			var data = new byte[] { 0x04, 0x01, 0x00, 0x00, 0x00, 0x00};
			var m_usb = new CUSB();
			byte b = 0;
			foreach (var t in te)
			{
				m_usb.Open(t);
				m_usb.CmdWrite(data,ref b);
				m_usb.Close();
			}
		}
	}//End class
}
