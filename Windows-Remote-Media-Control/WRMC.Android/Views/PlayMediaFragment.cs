using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WRMC.Android.Views.Adapters;
using WRMC.Android.Networking;
using WRMC.Core;
using WRMC.Core.Models;

namespace WRMC.Android.Views {
	/// <summary>
	/// Fragment which contains the view and the functionality to get the content of the servers directories and open files.
	/// </summary>
	public class PlayMediaFragment : BackButtonNotifiableFragment {
		private DirectoryRecyclerAdapter directoryAdapter;

		private Stack<string> directoryStack = new Stack<string>();

		private bool closeFragment;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.play_media, container, false);

			global::Android.Support.V7.Widget.Toolbar toolbar = view.FindViewById<global::Android.Support.V7.Widget.Toolbar>(Resource.Id.play_media_toolbar);

			if (toolbar != null) {
				(this.Activity as MainActivity).SetSupportActionBar(toolbar);
				(this.Activity as MainActivity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				(this.Activity as MainActivity).SupportActionBar.SetDisplayShowHomeEnabled(true);

				toolbar.SetNavigationIcon(Resource.Drawable.chevron_left);
				toolbar.NavigationClick += (s, e) => {
					this.closeFragment = true;
					this.Activity.OnBackPressed();
				};
			}

			RecyclerView directoryRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.play_media_directory_recycler_view);

			if (directoryRecyclerView != null) {
				directoryRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				this.directoryAdapter = new DirectoryRecyclerAdapter(new List<DirectoryItem>(), this.Item_OnClick);
				directoryRecyclerView.SetAdapter(this.directoryAdapter);
				directoryRecyclerView.AddItemDecoration(new LineDividerItemDecoration(directoryRecyclerView.Context));
			}

			ConnectionManager.OnConnectionClosed += this.ConnectionManager_OnConnectionClosed;
			ConnectionManager.OnDirectoryContentReceived += this.ConnectionManager_OnDirectoryContentReceived;

			ConnectionManager.SendRequest(new WRMC.Core.Networking.Request() {
				Method = WRMC.Core.Networking.Request.Type.GetDirectoryContent,
				Body = new WRMC.Core.Networking.DirectoryContentRequestBody() {
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext),
					Directory = ""
				}
			});

			return view;
		}

		public override bool OnBackButton() {
			if (this.directoryStack.Count == 0 || this.closeFragment) {
				ConnectionManager.OnDirectoryContentReceived -= this.ConnectionManager_OnDirectoryContentReceived;
				ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;
				return false;
			}

			this.GotoParentFolder();
			return true;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
			inflater.Inflate(Resource.Menu.play_media_toolbar_menu, menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			switch (item.ItemId) {
				case Resource.Id.refresh_directory:
					this.SendDirRequest(this.directoryStack.Peek());
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		public void ConnectionManager_OnDirectoryContentReceived(object sender, EventArgs<List<DirectoryItem>> e) {
			this.Activity.RunOnUiThread(() => {
				this.directoryAdapter.Items = e.Data;
				this.directoryAdapter.NotifyDataSetChanged();
			});
		}

		private void Item_OnClick(object sender, EventArgs<DirectoryItem> e) {
			if (e.Data.IsFolder) {
				this.GotoSubfolder(e.Data);
				return;
			}

			ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
				Method = WRMC.Core.Networking.Message.Type.PlayFile,
				Body = new WRMC.Core.Networking.PlayFileMessageBody() {
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext),
					FilePath = e.Data.Path
				}
			});
		}

		private void GotoSubfolder(DirectoryItem folder) {
			this.directoryStack.Push(folder.Path);
			this.SendDirRequest(folder.Path);
		}

		private void GotoParentFolder() {
			if (this.directoryStack.Count >= 1)
				this.directoryStack.Pop();

			this.SendDirRequest(this.directoryStack.Count == 0 ? "" : this.directoryStack.Peek());
		}

		private void SendDirRequest(string dir) {
			ConnectionManager.SendRequest(new WRMC.Core.Networking.Request() {
				Method = WRMC.Core.Networking.Request.Type.GetDirectoryContent,
				Body = new WRMC.Core.Networking.DirectoryContentRequestBody() {
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext),
					Directory = dir
				}
			});
		}

		public void ConnectionManager_OnConnectionClosed(object sender, EventArgs e) {
			this.Activity.RunOnUiThread(() => {
				this.closeFragment = true;
				this.Activity.OnBackPressed();
			});
		}
	}
}