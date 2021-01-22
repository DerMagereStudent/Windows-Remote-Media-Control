using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;

using System;
using System.Collections.Generic;
using System.Linq;

using WRMC.Android.Networking;
using WRMC.Android.Views.Adapters;
using WRMC.Core;
using WRMC.Core.Models;
using WRMC.Core.Networking;

namespace WRMC.Android.Views {
	public class MediaSessionsFragment : BackButtonNotifiableFragment {
		private MediaSessionsAdapter mediaSessionsAdapter;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.media_sessions, container, false);

			Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.media_sessions_toolbar);

			if (toolbar != null) {
				(this.Activity as MainActivity).SetSupportActionBar(toolbar);
				(this.Activity as MainActivity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				(this.Activity as MainActivity).SupportActionBar.SetDisplayShowHomeEnabled(true);

				toolbar.SetNavigationIcon(Resource.Drawable.chevron_left);
				toolbar.NavigationClick += (s, e) => this.Activity.OnBackPressed();
			}

			RecyclerView recyclerView = view.FindViewById<RecyclerView>(Resource.Id.media_sessions_recycler_view);

			if (recyclerView != null) {
				recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				this.mediaSessionsAdapter = new MediaSessionsAdapter(new List<MediaSession>(), this.MediaSessionsAdapter_OnMediaSessionSelected);
				recyclerView.SetAdapter(this.mediaSessionsAdapter);
				recyclerView.AddItemDecoration(new LineDividerItemDecoration(recyclerView.Context));
			}

			ConnectionManager.OnMediaSessionsReceived += this.ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged += this.ConnectionManager_OnMediaSessionChanged;
			ConnectionManager.OnConnectionClosed += this.ConnectionManager_OnConnectionClosed;

			ConnectionManager.SendRequest(new Request() {
				Method = Request.Type.GetMediaSessions,
				Body = new AuthenticatedMessageBody() {
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
				}
			});

			return view;
		}

		public override void OnBackButton() {
			ConnectionManager.OnMediaSessionsReceived -= this.ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged -= this.ConnectionManager_OnMediaSessionChanged;
			ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;
		}

		public void MediaSessionsAdapter_OnMediaSessionSelected(object sender, EventArgs<MediaSession> e) {
			ConnectionManager.OnMediaSessionsReceived -= this.ConnectionManager_OnMediaSessionsReceived;
			ConnectionManager.OnMediaSessionChanged -= this.ConnectionManager_OnMediaSessionChanged;
			ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;
			(this.Activity as MainActivity).ChangeFragment(new MediaSessionFragment(e.Data));
		}

		private void ConnectionManager_OnMediaSessionsReceived(object sender, EventArgs<List<MediaSession>> e) {
			this.Activity.RunOnUiThread(() => {
				this.mediaSessionsAdapter.MediaSessions = e.Data;
				this.mediaSessionsAdapter.NotifyDataSetChanged();
			});
		}

		private void ConnectionManager_OnMediaSessionChanged(object sender, EventArgs<MediaSession> e) {
			this.Activity.RunOnUiThread(() => {
				List<MediaSession> sessions = this.mediaSessionsAdapter.MediaSessions.Where(s => s.Equals(e.Data)).ToList();
				foreach (MediaSession session in sessions)
					session.Update(e.Data);

				this.mediaSessionsAdapter.NotifyDataSetChanged();
			});
		}

		private void ConnectionManager_OnConnectionClosed(object sender, EventArgs e) {
			this.Activity.RunOnUiThread(() => {
				this.Activity.OnBackPressed();
			});
		}
	}
}