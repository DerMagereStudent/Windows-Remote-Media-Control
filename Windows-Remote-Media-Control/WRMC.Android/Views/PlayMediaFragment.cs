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

namespace WRMC.Android.Views {
	public class PlayMediaFragment : BackButtonNotifiableFragment {
		private DirectoryRecyclerAdapter directoryAdapter;

		private IMenu menu;

		private string parentDir;
		private string currentDir;

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

				this.directoryAdapter = new DirectoryRecyclerAdapter(new List<string>(), new List<string>(), this.Item_OnClick);
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
			if ((string.IsNullOrEmpty(this.currentDir) && string.IsNullOrEmpty(this.parentDir)) || this.closeFragment) {
				ConnectionManager.OnDirectoryContentReceived -= this.ConnectionManager_OnDirectoryContentReceived;
				ConnectionManager.OnConnectionClosed -= this.ConnectionManager_OnConnectionClosed;
				return false;
			}

			this.ChangeDir(this.parentDir);
			return true;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
			inflater.Inflate(Resource.Menu.play_media_toolbar_menu, menu);
			this.menu = menu;
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			switch (item.ItemId) {
				case Resource.Id.refresh_directory:
					this.SendDirRequest(this.currentDir);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		public void ConnectionManager_OnDirectoryContentReceived(object sender, EventArgs<Tuple<List<string>, List<string>>> e) {
			this.Activity.RunOnUiThread(() => {
				this.directoryAdapter.Folders = e.Data.Item1;
				this.directoryAdapter.Files = e.Data.Item2;
				this.directoryAdapter.NotifyDataSetChanged();
			});
		}

		private void Item_OnClick(object sender, EventArgs<string> e) {
			if (!System.IO.Path.HasExtension(e.Data)) {
				this.ChangeDir(e.Data);
				return;
			}

			ConnectionManager.SendMessage(new WRMC.Core.Networking.Message() {
				Method = WRMC.Core.Networking.Message.Type.PlayFile,
				Body = new WRMC.Core.Networking.PlayFileMessageBody() {
					ClientDevice = DeviceInformation.GetClientDevice(this.Activity.ApplicationContext),
					FilePath = e.Data
				}
			});
		}

		private void ChangeDir(string newDir) {
			this.UpdateDirs(newDir);
			this.SendDirRequest(newDir);
		}

		private void UpdateDirs(string currentDir) {
			this.currentDir = currentDir;

			int index = this.currentDir.LastIndexOf("\\");

			if (index > 0 && index == this.currentDir.Length - 1) {
				this.currentDir = this.currentDir.Substring(0, index);
				index = this.currentDir.LastIndexOf("\\");
			}

			if (index < 0) {
				this.parentDir = "";
				return;
			}

			this.parentDir = this.currentDir.Substring(0, index);
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