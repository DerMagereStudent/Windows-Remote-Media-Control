using System;
using System.Collections.Generic;

using WRMC.Core.Models;

namespace WRMC.Windows.Media {
	public abstract class MediaCommandInvoker {
		private static Dictionary<Type, MediaCommandInvoker> registeredInvokers;

		public static MediaCommandInvoker Default { get; set; }
		public static List<Type> RegisteredExtractors => new List<Type>(registeredInvokers.Keys);

		static MediaCommandInvoker() {
			registeredInvokers = new Dictionary<Type, MediaCommandInvoker>();
			registeredInvokers.Add(typeof(TransportControlsMediaSessionExtractor), new TransportControlsMediaCommandInvoker());
		}

		public static void SetInvoker(Type type) {
			if (!registeredInvokers.ContainsKey(type))
				return;

			Default = registeredInvokers[type];
		}

		public abstract void Play(MediaSession session);
		public abstract void Pause(MediaSession session);
		public abstract void SkipNext(MediaSession session);
		public abstract void SkipPrevious(MediaSession session);
		public abstract void MoveToScreen(MediaSession session, string screen);
		public abstract void SetAudioEndpoint(MediaSession session, AudioEndpoint endpoint);
		public abstract void SetVolume(MediaSession session, int volume);
	}
}