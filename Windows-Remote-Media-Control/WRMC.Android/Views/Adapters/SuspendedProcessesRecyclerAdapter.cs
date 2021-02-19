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

using WRMC.Core;
using WRMC.Core.Models;

namespace WRMC.Android.Views.Adapters {
	public class SuspendedProcessesRecyclerAdapter : RecyclerView.Adapter {
		private EventHandler<EventArgs<SuspendedProcess>> onSuspendedProcessSelected = null;
		private Dictionary<View, int> viewPositions = new Dictionary<View, int>();

		public List<SuspendedProcess> SuspendedProcesses { get; set; }
		public override int ItemCount => this.SuspendedProcesses.Count;

		public SuspendedProcessesRecyclerAdapter(List<SuspendedProcess> processes, EventHandler<EventArgs<SuspendedProcess>> onSuspendedProcessSelected) {
			this.SuspendedProcesses = processes;
			this.onSuspendedProcessSelected = onSuspendedProcessSelected;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.suspended_process_recycler_item, parent, false);
			TextView textView = view.FindViewById<TextView>(Resource.Id.process_name);

			SuspendedProcessViewHolder viewHolder = new SuspendedProcessViewHolder(view) {
				TextViewName = textView
			};

			return viewHolder;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			SuspendedProcessViewHolder viewHolder = holder as SuspendedProcessViewHolder;

			viewHolder.View.Click -= this.OnClick;
			viewHolder.View.Click += this.OnClick;

			this.viewPositions[viewHolder.View] = position;
			viewHolder.TextViewName.Text = this.SuspendedProcesses[position].Name;
		}

		public void OnClick(object sender, EventArgs e) {
			if (this.viewPositions.ContainsKey(sender as View))
				this.onSuspendedProcessSelected?.Invoke(this, new EventArgs<SuspendedProcess>(this.SuspendedProcesses[this.viewPositions[sender as View]]));
		}
	}

	public class SuspendedProcessViewHolder : RecyclerView.ViewHolder {
		public View View { get; set; }
		public TextView TextViewName { get; set; }

		public SuspendedProcessViewHolder(View view) : base(view) {
			this.View = view;
		}
	}
}