using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Provider;

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

using WRMC.Core.Networking;

namespace WRMC.Android {
	/// <summary>
	/// Static class to read out the necessary device information.
	/// </summary>
	public static class DeviceInformation {
		public static string DeviceName => Xamarin.Essentials.DeviceInfo.Name;

		/// <summary>
		/// Reads out the local IPv4 address of the device.
		/// </summary>
		/// <returns></returns>
		public static IPAddress GetIPAddress() {
			//return new IPAddress((context.GetSystemService(Context.WifiService) as WifiManager).ConnectionInfo.IpAddress);
			foreach (NetworkInterface nInterface in NetworkInterface.GetAllNetworkInterfaces()) {
				if ((nInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || nInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) && nInterface.OperationalStatus == OperationalStatus.Up)
					foreach (var ip in nInterface.GetIPProperties().UnicastAddresses)
						if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
							return ip.Address;
			}

			return IPAddress.None;
		}

		public static string GetDevideId(Context context) {
			return Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
		}

		public static ClientDevice GetClientDevice(Context context) {
			return new ClientDevice() {
				ID = DeviceInformation.GetDevideId(context),
				Name = DeviceInformation.DeviceName,
				IPAddress = DeviceInformation.GetIPAddress()
			};
		}
	}
}