using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;

using WRMC.Android.Networking;
using WRMC.Android.Views.Adapters;
using WRMC.Core.Networking;

namespace WRMC.Android.Views {
	/// <summary>
	/// Fragment which contains the view and the functionality to search and connect to servers.
	/// </summary>
	public class FindServerFragment : BackButtonNotifiableFragment {
		private List<ServerDevice> knownServers = new List<ServerDevice>();
		private List<ServerDevice> availableServers = new List<ServerDevice>();

		private IMenu menu;
		private AlertDialog.Builder connectDialogBuilder;
		private AlertDialog connectDialog;

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
				toolbar.NavigationClick += (s, e) => this.Activity.OnBackPressed();
			}

			this.knownServersLayout = view.FindViewById<LinearLayout>(Resource.Id.find_server_known_layout);
			this.availableServersLayout = view.FindViewById<LinearLayout>(Resource.Id.find_server_available_layout);

			RecyclerView knownServersRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.find_server_known_recycler_view);

			if (knownServersRecyclerView != null) {
				knownServersRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				this.knownServersAdapter = new ServersRecyclerAdapter(this.knownServers, -1, this.CurrentServerActionsRecyclerAdapter_OnServerDeviceSelected);
				knownServersRecyclerView.SetAdapter(this.knownServersAdapter);
				knownServersRecyclerView.AddItemDecoration(new LineDividerItemDecoration(knownServersRecyclerView.Context));
			}

			RecyclerView availableServersRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.find_server_available_recycler_view);

			if (availableServersRecyclerView != null) {
				availableServersRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				this.availableServersAdapter = new ServersRecyclerAdapter(this.availableServers, -1, this.CurrentServerActionsRecyclerAdapter_OnServerDeviceSelected);
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

		public override bool OnBackButton() {
			ConnectionManager.OnFindServerResponseReceived -= this.ConnectionManager_OnFindServerResponseReceived;
			ConnectionManager.OnFindServerFinished -= this.ConnectionManager_OnFindServerFinished;
			ConnectionManager.StopFindServer();
			this.connectDialog?.Cancel();
			return false;
		}

		private void CurrentServerActionsRecyclerAdapter_OnServerDeviceSelected(object sender, ServerEventArgs e) {
			this.connectDialogBuilder = new AlertDialog.Builder(this.Context, Resource.Style.CustomDialog);
			this.connectDialogBuilder.SetView(this.LayoutInflater.Inflate(Resource.Layout.alert_dialog_connecting, null));
			this.connectDialogBuilder.SetTitle("Connecting...");

			this.connectDialogBuilder.SetCancelable(false).SetNegativeButton("Cancel", (s, e) => {
				ConnectionManager.StopConnect();
				this.connectDialog.Cancel();
			});

			this.connectDialog = this.connectDialogBuilder.Create();
			this.connectDialog.Show();

			ConnectionManager.OnTcpConnectSuccess += this.ConnectionManager_OnTcpConnectSuccess;
			ConnectionManager.OnTcpConnectFailure += this.ConnectionManager_OnTcpConnectFailure;
			ConnectionManager.OnConnectSuccess += this.ConnectionManager_OnConnectSuccess;
			ConnectionManager.OnConnectFailure += this.ConnectionManager_OnConnectFailure;

			if (!ConnectionManager.StartConnect(e.ServerDevice)) {
				ConnectionManager.OnTcpConnectSuccess -= this.ConnectionManager_OnTcpConnectSuccess;
				ConnectionManager.OnTcpConnectFailure -= this.ConnectionManager_OnTcpConnectFailure;
				ConnectionManager.OnConnectSuccess -= this.ConnectionManager_OnConnectSuccess;
				ConnectionManager.OnConnectFailure -= this.ConnectionManager_OnConnectFailure;

				this.connectDialog.Cancel();
			}
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
						item.SetIcon(Resource.Drawable.close);
					} else {
						ConnectionManager.StopFindServer();
						item.SetIcon(Resource.Drawable.magnify);
					}

					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		private void ConnectionManager_OnFindServerResponseReceived(object sender, MessageBodyEventArgs<ServerResponseBody> e) {
			lock (this.serverListsLock) {
				if (ConnectionManager.KnownServers.ContainsKey(e.DataBody.Server)) {
					if (!this.knownServers.Contains(e.DataBody.Server)) {
						this.knownServers.Add(e.DataBody.Server);

						if (this.availableServers.Contains(e.DataBody.Server))
							this.availableServers.Remove(e.DataBody.Server);

						this.Activity.RunOnUiThread(() => {
							this.knownServersAdapter.NotifyDataSetChanged();
							this.knownServersLayout.Visibility = ViewStates.Visible;
							this.availableServersLayout.Visibility = this.availableServers.Count == 0 ? ViewStates.Gone : ViewStates.Visible;
						});
					}
				} else {
					if (!this.availableServers.Contains(e.DataBody.Server)) {
						this.availableServers.Add(e.DataBody.Server);
						this.Activity.RunOnUiThread(() => {
							this.availableServersAdapter.NotifyDataSetChanged();
							this.availableServersLayout.Visibility = ViewStates.Visible;
						});
					}
				}
			}
		}

		private void ConnectionManager_OnFindServerFinished(object sender, EventArgs e) {
			this.Activity.RunOnUiThread(() => {
				this.menu.FindItem(Resource.Id.find_server_toolbar_item_start_stop)?.SetIcon(Resource.Drawable.magnify);
			});
		}

		private void ConnectionManager_OnTcpConnectSuccess(object sender, EventArgs e) {
			ConnectionManager.OnTcpConnectSuccess -= this.ConnectionManager_OnTcpConnectSuccess;
			ConnectionManager.OnTcpConnectFailure -= this.ConnectionManager_OnTcpConnectFailure;

			ClientDevice clientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext);
			ConnectionManager.SendConnectRequest(clientDevice);
		}

		private void ConnectionManager_OnTcpConnectFailure(object sender, EventArgs e) {
			ConnectionManager.OnTcpConnectSuccess -= this.ConnectionManager_OnTcpConnectSuccess;
			ConnectionManager.OnTcpConnectFailure -= this.ConnectionManager_OnTcpConnectFailure;
			ConnectionManager.OnConnectSuccess -= this.ConnectionManager_OnConnectSuccess;
			ConnectionManager.OnConnectFailure -= this.ConnectionManager_OnConnectFailure;

			this.Activity.RunOnUiThread(() => {
				this.connectDialog.Cancel();
				Toast.MakeText(this.Context, "TcpConnect", ToastLength.Long).Show();
			});
		}

		private void ConnectionManager_OnConnectSuccess(object sender, EventArgs e) {
			ConnectionManager.OnConnectSuccess -= this.ConnectionManager_OnConnectSuccess;
			ConnectionManager.OnConnectFailure -= this.ConnectionManager_OnConnectFailure;

			ConnectionManager.StopFindServer();

			this.Activity.RunOnUiThread(() => {
				this.menu.GetItem(0).SetIcon((this.Activity as MainActivity).GetDrawable(Resource.Drawable.magnify));
				this.Activity.OnBackPressed();
			});
		}

		private void ConnectionManager_OnConnectFailure(object sender, EventArgs e) {
			ConnectionManager.OnConnectSuccess -= this.ConnectionManager_OnConnectSuccess;
			ConnectionManager.OnConnectFailure -= this.ConnectionManager_OnConnectFailure;

			this.Activity.RunOnUiThread(() => {
				this.connectDialog.Cancel();
				Toast.MakeText(this.Context, "Connect", ToastLength.Long).Show();
			});
		}
	}
}