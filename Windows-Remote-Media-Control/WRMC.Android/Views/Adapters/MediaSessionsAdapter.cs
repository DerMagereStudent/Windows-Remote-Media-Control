using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;

using WRMC.Core;
using WRMC.Core.Models;

namespace WRMC.Android.Views.Adapters {
	public class MediaSessionsAdapter : RecyclerView.Adapter {
		private EventHandler<EventArgs<MediaSession>> onMediaSessionSelected;
		private Dictionary<View, int> viewPositions = new Dictionary<View, int>();

		public List<MediaSession> MediaSessions { get; set; }

		public override int ItemCount => this.MediaSessions.Count;

		public MediaSessionsAdapter(List<MediaSession> mediaSessions, EventHandler<EventArgs<MediaSession>> onMediaSessionSelected) {
			this.MediaSessions = mediaSessions;
			this.onMediaSessionSelected = onMediaSessionSelected;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.media_session_recycler_view_item, parent, false);
			Button titleButton = view.FindViewById<Button>(Resource.Id.media_session_item_name);

			MediaSessionViewHolder viewHolder = new MediaSessionViewHolder(view) {
				TitleButton = titleButton
			};

			return viewHolder;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			MediaSessionViewHolder viewHolder = holder as MediaSessionViewHolder;
			viewHolder.View.Click -= this.OnClick;
			viewHolder.View.Click += this.OnClick;
			this.viewPositions[viewHolder.View] = position;
			viewHolder.TitleButton.Text = this.MediaSessions[position].Title;
		}

		public void OnClick(object sender, EventArgs e) {
			if (this.viewPositions.ContainsKey(sender as View))
				this.onMediaSessionSelected?.Invoke(this, new EventArgs<MediaSession>(this.MediaSessions[this.viewPositions[sender as View]]));
		}
	}

	public class MediaSessionViewHolder : RecyclerView.ViewHolder {
		public View View { get; set; }
		public Button TitleButton { get; set; }

		public MediaSessionViewHolder(View view) : base(view) {
			this.View = view;
		}
	}
}