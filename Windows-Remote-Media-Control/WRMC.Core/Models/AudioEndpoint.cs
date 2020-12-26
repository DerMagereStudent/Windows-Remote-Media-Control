using System;
using System.Collections.Generic;
using System.Text;

namespace WRMC.Core.Models {
	/// <summary>
	/// Represents a single audio output device.
	/// </summary>
	public class AudioEndpoint : IEquatable<AudioEndpoint> {
		/// <summary>
		/// The ID of the audio device.
		/// </summary>
		public string ID { get; set; }

		/// <summary>
		/// The Name of the audio device.
		/// </summary>
		public string Name { get; set; }

		public bool Equals(AudioEndpoint other) => this.ID.Equals(other.ID);
	}
}