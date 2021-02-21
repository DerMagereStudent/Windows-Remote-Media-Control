using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
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
		private TextView textViewVolume;
		private SeekBar seekBarVolume;

		private ImageView imageViewMediaType;

		private TextView textViewTitle;
		private TextView textViewArtist;

		private ImageButton buttonPlayPause;
		private ImageButton buttonSkipNext;
		private ImageButton buttonSkipPrevious;

		private ImageButton buttonCollapseExpand;

		public MediaSession MediaSession { get; set; }

		public MediaSessionFragment(MediaSession session) {
			this.MediaSession = session;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.media_session, container, false);

			BottomSheetBehavior bottomSheetBehavior = BottomSheetBehavior.From(view.FindViewById(Resource.Id.media_session_controls));

			this.imageViewMediaType = view.FindViewById<ImageView>(Resource.Id.media_session_image_view_media_type);

			this.textViewTitle = view.FindViewById<TextView>(Resource.Id.media_session_text_view_title);
			this.textViewArtist= view.FindViewById<TextView>(Resource.Id.media_session_text_view_artist);

			this.buttonPlayPause = view.FindViewById<ImageButton>(Resource.Id.media_session_controls_button_play_pause);
			this.buttonSkipNext = view.FindViewById<ImageButton>(Resource.Id.media_session_controls_button_skip_next);
			this.buttonSkipPrevious = view.FindViewById<ImageButton>(Resource.Id.media_session_controls_button_skip_previous);

			this.buttonCollapseExpand = view.FindViewById<ImageButton>(Resource.Id.media_session_controls_button_expand_collapse);

			int bottom = this.buttonPlayPause.Bottom;
			bottomSheetBehavior.PeekHeight = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 150, this.Resources.DisplayMetrics);

			MultiBottomSheetCallback bottomSheetCallback = new MultiBottomSheetCallback();

			bottomSheetCallback.OnSlideEvent += (s, e) => {
				this.buttonCollapseExpand.Rotation = e.Data * -180.0f;
			};

			bottomSheetBehavior.SetBottomSheetCallback(bottomSheetCallback);

			this.buttonPlayPause.Click += this.ButtonPlayPause_Click;
			this.buttonSkipNext.Click += this.ButtonSkipNext_Click;
			this.buttonSkipPrevious.Click += this.ButtonSkipPrevious_Click;

			this.buttonCollapseExpand.Click += (s, e) => {
				if (bottomSheetBehavior.State == BottomSheetBehavior.StateCollapsed)
					bottomSheetBehavior.State = BottomSheetBehavior.StateExpanded;
				else if (bottomSheetBehavior.State == BottomSheetBehavior.StateExpanded)
					bottomSheetBehavior.State = BottomSheetBehavior.StateCollapsed;
			};

			ConnectionManager.OnScreensReceived += this.ConnectionManager_OnScreensReceived;
			ConnectionManager.SendRequest(new Request() {
				Method = Request.Type.GetScreens,
				Body = new AuthenticatedMessageBody() {
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
				}
			});

			ConnectionManager.OnAudioDevicesReceived += this.ConnectionManager_OnAudioDevicesReceived;
			ConnectionManager.SendRequest(new Request() {
				Method = Request.Type.GetAudioEndpoints,
				Body = new AuthenticatedMessageBody() {
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
				}
			});

			ConnectionManager.OnThumbnailReceived += this.ConnectionManager_OnThumbnailReceived;
			ConnectionManager.SendRequest(new Request() {
				Method = Request.Type.GetThumbnail,
				Body = new MediaSessionMessageBody() {
					MediaSession = this.MediaSession,
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
				}
			});

			ConnectionManager.OnMediaSessionsReceived += this.ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged += this.ConnectionManager_OnMediaSessionChanged;
			ConnectionManager.OnConnectionClosed += this.ConnectionManager_OnConnectionClosed;

			this.UpdateUI();
			MediaSessionNotificationManager.UpdateDisplayedSession(this.MediaSession);

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

		public override bool OnBackButton() {
			ConnectionManager.OnMediaSessionsReceived -= this.ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged -= this.ConnectionManager_OnMediaSessionChanged;
			ConnectionManager.OnScreensReceived -= this.ConnectionManager_OnScreensReceived;
			ConnectionManager.OnAudioDevicesReceived -= this.ConnectionManager_OnAudioDevicesReceived;
			ConnectionManager.OnVolumeReceived -= this.ConnectionManager_OnVolumeReceived;
			ConnectionManager.OnThumbnailReceived -= this.ConnectionManager_OnThumbnailReceived;
			ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;
			return false;
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

		private void ConnectionManager_OnScreensReceived(object sender, EventArgs<List<string>> e) {
			this.Activity.RunOnUiThread(() => {
				Spinner spinner = this.View.FindViewById<Spinner>(Resource.Id.spinner_screen_selection);

				ArrayAdapter<string> adapter = new ArrayAdapter<string>(this.Context, Resource.Layout.spinner_item, e.Data);
				spinner.Adapter = adapter;

				ConnectionManager.OnScreensReceived -= this.ConnectionManager_OnScreensReceived;
				spinner.SetSelection(0, false);
				spinner.ItemSelected += this.SpinnerScreens_ItemSelected;
			});
		}

		private void SpinnerScreens_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e) {
			string screen = (sender as Spinner).SelectedItem.ToString();

			ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
				Method = WRMC.Core.Networking.Message.Type.SetScreen,
				Body = new SetScreenMessageBody() {
					MediaSession = this.MediaSession,
					Screen = screen,
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
				}
			});
		}

		private void ConnectionManager_OnAudioDevicesReceived(object sender, EventArgs<List<AudioEndpoint>> e) {
			this.Activity.RunOnUiThread(() => {
				Spinner spinner = this.View.FindViewById<Spinner>(Resource.Id.spinner_audio_device_selection);
				this.seekBarVolume = this.View.FindViewById<SeekBar>(Resource.Id.seek_bar_volume);
				this.textViewVolume = this.View.FindViewById<TextView>(Resource.Id.text_view_volume);

				ArrayAdapter<AudioEndpoint> adapter = new ArrayAdapter<AudioEndpoint>(this.Context, Resource.Layout.spinner_item, e.Data);
				spinner.Adapter = adapter;

				ConnectionManager.OnAudioDevicesReceived -= this.ConnectionManager_OnAudioDevicesReceived;
				spinner.SetSelection(0, false);

				ConnectionManager.OnVolumeReceived += this.ConnectionManager_OnVolumeReceived;

				ConnectionManager.SendRequest(new Request() {
					Method = Request.Type.GetVolume,
					Body = new AuthenticatedMessageBody() {
						ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
					}
				});

				spinner.ItemSelected += delegate (object sender, AdapterView.ItemSelectedEventArgs args) {
					if (args.Position < 0 || args.Position >= e.Data.Count)
						return;

					ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
						Method = WRMC.Core.Networking.Message.Type.SetAudioEndpoint,
						Body = new SetAudioEndpointMessageBody() {
							MediaSession = this.MediaSession,
							AudioEndpoint = e.Data[args.Position],
							ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
						}
					});

					ConnectionManager.OnVolumeReceived += this.ConnectionManager_OnVolumeReceived;

					ConnectionManager.SendRequest(new Request() {
						Method = Request.Type.GetVolume,
						Body = new AuthenticatedMessageBody() {
							ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
						}
					});
				};

				if (this.seekBarVolume != null) {
					this.seekBarVolume.ProgressChanged += (s, e) => {
						this.textViewVolume.Text = "Volume: " + e.Progress;

						ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
							Method = WRMC.Core.Networking.Message.Type.SetVolume,
							Body = new SetVolumeMessageBody() {
								Volume = e.Progress,
								ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
							}
						});
					};
				}
			});
		}

		private void ConnectionManager_OnVolumeReceived(object sender, EventArgs<int> e) {
			this.Activity.RunOnUiThread(() => {
				this.textViewVolume.Text = "Volume: " + e.Data;
				this.seekBarVolume.Progress = e.Data;
			});

			ConnectionManager.OnVolumeReceived -= this.ConnectionManager_OnVolumeReceived;
		}

		private void ConnectionManager_OnThumbnailReceived(object sender, EventArgs<byte[]> e) {
			this.Activity.RunOnUiThread(() => {
				if (e.Data.Length == 0) {
					this.imageViewMediaType.SetImageResource(
						this.MediaSession.Type == MediaSession.MediaType.Video ? Resource.Drawable.video : Resource.Drawable.sound_waves
					);
					int dp = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 25, this.Resources.DisplayMetrics);
					this.imageViewMediaType.SetPadding(dp, 0, dp, 0);

					return;
				}

				Bitmap bitmap = BitmapFactory.DecodeByteArray(e.Data, 0, e.Data.Length);
				this.imageViewMediaType.SetImageBitmap(bitmap);
				this.imageViewMediaType.SetPadding(0, 0, 0, 0);
			});

			ConnectionManager.OnThumbnailReceived -= this.ConnectionManager_OnThumbnailReceived;
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

				this.textViewTitle.Text = this.MediaSession.Title;
				this.textViewArtist.Text = this.MediaSession.Artist;
				this.buttonPlayPause.SetImageResource(this.MediaSession.State == MediaSession.PlaybackState.Playing ? Resource.Drawable.pause : Resource.Drawable.play);

				ConnectionManager.OnThumbnailReceived += this.ConnectionManager_OnThumbnailReceived;
				ConnectionManager.SendRequest(new Request() {
					Method = Request.Type.GetThumbnail,
					Body = new MediaSessionMessageBody() {
						MediaSession = this.MediaSession,
						ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
					}
				});
			});
		}
	}

	public class MultiBottomSheetCallback : BottomSheetBehavior.BottomSheetCallback {
		public event EventHandler<EventArgs<int>> OnStateEvent = null;
		public event EventHandler<EventArgs<float>> OnSlideEvent = null;

		public override void OnSlide(View bottomSheet, float slideOffset) {
			this.OnSlideEvent?.Invoke(null, slideOffset);
		}

		public override void OnStateChanged(View bottomSheet, int newState) {
			this.OnStateEvent?.Invoke(null, newState);
		}
	}
}