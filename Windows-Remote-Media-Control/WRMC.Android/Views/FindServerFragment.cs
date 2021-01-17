using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WRMC.Android.Networking;
using WRMC.Core.Networking;

namespace WRMC.Android.Views {
	public class FindServerFragment : Fragment {
		private List<ServerDevice> knownServers = new List<ServerDevice>();
		private List<ServerDevice> availableServers = new List<ServerDevice>();

		private IMenu menu;

		private object serverListsLock = new object();

		private LinearLayout knownServersLayout;
		private LinearLayout availableServersLayout;

		private RecyclerView.Adapter knownServersAdapter;
		private RecyclerView.Adapter availableServersAdapter;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.find_server, container, false);

			global::Android.Support.V7.Widget.Toolbar toolbar = view.FindViewById<global::Android.Support.V7.Widget.Toolbar>(Resource.Id.find_server_toolbar);

			if (toolbar != null) {
				(this.Activity as MainActivity).SetSupportActionBar(toolbar);
				(this.Activity as MainActivity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				(this.Activity as MainActivity).SupportActionBar.SetDisplayShowHomeEnabled(true);

				toolbar.SetNavigationIcon(Resource.Drawable.chevron_left);
				toolbar.NavigationClick += (s, e) => {
					ConnectionManager.OnFindServerResponseReceived -= this.ConnectionManager_OnFindServerResponseReceived;
					ConnectionManager.OnFindServerFinished -= this.ConnectionManager_OnFindServerFinished;
					ConnectionManager.StopFindServer();
					(this.Activity as MainActivity).OnBackPressed();
				};
			}

			this.knownServersLayout = view.FindViewById<LinearLayout>(Resource.Id.find_server_known_layout);
			this.availableServersLayout = view.FindViewById<LinearLayout>(Resource.Id.find_server_available_layout);

			RecyclerView knownServersRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.find_server_known_recycler_view);

			if (knownServersRecyclerView != null) {
				knownServersRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				this.knownServersAdapter = new ServersRecyclerAdapter(this.knownServers, -1);
				knownServersRecyclerView.SetAdapter(this.knownServersAdapter);
				knownServersRecyclerView.AddItemDecoration(new LineDividerItemDecoration(knownServersRecyclerView.Context));
			}

			RecyclerView availableServersRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.find_server_available_recycler_view);

			if (availableServersRecyclerView != null) {
				availableServersRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				this.availableServersAdapter = new ServersRecyclerAdapter(this.availableServers, -1);
				availableServersRecyclerView.SetAdapter(this.availableServersAdapter);
				availableServersRecyclerView.AddItemDecoration(new LineDividerItemDecoration(availableServersRecyclerView.Context));
			}

			this.knownServersLayout.Visibility = ViewStates.Gone;
			this.availableServersLayout.Visibility = ViewStates.Gone;

			ConnectionManager.OnFindServerResponseReceived += this.ConnectionManager_OnFindServerResponseReceived;
			ConnectionManager.OnFindServerFinished += this.ConnectionManager_OnFindServerFinished;

			this.HasOptionsMenu = true;

			return view;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
			inflater.Inflate(Resource.Menu.find_server_toolbar_menu, menu);
			this.menu = menu;
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			switch (item.ItemId) {
				case Resource.Id.find_server_toolbar_item_start_stop:
					if (!ConnectionManager.IsSearchingServers) {
						ConnectionManager.StartFindServer();
						item.SetIcon((this.Activity as MainActivity).GetDrawable(Resource.Drawable.close));
					}
					else {
						ConnectionManager.StopFindServer();
						item.SetIcon((this.Activity as MainActivity).GetDrawable(Resource.Drawable.magnify));
					}

					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		private void ConnectionManager_OnFindServerResponseReceived(object sender, MessageBodyEventArgs<ServerResponseBody> e) {
			lock (this.serverListsLock) {
				if (ConnectionManager.KnownServers.Contains(e.DataBody.Server)) {
					if (!this.knownServers.Contains(e.DataBody.Server)) {
						this.knownServers.Add(e.DataBody.Server);
						this.Activity.RunOnUiThread(() => {
							this.knownServersAdapter.NotifyItemInserted(this.knownServers.Count - 1);
							this.knownServersLayout.Visibility = ViewStates.Visible;
						});
					}
				}
				else {
					if (!this.availableServers.Contains(e.DataBody.Server)) {
						this.availableServers.Add(e.DataBody.Server);
						this.Activity.RunOnUiThread(() => {
							this.availableServersAdapter.NotifyItemInserted(this.availableServers.Count - 1);
							this.availableServersLayout.Visibility = ViewStates.Visible;
						});
					}
				}
			}
		}

		private void ConnectionManager_OnFindServerFinished(object sender, EventArgs e) {
			this.Activity.RunOnUiThread(() => {
				this.menu.FindItem(Resource.Id.find_server_toolbar_item_start_stop)?.SetIcon((this.Activity as MainActivity).GetDrawable(Resource.Drawable.magnify));
			});
		}
	}
}