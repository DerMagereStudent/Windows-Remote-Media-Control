using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

using Windows.System;
using Windows.System.Profile;
using Windows.Storage.Streams;

using WRMC.Core.Networking;
using System.Net.Sockets;

namespace WRMC.Windows {
	public static class DeviceInformation {
		public static string DeviceName => Environment.MachineName;

		public static string DevideId {
			get {
				IBuffer iBuffer = SystemIdentification.GetSystemIdForUser(null).Id;
				byte[] buffer = new byte[iBuffer.Length];
				DataReader.FromBuffer(iBuffer).ReadBytes(buffer);
				return Convert.ToBase64String(buffer, Base64FormattingOptions.None);
			}
		}

		public static IPAddress IPAddress {
			get {
				foreach (NetworkInterface nInterface in NetworkInterface.GetAllNetworkInterfaces()) {
					if ((nInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || nInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) && nInterface.OperationalStatus == OperationalStatus.Up)
						foreach (var ip in nInterface.GetIPProperties().UnicastAddresses)
							if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
								return ip.Address;
				}

				return IPAddress.Parse("123.123.123.123");
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