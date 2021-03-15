using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.IO;

using WRMC.Core;
using WRMC.Core.Models;

namespace WRMC.Android.Views.Adapters {
	public class DirectoryRecyclerAdapter : RecyclerView.Adapter {
		public List<DirectoryItem> Items { get; set; }
		private EventHandler<EventArgs<DirectoryItem>> itemClickedCallback = null;
		private Dictionary<View, int> itemPositions = new Dictionary<View, int>();

		public override int ItemCount => this.Items.Count;

		public DirectoryRecyclerAdapter(List<DirectoryItem> items, EventHandler<EventArgs<DirectoryItem>> itemClickedCallback) {
			this.Items = items;
			this.itemClickedCallback = itemClickedCallback;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.directory_recycler_view_item, parent, false);
			Button item = view.FindViewById<Button>(Resource.Id.directory_item_button);

			DirectoryItemViewHolder viewHolder = new DirectoryItemViewHolder(view) {
				Item = item
			};

			return viewHolder;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			DirectoryItemViewHolder viewHolder = holder as DirectoryItemViewHolder;

			this.itemPositions[viewHolder.Item] = position;
			viewHolder.Item.Text = this.Items[position].Name;

			Bitmap bitmap = BitmapFactory.DecodeByteArray(this.Items[position].Icon, 0, this.Items[position].Icon.Length);

			viewHolder.Item.SetCompoundDrawablesWithIntrinsicBounds(
				/*ContextCompat.GetDrawable(viewHolder.Item.Context, this.Items[position].IsFolder ? Resource.Drawable.folder : Resource.Drawable.file)*/
				new BitmapDrawable(viewHolder.Item.Context.Resources, bitmap == null ? bitmap : Bitmap.CreateScaledBitmap(bitmap, 64, 64, false)),
				null,
				ContextCompat.GetDrawable(viewHolder.Item.Context, Resource.Drawable.chevron_right),
				null
			);

			viewHolder.Item.Click -= this.Item_Click;
			viewHolder.Item.Click += this.Item_Click;
		}

		private void Item_Click(object sender, EventArgs e) {
			this.itemClickedCallback?.Invoke(null, this.Items[this.itemPositions[sender as View]]);
		}
	}

	public class DirectoryItemViewHolder : RecyclerView.ViewHolder {
		public View View { get; set; }
		public Button Item { get; set; }

		public DirectoryItemViewHolder(View view) : base(view) {
			this.View = view;
		}
	}
}