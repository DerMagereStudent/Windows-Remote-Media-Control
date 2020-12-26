using System;
using System.Collections.Generic;
using System.Text;

namespace WRMC.Core.Models {
	/// <summary>
	/// Represents a server specific combination of a screen (visual output device) and an audio output device.
	/// </summary>
	public class Configuration {
		/// <summary>
		/// The id of the server this configuration belongs to.
		/// </summary>
		public string ServerID { get; set; }

		/// <summary>
		/// The name of the screen.
		/// </summary>
		public string Screen { get; set; }

		/// <summary>
		/// The audio output device.
		/// </summary>
		public AudioEndpoint AudioEndpoint { get; set; }
	}
}