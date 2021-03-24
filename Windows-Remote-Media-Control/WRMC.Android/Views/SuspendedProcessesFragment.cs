using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WRMC.Android.Networking;
using WRMC.Android.Views.Adapters;
using WRMC.Core;
using WRMC.Core.Models;
using WRMC.Core.Networking;

namespace WRMC.Android.Views {
	/// <summary>
	/// Fragment which contains the view and the functionality to access and resume suspended processes.
	/// </summary>
	public class SuspendedProcessesFragment : BackButtonNotifiableFragment {
		private SuspendedProcessesRecyclerAdapter suspendedProcessesAdapter;
		private SuspendedProcess processToResume;
		private AlertDialog resumeDialog;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.suspended_processes, container, false);

			Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.suspended_processes_toolbar);

			if (toolbar != null) {
				(this.Activity as MainActivity).SetSupportActionBar(toolbar);
				(this.Activity as MainActivity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				(this.Activity as MainActivity).SupportActionBar.SetDisplayShowHomeEnabled(true);

				toolbar.SetNavigationIcon(Resource.Drawable.chevron_left);
				toolbar.NavigationClick += (s, e) => this.Activity.OnBackPressed();
			}

			RecyclerView recyclerView = view.FindViewById<RecyclerView>(Resource.Id.suspended_processes_recycler_view);

			if (recyclerView != null) {
				recyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				this.suspendedProcessesAdapter = new SuspendedProcessesRecyclerAdapter(new List<SuspendedProcess>(), this.SuspendedProcessesRecyclerAdapter_OnSuspendedProcessSelected);
				recyclerView.SetAdapter(this.suspendedProcessesAdapter);
				recyclerView.AddItemDecoration(new LineDividerItemDecoration(recyclerView.Context));
			}

			AlertDialog.Builder builder = new AlertDialog.Builder(this.Context, Resource.Style.CustomDialog)
				.SetTitle("Do you want to resume this process?")
				.SetCancelable(false).SetPositiveButton("Yes", (s, e) => {
					ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
						Method = WRMC.Core.Networking.Message.Type.ResumeSuspendedProcess,
						Body = new ResumeSuspendedProcessMessageBody() {
							Process = this.processToResume,
							ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
						}
					});

					this.resumeDialog.Cancel();
				})
				.SetNegativeButton("No", (s, e) => this.resumeDialog.Cancel());

			this.resumeDialog = builder.Create();

			ConnectionManager.OnSuspendedProcessesReceived += this.ConnectionManager_OnSuspendedProcessesReceived;
			ConnectionManager.OnConnectionClosed += this.ConnectionManager_OnConnectionClosed;

			ConnectionManager.SendRequest(new Request() {
				Method = Request.Type.GetSuspendedMediaProcesses,
				Body = new AuthenticatedMessageBody() {
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
				}
			});

			return view;
		}

		public override bool OnBackButton() {
			ConnectionManager.OnSuspendedProcessesReceived -= this.ConnectionManager_OnSuspendedProcessesReceived;
			ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;
			return false;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
			inflater.Inflate(Resource.Menu.suspended_processes_toolbar_menu, menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			switch (item.ItemId) {
				case Resource.Id.refresh_suspended_processes:
					ConnectionManager.SendRequest(new Request() {
						Method = Request.Type.GetSuspendedMediaProcesses,
						Body = new AuthenticatedMessageBody() {
							ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext)
						}
					});

					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		public void SuspendedProcessesRecyclerAdapter_OnSuspendedProcessSelected(object sender, EventArgs<SuspendedProcess> e) {
			this.processToResume = e.Data;
			this.resumeDialog.Show();
		}

		public void ConnectionManager_OnSuspendedProcessesReceived(object sender, EventArgs<List<SuspendedProcess>> e) {
			this.Activity.RunOnUiThread(() => {
				this.suspendedProcessesAdapter.SuspendedProcesses = e.Data;
				this.suspendedProcessesAdapter.NotifyDataSetChanged();
			});
		}

		public void ConnectionManager_OnConnectionClosed(object sender, EventArgs e) {
			this.Activity.RunOnUiThread(() => {
				this.resumeDialog?.Cancel();
				this.Activity.OnBackPressed();
			});
		}
	}
}