using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using WRMC.Core;
using WRMC.Core.Models;
using WRMC.Windows.Properties;

namespace WRMC.Windows.Controls {
	public partial class MediaSessionControl : UserControl {
		private static Image PREVIOUS_IMAGE = Resources.icons8_previous_50;
		private static Image NEXT_IMAGE = Resources.icons8_next_50;
		private static Image PLAY_IMAGE = Resources.icons8_play_50;
		private static Image PAUSE_IMAGE = Resources.icons8_pause_32;

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

				this.labelProcessID.Text = this._mediaSession.ProcessID.ToString();
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

			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.buttonNext.BackgroundImage = NEXT_IMAGE;
			this.buttonPrevious.BackgroundImage = PREVIOUS_IMAGE;
			this.buttonPlayPause.BackgroundImage = PLAY_IMAGE;

		this.buttonPlayPause.Click += (s, e) => { this.OnPlayPause?.Invoke(this, EventArgs.Empty); };
			this.buttonNext.Click += (s, e) => { this.OnNext?.Invoke(this, EventArgs.Empty); };
			this.buttonPrevious.Click += (s, e) => { this.OnPrevious?.Invoke(this, EventArgs.Empty); };
		}
	}
}