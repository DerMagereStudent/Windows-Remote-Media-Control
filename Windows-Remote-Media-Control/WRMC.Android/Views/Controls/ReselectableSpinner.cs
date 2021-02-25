using Android.Content;
using Android.Util;
using Android.Widget;
using System;

namespace WRMC.Android.Views.Controls {
	public class ReselectableSpinner : Spinner {
		public event EventHandler<ItemSelectedEventArgs> SameItemSelected = null;

		public ReselectableSpinner(Context context) : base(context) { }
        public ReselectableSpinner(Context context, IAttributeSet attrs) : base(context, attrs) { }
        public ReselectableSpinner(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { }

		public override void SetSelection(int position) {
			bool samePosition = position == this.SelectedItemPosition;
			base.SetSelection(position);

			if (samePosition)
				this.SameItemSelected?.Invoke(this, new ItemSelectedEventArgs(this, this.SelectedView, position, this.SelectedItemId));
		}

		public override void SetSelection(int position, bool animate) {
			bool samePosition = position == this.SelectedItemPosition;
			base.SetSelection(position, animate);

			if (samePosition)
				this.SameItemSelected?.Invoke(this, new ItemSelectedEventArgs(this, this.SelectedView, position, this.SelectedItemId));
		}
	}
}