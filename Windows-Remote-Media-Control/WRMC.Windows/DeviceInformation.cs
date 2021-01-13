using System;
using System.Net;
using System.Net.NetworkInformation;

using Windows.System.UserProfile;

using WRMC.Core.Networking;

namespace WRMC.Windows {
	public static class DeviceInformation {
		public static string DeviceName => Environment.MachineName;
		public static string DevideId => AdvertisingManager.AdvertisingId;
		public static IPAddress IPAddress {
			get {
				foreach (NetworkInterface nInterface in NetworkInterface.GetAllNetworkInterfaces()) {
					if (nInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nInterface.OperationalStatus == OperationalStatus.Up)
						foreach (var ip in nInterface.GetIPProperties().UnicastAddresses)
							if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
								return ip.Address;
				}

				return IPAddress.None;
			}
		}

		public static ServerDevice ServerDevice {
			get => new ServerDevice() {
				ID = DeviceInformation.DevideId,
				Name = DeviceInformation.DeviceName,
				IPAddress = DeviceInformation.IPAddress
			};
		}
	}
}