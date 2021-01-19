using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using WRMC.Android.Networking;
using WRMC.Core.Networking;

namespace WRMC.Android.Views {
	public class HomeFragment : Fragment {
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate(Resource.Layout.home, container, false);

			global::Android.Support.V7.Widget.Toolbar toolbar = view.FindViewById<global::Android.Support.V7.Widget.Toolbar>(Resource.Id.home_toolbar);

			if (toolbar != null)
				(this.Activity as MainActivity).SetSupportActionBar(toolbar);

			RecyclerView recentServerRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.home_recent_servers_recycler_view);

			if (recentServerRecyclerView != null) {
				recentServerRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));
				recentServerRecyclerView.SetAdapter(new ServersRecyclerAdapter(ConnectionManager.KnownServers.Keys.ToList(), 3, null));
				recentServerRecyclerView.AddItemDecoration(new LineDividerItemDecoration(recentServerRecyclerView.Context));
			}

			RecyclerView currentServerRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.home_current_server_recycler_view);

			if (currentServerRecyclerView != null) {
				currentServerRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				currentServerRecyclerView.SetAdapter(new CurrentServerActionsRecyclerAdapter(new List<Tuple<string, Action>>() {
					new Tuple<string, Action>("Media Sessions", () => { }),
					new Tuple<string, Action>("Suspended Processes", () => { }),
					new Tuple<string, Action>("Play Media", () => { })
				}));

				currentServerRecyclerView.AddItemDecoration(new LineDividerItemDecoration(currentServerRecyclerView.Context));
			}

			this.HasOptionsMenu = true;

			return view;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
			inflater.Inflate(Resource.Menu.home_toolbar_menu, menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			switch (item.ItemId) {
				case Resource.Id.home_toolbar_item_find_server:
					(this.Activity as MainActivity).ChangeFragment(new FindServerFragment());
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
	}

	public class ServersRecyclerAdapter : RecyclerView.Adapter {
		private List<ServerDevice> serverDevices;
		private EventHandler<ServerEventArgs> onServerDeviceSelected;

		public override int ItemCount => this.serverDevices.Count;

		public ServersRecyclerAdapter(List<ServerDevice> serverDevices, int limit, EventHandler<ServerEventArgs> onServerDeviceSelected) {
			this.serverDevices = limit <= 0 ? serverDevices : serverDevices.TakeLast(3).Reverse().ToList();
			this.onServerDeviceSelected = onServerDeviceSelected;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.server_recycler_view_item, parent, false);
			TextView nameView = view.FindViewById<TextView>(Resource.Id.server_item_name);

			ServerViewHolder viewHolder = new ServerViewHolder(view) {
				NameView = nameView
			};

			return viewHolder;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			ServerViewHolder viewHolder = holder as ServerViewHolder;

			viewHolder.View.Click += (s, e) => this.onServerDeviceSelected?.Invoke(this, new ServerEventArgs(this.serverDevices[position]));

			viewHolder.NameView.Text = this.serverDevices[position].Name;
		}
	}

	public class CurrentServerActionsRecyclerAdapter : RecyclerView.Adapter {
		private List<Tuple<string, Action>> actions;

		public override int ItemCount => this.actions.Count;

		public CurrentServerActionsRecyclerAdapter(List<Tuple<string, Action>> actions) {
			this.actions = actions;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.current_server_action_recycler_view_item, parent, false);

			Button action = view.FindViewById<Button>(Resource.Id.server_action_button);

			ServerActionViewHolder viewHolder = new ServerActionViewHolder(view) {
				Action = action
			};

			return viewHolder;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			ServerActionViewHolder viewHolder = holder as ServerActionViewHolder;

			viewHolder.Action.Text = this.actions[position].Item1;
			viewHolder.Action.Click += (s, e) => this.actions[position].Item2.Invoke();
		}
	}

	public class ServerViewHolder : RecyclerView.ViewHolder {
		public View View { get; set; }
		public TextView NameView { get; set; }

		public ServerViewHolder(View view) : base(view) {
			this.View = view;
		}
	}

	public class ServerActionViewHolder : RecyclerView.ViewHolder {
		public View View { get; set; }
		public Button Action { get; set; }

		public ServerActionViewHolder(View view) : base(view) {
			this.View = view;
		}
	}
}