using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Provider;

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

using WRMC.Core.Networking;

namespace WRMC.Android {
	public static class DeviceInformation {
		public static string DeviceName => Xamarin.Essentials.DeviceInfo.Name;

		public static IPAddress GetIPAddress(Context context) {
			return new IPAddress((context.GetSystemService(Context.WifiService) as WifiManager).ConnectionInfo.IpAddress);
		}

		public static string GetDevideId(Context context) {
			return Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
		}

		public static ClientDevice GetClientDevice(Context context) {
			return new ClientDevice() {
				ID = DeviceInformation.GetDevideId(context),
				Name = DeviceInformation.DeviceName,
				IPAddress = DeviceInformation.GetIPAddress(context)
			};
		}
	}
}