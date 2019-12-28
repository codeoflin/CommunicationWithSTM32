using System;
using System.Drawing;
using USBHelper;

namespace CommunicationWithSTM32
{
	class Program
	{
		static void Test()
		{
			//Get the details of all connected USB HID devices
			var devices = HIDDevice.getConnectedDevices();

			//Arbitrarily select one of the devices which we found in the previous step
			//record the details of this device to be used in the class constructor
			int selectedDeviceIndex = -1;
			for (int i = 0; i < devices.Length; i++)
			{
				if (devices[i].VID == 0x0001 && devices[i].PID == 0x5753) selectedDeviceIndex = i;
			}
			ushort VID = devices[selectedDeviceIndex].VID;
			ushort PID = devices[selectedDeviceIndex].PID;
			int SN = devices[selectedDeviceIndex].serialNumber;
			string devicePath = devices[selectedDeviceIndex].devicePath;

			//create a handle to the device by calling the constructor of the HID class
			//This can be done using either the VID/PID/Serialnumber, or the device path (string) 
			//all of these details are available from the HIDDevice.interfaceDetails[] struct array created above
			//The "true" boolean in the constructor tells the class we want asynchronous operation this time
			var device = new HIDDevice(devicePath, false);
			//OR, the normal usage when you know the VID and PID of the device
			//HIDDevice device = new HIDDevice(VID, PID, (ushort)SN, true);

			//next create the event handler for the incoming reports
			//device.dataReceived += new HIDDevice.dataReceivedEvent(device_dataReceived);

			//Write some data to the device (the write method throws an exception if the data is longer than the report length
			//specified by the device, this length can be found in the HIDDevice.interfaceDetails struct)
			//The write method is identical to the synchronous mode of operation
			byte[] writeData = { 0x04, 0x01, 0x02, 0x03, 0x04 };
			device.write(writeData);    //Its that easy!!

			//Send your program off to do other stuff here, wait for UI events etc
			//When a report occurs, the device_dataReceived(byte[] message) method will be called
			System.Threading.Thread.Sleep(100);

			//close the device to release all handles etc
			device.close();
		}
		static void Main(string[] args)
		{
			var te = CUSB.EnumPorts(0x0001, 0x5753);
			var data = new byte[] { 0x04, 0x01, 0x00, 0x00, 0x00, 0x00 };
			var m_usb = new CUSB();
			byte b = 0;
			foreach (var t in te)
			{
				m_usb.Open(t);
				m_usb.CmdWrite(data, ref b);
				m_usb.Close();
			}
		}
	}//End class
}
