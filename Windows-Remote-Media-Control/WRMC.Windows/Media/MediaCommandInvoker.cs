using WRMC.Core.Models;

namespace WRMC.Windows.Media {
	public abstract class MediaCommandInvoker {
		public static MediaCommandInvoker Default { get; set; }

		public abstract void Play(MediaSession session);
		public abstract void Pause(MediaSession session);
		public abstract void SkipNext(MediaSession session);
		public abstract void SkipPrevious(MediaSession session);
		public abstract void MoveToScreen(MediaSession session, string screen);
		public abstract void SetAudioEndpoint(MediaSession session, AudioEndpoint endpoint);
		public abstract void SetVolume(MediaSession session, int volume);
	}
}