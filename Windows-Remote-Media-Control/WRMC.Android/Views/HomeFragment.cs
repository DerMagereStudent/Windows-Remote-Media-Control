using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;

using WRMC.Android.Networking;
using WRMC.Android.Views.Adapters;
using WRMC.Core.Networking;

namespace WRMC.Android.Views {
	public class HomeFragment : Fragment {
		private AlertDialog.Builder connectDialogBuilder;
		private AlertDialog connectDialog;

		private ServersRecyclerAdapter recentDevicesAdapter;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.home, container, false);

			global::Android.Support.V7.Widget.Toolbar toolbar = view.FindViewById<global::Android.Support.V7.Widget.Toolbar>(Resource.Id.home_toolbar);

			if (toolbar != null)
				(this.Activity as MainActivity).SetSupportActionBar(toolbar);

			RecyclerView recentServerRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.home_recent_servers_recycler_view);

			if (recentServerRecyclerView != null) {
				recentServerRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				this.recentDevicesAdapter = new ServersRecyclerAdapter(ConnectionManager.KnownServers.Keys.ToList(), 3, this.CurrentServerActionsRecyclerAdapter_OnServerDeviceSelected);
				recentServerRecyclerView.SetAdapter(this.recentDevicesAdapter);
				recentServerRecyclerView.AddItemDecoration(new LineDividerItemDecoration(recentServerRecyclerView.Context));
			}

			RecyclerView currentServerRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.home_current_server_recycler_view);

			if (currentServerRecyclerView != null) {
				currentServerRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				currentServerRecyclerView.SetAdapter(new CurrentServerActionsRecyclerAdapter(new List<Tuple<string, Action>>() {
					new Tuple<string, Action>("Media Sessions", () => {
						this.DetachEventHandlers();
						(this.Activity as MainActivity).ChangeFragment(new MediaSessionsFragment());
					}),

					new Tuple<string, Action>("Suspended Processes", () => {
						this.DetachEventHandlers();
						(this.Activity as MainActivity).ChangeFragment(new SuspendedProcessesFragment());
					}),

					new Tuple<string, Action>("Play Media", () => {
						this.DetachEventHandlers();
						(this.Activity as MainActivity).ChangeFragment(new PlayMediaFragment());
					})
				}));

				currentServerRecyclerView.AddItemDecoration(new LineDividerItemDecoration(currentServerRecyclerView.Context));
			}

			this.HasOptionsMenu = true;

			LinearLayout linearLayout = view.FindViewById<LinearLayout>(Resource.Id.home_current_device_layout);
			
			if (linearLayout != null)
				linearLayout.Visibility = ConnectionManager.CurrentServer != null ? ViewStates.Visible : ViewStates.Gone;

			Button buttonDisconnect = view.FindViewById<Button>(Resource.Id.home_button_disconnect);
			 
			if (buttonDisconnect != null)
				buttonDisconnect.Click += (s, e) => ConnectionManager.CloseConnection(DeviceInformation.GetClientDevice(this.Activity.ApplicationContext));

			this.connectDialogBuilder = new AlertDialog.Builder(this.Context, Resource.Style.CustomDialog);
			this.connectDialogBuilder.SetView(this.LayoutInflater.Inflate(Resource.Layout.alert_dialog_connecting, null));
			this.connectDialogBuilder.SetTitle("Connecting...");

			this.connectDialogBuilder.SetCancelable(false).SetNegativeButton("Cancel", (s, e) => {
				ConnectionManager.StopConnect();
				this.connectDialog.Cancel();
			});

			this.connectDialog = this.connectDialogBuilder.Create();

			if (ConnectionManager.CurrentServer != null)
				ConnectionManager.OnConnectionClosed += this.ConnectionManager_OnConnectionClosed;

			return view;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
			inflater.Inflate(Resource.Menu.home_toolbar_menu, menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			switch (item.ItemId) {
				case Resource.Id.home_toolbar_item_find_server:
					this.DetachEventHandlers();
					(this.Activity as MainActivity).ChangeFragment(new FindServerFragment());
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		public void DetachEventHandlers() {
			ConnectionManager.OnTcpConnectSuccess -= this.ConnectionManager_OnTcpConnectSuccess;
			ConnectionManager.OnTcpConnectFailure -= this.ConnectionManager_OnTcpConnectFailure;
			ConnectionManager.OnConnectSuccess -= this.ConnectionManager_OnConnectSuccess;
			ConnectionManager.OnConnectFailure -= this.ConnectionManager_OnConnectFailure;
			ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;
		}

		private void CurrentServerActionsRecyclerAdapter_OnServerDeviceSelected(object sender, ServerEventArgs e) {
			this.connectDialog.Show();

			ConnectionManager.OnTcpConnectSuccess += this.ConnectionManager_OnTcpConnectSuccess;
			ConnectionManager.OnTcpConnectFailure += this.ConnectionManager_OnTcpConnectFailure;
			ConnectionManager.OnConnectSuccess += this.ConnectionManager_OnConnectSuccess;
			ConnectionManager.OnConnectFailure += this.ConnectionManager_OnConnectFailure;

			if (!ConnectionManager.StartConnect(e.ServerDevice)) {
				this.DetachEventHandlers();
				this.connectDialog.Cancel();
			}
		}

		private void ConnectionManager_OnTcpConnectSuccess(object sender, EventArgs e) {
			ConnectionManager.OnTcpConnectSuccess -= this.ConnectionManager_OnTcpConnectSuccess;
			ConnectionManager.OnTcpConnectFailure -= this.ConnectionManager_OnTcpConnectFailure;

			ClientDevice clientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext);
			ConnectionManager.SendConnectRequest(clientDevice);
		}

		private void ConnectionManager_OnTcpConnectFailure(object sender, EventArgs e) {
			this.DetachEventHandlers();

			this.Activity.RunOnUiThread(() => {
				this.connectDialog.Cancel();
				Toast.MakeText(this.Context, "TcpConnect", ToastLength.Long).Show();
			});
		}

		private void ConnectionManager_OnConnectSuccess(object sender, EventArgs e) {
			ConnectionManager.OnConnectSuccess -= this.ConnectionManager_OnConnectSuccess;
			ConnectionManager.OnConnectFailure -= this.ConnectionManager_OnConnectFailure;

			ConnectionManager.OnConnectionClosed += this.ConnectionManager_OnConnectionClosed;

			this.UpdateRecentDevicesLayout();
			this.UpdateCurrentDeviceLayout();
			this.Activity.RunOnUiThread(() => this.connectDialog.Cancel());
		}

		private void ConnectionManager_OnConnectFailure(object sender, EventArgs e) {
			ConnectionManager.OnConnectSuccess -= this.ConnectionManager_OnConnectSuccess;
			ConnectionManager.OnConnectFailure -= this.ConnectionManager_OnConnectFailure;

			this.Activity.RunOnUiThread(() => {
				this.connectDialog.Cancel();
				Toast.MakeText(this.Context, "Connect", ToastLength.Long).Show();
			});
		}

		private void ConnectionManager_OnConnectionClosed(object sender, EventArgs e) {
			ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;
			this.UpdateCurrentDeviceLayout();
		}

		private void UpdateRecentDevicesLayout() {
			this.Activity.RunOnUiThread(() => {
				this.recentDevicesAdapter.ServerDevices = ConnectionManager.KnownServers.Keys;
				this.recentDevicesAdapter.NotifyDataSetChanged();
			});
		}

		private void UpdateCurrentDeviceLayout() {
			this.Activity.RunOnUiThread(() => {
				this.View.FindViewById<LinearLayout>(Resource.Id.home_current_device_layout).Visibility = ConnectionManager.CurrentServer != null ? ViewStates.Visible : ViewStates.Gone;
			});
		}
	}
}