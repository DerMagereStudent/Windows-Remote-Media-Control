﻿using System;
using System.Collections.Generic;

using WRMC.Core.Models;

namespace WRMC.Windows.Media {
	public abstract class MediaCommandInvoker {
		private static Dictionary<Type, MediaCommandInvoker> registeredInvokers;

		public static MediaCommandInvoker Default { get; set; }
		public static List<Type> RegisteredInvokers => new List<Type>(registeredInvokers.Keys);

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
		public abstract void SetMasterVolume(int volume);
		public abstract void SetVolume(MediaSession session, int volume);
		public abstract void SetConfiguration(MediaSession session, Configuration configuration);
		public abstract void ResumeSuspendedProcess(SuspendedProcess suspendedProcess);

		public abstract void PlayFile(string filePath);

		public abstract List<string> GetScreens();
		public abstract List<AudioEndpoint> GetAudioEndpoints();
		public abstract int GetMasterVolume();
		public abstract int GetVolume(MediaSession session);
		public abstract byte[] GetThumbnail(MediaSession session);
		public abstract List<SuspendedProcess> GetSuspendedProcesses();
		public abstract List<DirectoryItem> GetDirectoryContent(string directory);
	}
}