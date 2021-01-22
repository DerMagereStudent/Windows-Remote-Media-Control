using Android.OS;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;

using WRMC.Android.Networking;
using WRMC.Core;
using WRMC.Core.Models;
using WRMC.Core.Networking;

namespace WRMC.Android.Views {
	public class MediaSessionFragment : BackButtonNotifiableFragment {
		private ImageView imageViewMediaType;

		private TextView textViewTitle;
		private TextView textViewArtist;

		private ImageButton buttonPlayPause;
		private ImageButton buttonSkipNext;
		private ImageButton buttonSkipPrevious;

		public MediaSession MediaSession { get; set; }

		public MediaSessionFragment(MediaSession session) {
			this.MediaSession = session;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.media_session, container, false);

			this.imageViewMediaType = view.FindViewById<ImageView>(Resource.Id.media_session_image_view_media_type);

			this.textViewTitle = view.FindViewById<TextView>(Resource.Id.media_session_text_view_title);
			this.textViewArtist= view.FindViewById<TextView>(Resource.Id.media_session_text_view_artist);

			this.buttonPlayPause = view.FindViewById<ImageButton>(Resource.Id.media_session_controls_button_play_pause);
			this.buttonSkipNext = view.FindViewById<ImageButton>(Resource.Id.media_session_controls_button_skip_next);
			this.buttonSkipPrevious = view.FindViewById<ImageButton>(Resource.Id.media_session_controls_button_skip_previous);

			this.buttonPlayPause.Click += this.ButtonPlayPause_Click;
			this.buttonSkipNext.Click += this.ButtonSkipNext_Click;
			this.buttonSkipPrevious.Click += this.ButtonSkipPrevious_Click;

			ConnectionManager.OnMediaSessionsReceived += this.ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged += this.ConnectionManager_OnMediaSessionChanged;
			ConnectionManager.OnConnectionClosed += this.ConnectionManager_OnConnectionClosed;

			this.UpdateUI();

			return view;
		}

		private void ButtonPlayPause_Click(object sender, EventArgs e) {
			ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
				Method = WRMC.Core.Networking.Message.Type.PlayPause,
				Body = new MediaSessionMessageBody() {
					MediaSession = this.MediaSession,
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
				}
			});
		}

		private void ButtonSkipNext_Click(object sender, EventArgs e) {
			ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
				Method = WRMC.Core.Networking.Message.Type.NextMedia,
				Body = new MediaSessionMessageBody() {
					MediaSession = this.MediaSession,
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
				}
			});
		}

		private void ButtonSkipPrevious_Click(object sender, EventArgs e) {
			ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
				Method = WRMC.Core.Networking.Message.Type.PreviousMedia,
				Body = new MediaSessionMessageBody() {
					MediaSession = this.MediaSession,
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
				}
			});
		}

		public override void OnBackButton() {
			ConnectionManager.OnMediaSessionsReceived -= this.ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged -= this.ConnectionManager_OnMediaSessionChanged;
			ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;
		}

		private void ConnectionManager_OnMediaSessionsReceived(object sender, EventArgs<List<MediaSession>> e) {
			if (!e.Data.Contains(this.MediaSession))
				this.Activity.RunOnUiThread(() => (this.Activity as MainActivity).OnBackPressed());
		}

		private void ConnectionManager_OnMediaSessionChanged(object sender, EventArgs<MediaSession> e) {
			if (e.Data.Equals(this.MediaSession)) {
				this.MediaSession = e.Data;
				this.UpdateUI();
			}
		}

		private void ConnectionManager_OnConnectionClosed(object sender, EventArgs e) {
			ConnectionManager.OnMediaSessionsReceived -= this.ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged -= this.ConnectionManager_OnMediaSessionChanged;
			ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;

			this.Activity.RunOnUiThread(() => {
				(this.Activity as MainActivity).SupportFragmentManager.PopBackStackImmediate();
				(this.Activity as MainActivity).OnBackPressed();
			});
		}

		public void UpdateUI() {
			this.Activity.RunOnUiThread(() => {
				if (this.MediaSession == null)
					return;

				this.imageViewMediaType.SetImageResource(this.MediaSession.Type == MediaSession.MediaType.Video ? Resource.Drawable.video : Resource.Drawable.sound_waves);
				this.textViewTitle.Text = this.MediaSession.Title;
				this.textViewArtist.Text = this.MediaSession.Artist;
				this.buttonPlayPause.SetImageResource(this.MediaSession.State == MediaSession.PlaybackState.Playing ? Resource.Drawable.pause : Resource.Drawable.play);
			});
		}
	}
}