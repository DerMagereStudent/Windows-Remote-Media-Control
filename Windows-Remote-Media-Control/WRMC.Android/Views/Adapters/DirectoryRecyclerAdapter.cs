using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.IO;

using WRMC.Core;

namespace WRMC.Android.Views.Adapters {
	public class DirectoryRecyclerAdapter : RecyclerView.Adapter {
		public List<string> Folders { get; set; }
		public List<string> Files { get; set; }
		private EventHandler<EventArgs<string>> itemClickedCallback = null;
		private Dictionary<View, int> itemPositions = new Dictionary<View, int>();

		public override int ItemCount => this.Folders.Count + this.Files.Count;

		public DirectoryRecyclerAdapter(List<string> folders, List<string> files, EventHandler<EventArgs<string>> itemClickedCallback) {
			this.Folders = folders;
			this.Files = files;
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
			viewHolder.Item.Text = this.GetName(this.GetItem(position, out bool isFolder));

			viewHolder.Item.SetCompoundDrawablesWithIntrinsicBounds(
				ContextCompat.GetDrawable(viewHolder.Item.Context, isFolder ? Resource.Drawable.folder : Resource.Drawable.file),
				null,
				ContextCompat.GetDrawable(viewHolder.Item.Context, Resource.Drawable.chevron_right),
				null
			);

			viewHolder.Item.Click -= this.Item_Click;
			viewHolder.Item.Click += this.Item_Click;
		}

		private string GetName(string path) {
			int index = path.LastIndexOf("\\");

			if (index < 0)
				return path;

			if (index == path.Length - 1)
				return path.Substring(0, index);

			return path.Substring(index + 1);
		}

		private void Item_Click(object sender, EventArgs e) {
			this.itemClickedCallback?.Invoke(null, this.GetItem(this.itemPositions[sender as View]));
		}

		private string GetItem(int position) {
			if (position < 0 || position >= this.ItemCount)
				return null;

			if (position < this.Folders.Count)
				return this.Folders[position];
			else
				return this.Files[position - this.Folders.Count];
		}

		private string GetItem(int position, out bool isFolder) {
			if (position < 0 || position >= this.ItemCount) {
				isFolder = false;
				return null;
			}
			
			if (position < this.Folders.Count) {
				isFolder = true;
				return this.Folders[position];
			} else {
				isFolder = false;
				return this.Files[position - this.Folders.Count];
			}
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