using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;

using WRMC.Core.Networking;

namespace WRMC.Android.Views.Adapters {
	public class ServersRecyclerAdapter : RecyclerView.Adapter {
		private List<ServerDevice> _serverDevices;
		private EventHandler<ServerEventArgs> onServerDeviceSelected;
		private int limit;

		private Dictionary<View, int> viewPositions = new Dictionary<View, int>();

		public List<ServerDevice> ServerDevices {
			get => this._serverDevices;
			set => this._serverDevices = this.limit <= 0 ? value : value.TakeLast(3).Reverse().ToList();
		}
		public override int ItemCount => this.ServerDevices.Count;

		public ServersRecyclerAdapter(List<ServerDevice> serverDevices, int limit, EventHandler<ServerEventArgs> onServerDeviceSelected) {
			this.limit = limit;
			this.ServerDevices = serverDevices;
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
			viewHolder.View.Click -= this.OnClick;
			viewHolder.View.Click += this.OnClick;
			this.viewPositions[viewHolder.View] = position;
			viewHolder.NameView.Text = this.ServerDevices[position].Name;
		}

		public void OnClick(object sender, EventArgs e) {
			if (this.viewPositions.ContainsKey(sender as View)) {
				int position = this.viewPositions[sender as View];

				if (position >= 0 && position < this.ServerDevices.Count)
					this.onServerDeviceSelected?.Invoke(this, new ServerEventArgs(this.ServerDevices[position]));
			}
		}
	}

	public class ServerViewHolder : RecyclerView.ViewHolder {
		public View View { get; set; }
		public TextView NameView { get; set; }

		public ServerViewHolder(View view) : base(view) {
			this.View = view;
		}
	}
}