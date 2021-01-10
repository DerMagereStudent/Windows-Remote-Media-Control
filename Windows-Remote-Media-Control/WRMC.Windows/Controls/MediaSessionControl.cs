using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using WRMC.Core;
using WRMC.Core.Models;
using WRMC.Windows.Media;
using WRMC.Windows.Properties;

namespace WRMC.Windows.Controls {
	public partial class MediaSessionControl : UserControl {
		private static Image PREVIOUS_IMAGE = Resources.icons8_previous_50;
		private static Image NEXT_IMAGE = Resources.icons8_next_50;
		private static Image PLAY_IMAGE = Resources.icons8_play_50;
		private static Image PAUSE_IMAGE = Resources.icons8_pause_32;

		private static Image PREVIOUS_IMAGE_ACTIVE = Resources.icons8_previous_50_active;
		private static Image NEXT_IMAGE_ACTIVE = Resources.icons8_next_50_active;
		private static Image PLAY_IMAGE_ACTIVE = Resources.icons8_play_50_active;
		private static Image PAUSE_IMAGE_ACTIVE = Resources.icons8_pause_32_active;

		private MediaSession _mediaSession;

		protected override CreateParams CreateParams {
			get {
				CreateParams param = base.CreateParams;
				param.ExStyle |= 0x02000000;
				return param;
			}
		}

		public MediaSession MediaSession {
			get => this._mediaSession;
			set {
				this._mediaSession = value;

				if (value == null)
					return;

				this.labelProcessID.Text = string.Join(", ", this._mediaSession.ProcessIDs);
				this.labelProcessName.Text = new FileInfo(this._mediaSession.ProcessName).Name;
				this.labelTitle.Text = this._mediaSession.Title;
				this.labelArtist.Text = this._mediaSession.Artist;

				this.buttonPlayPause.BackgroundImage = this._mediaSession.State == MediaSession.PlaybackState.Playing ? PAUSE_IMAGE : PLAY_IMAGE;
			}
		}

		public event TypedEventHandler<MediaSessionControl, EventArgs> OnPlayPause = null;
		public event TypedEventHandler<MediaSessionControl, EventArgs> OnNext = null;
		public event TypedEventHandler<MediaSessionControl, EventArgs> OnPrevious = null;

		public MediaSessionControl() {
			this.InitializeComponent();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.buttonNext.BackgroundImage = NEXT_IMAGE;
			this.buttonPrevious.BackgroundImage = PREVIOUS_IMAGE;
			this.buttonPlayPause.BackgroundImage = PLAY_IMAGE;

			this.buttonPlayPause.Click += (s, e) => {
				if (this.MediaSession == null)
					return;

				if (this.MediaSession.State == MediaSession.PlaybackState.Playing)
					MediaCommandInvoker.Default.Pause(this.MediaSession);
				else if (this.MediaSession.State == MediaSession.PlaybackState.Paused)
					MediaCommandInvoker.Default.Play(this.MediaSession);

				this.OnPlayPause?.Invoke(this, EventArgs.Empty);
			};

			this.buttonPlayPause.MouseEnter += (s, e) => {
				if (this.MediaSession.State == MediaSession.PlaybackState.Playing)
					this.buttonPlayPause.BackgroundImage = PAUSE_IMAGE_ACTIVE;
				else if (this.MediaSession.State == MediaSession.PlaybackState.Paused)
					this.buttonPlayPause.BackgroundImage = PLAY_IMAGE_ACTIVE;
			};

			this.buttonPlayPause.MouseLeave += (s, e) => {
				if (this.MediaSession.State == MediaSession.PlaybackState.Playing)
					this.buttonPlayPause.BackgroundImage = PAUSE_IMAGE;
				else if (this.MediaSession.State == MediaSession.PlaybackState.Paused)
					this.buttonPlayPause.BackgroundImage = PLAY_IMAGE;
			};

			this.buttonNext.Click += (s, e) => {
				MediaCommandInvoker.Default.SkipNext(this.MediaSession);
				this.OnNext?.Invoke(this, EventArgs.Empty);
			};

			this.buttonNext.MouseEnter += (s, e) => {
				this.buttonNext.BackgroundImage = NEXT_IMAGE_ACTIVE;
			};

			this.buttonNext.MouseLeave += (s, e) => {
				this.buttonNext.BackgroundImage = NEXT_IMAGE;
			};

			this.buttonPrevious.Click += (s, e) => {
				MediaCommandInvoker.Default.SkipPrevious(this.MediaSession);
				this.OnPrevious?.Invoke(this, EventArgs.Empty);
			};

			this.buttonPrevious.MouseEnter += (s, e) => {
				this.buttonPrevious.BackgroundImage = PREVIOUS_IMAGE_ACTIVE;
			};

			this.buttonPrevious.MouseLeave += (s, e) => {
				this.buttonPrevious.BackgroundImage = PREVIOUS_IMAGE;
			};
		}

		public MediaSessionControl(MediaSession session) : this() {
			this.MediaSession = session;
		}
	}
}