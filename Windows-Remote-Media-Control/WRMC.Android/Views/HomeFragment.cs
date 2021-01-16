using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

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

				recentServerRecyclerView.SetAdapter(new RecentServersRecyclerAdapter(new List<ServerDevice>() {
					new ServerDevice() { Name = "Samsung S9" },
					new ServerDevice() { Name = "Desktop-MXTF6Z" },
					new ServerDevice() { Name = "PC-MaPa" }
				}));

				recentServerRecyclerView.AddItemDecoration(new LineDividerItemDecoration(recentServerRecyclerView.Context));
			}

			RecyclerView currentServerRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.home_current_server_recycler_view);

			if (currentServerRecyclerView != null) {
				currentServerRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));

				currentServerRecyclerView.SetAdapter(new CurrentServerActionsRecyclerAdapter(new List<Tuple<string, Action>>() {
					new Tuple<string, Action>("Sessions", () => { }),
					new Tuple<string, Action>("Processes", () => { }),
					new Tuple<string, Action>("Play Media", () => { })
				}));

				currentServerRecyclerView.AddItemDecoration(new LineDividerItemDecoration(currentServerRecyclerView.Context));
			}

			return view;
		}

		public class RecentServersRecyclerAdapter : RecyclerView.Adapter {
			private List<ServerDevice> serverDevices;

			public override int ItemCount => this.serverDevices.Count;

			public RecentServersRecyclerAdapter(List<ServerDevice> serverDevices) {
				this.serverDevices = serverDevices.TakeLast(3).Reverse().ToList();
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
}