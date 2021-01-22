using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;

namespace WRMC.Android.Views.Adapters {
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

	public class ServerActionViewHolder : RecyclerView.ViewHolder {
		public View View { get; set; }
		public Button Action { get; set; }

		public ServerActionViewHolder(View view) : base(view) {
			this.View = view;
		}
	}
}