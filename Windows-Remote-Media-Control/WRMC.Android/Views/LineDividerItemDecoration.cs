using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WRMC.Android.Views {
	public class LineDividerItemDecoration : RecyclerView.ItemDecoration {
		private Drawable divider;

		public LineDividerItemDecoration(Context context) {
			this.divider = ContextCompat.GetDrawable(context, Resource.Drawable.line_divider);
		}

        public override void OnDrawOver(Canvas c, RecyclerView parent, RecyclerView.State state) {
            int left = parent.PaddingLeft;
            int right = parent.Width - parent.PaddingRight;

            int childCount = parent.GetAdapter().ItemCount;
            for (int i = 0; i < childCount; i++) {
                View child = parent.GetChildAt(i);

                if (child == null || parent.GetChildAdapterPosition(child) == parent.GetAdapter().ItemCount - 1) {
                    continue;
                }

                RecyclerView.LayoutParams layoutParams = (RecyclerView.LayoutParams)child.LayoutParameters;

                int top = child.Bottom + layoutParams.BottomMargin;
                int bottom = top + this.divider.IntrinsicHeight;

                this.divider.SetBounds(left, top, right, bottom);
                this.divider.Draw(c);
            }
        }
    }
}