using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WRMC.Core;

namespace WRMC.Windows.Controls {
	public partial class CustomScrollBarV : UserControl {
		private float _scrollValue;
		private float _visiblePercent;
		private IScrollable _target;

		private int yOffset;
		private bool hover;
		private bool mouseDown;
		private bool firing;

		/// <summary>
		/// The default color of the handle.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(0)]
		[Category("Appearance")]
		[Description("The default color of the handle.")]
		public Color HandleColor { get; set; }

		/// <summary>
		/// The color of the handle when the mouse hovers it.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(0)]
		[Category("Appearance")]
		[Description("The color of the handle when the mouse hovers it.")]
		public Color HandleHoverColor { get; set; }

		/// <summary>
		/// The color of the handle when the mouse clicks it.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(0)]
		[Category("Appearance")]
		[Description("The color of the handle when the mouse clicks it.")]
		public Color HandleClickColor { get; set; }

		/// <summary>
		/// The width of the channel when the mouse is inside of the bounds of the control.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(12)]
		[Category("Appearance")]
		[Description("The width of the channel when the mouse is inside of the bounds of the control.")]
		public int ActiveWidth { get; set; } = 12;

		/// <summary>
		/// The width of the channel when the mouse is outside of the bounds of the control.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(6)]
		[Category("Appearance")]
		[Description("The width of the channel when the mouse is outside of the bounds of the control.")]
		public int InactiveWidth { get; set; } = 6;

		/// <summary>
		/// The visible percent of the content of the scrollable control the handle size will reflect.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public float VisiblePercent {
			get => this._visiblePercent;
			set {
				this._visiblePercent = Math.Max(Math.Min(100.0f, value), 0.0f);
				this.SetHandleByVisiblePercent();
			}
		}

		/// <summary>
		/// The the percentage scroll value.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public float ScrollValue {
			get => this._scrollValue;
			set {
				this.firing = true;
				this._scrollValue = Math.Max(Math.Min(100.0f, value), 0.0f);
				this.SetHandleByValue();
				this.OnScrollValueChanged?.Invoke(this, EventArgs.Empty);
				this.firing = false;
			}
		}

		/// <summary>
		/// The target of the scroll bar.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("The target of the scroll bar.")]
		public IScrollable Target {
			get => this._target;
			set {
				if (this._target != null) {
					this._target.OnVisiblePercentVChanged -= this.target_OnVisiblePercentChanged;
					this._target.OnScrollValueVChanged -= this.target_OnScrollValueChanged;
				}

				this._target = value;

				if (this._target != null) {
					this._target.OnVisiblePercentVChanged += this.target_OnVisiblePercentChanged;
					this._target.OnScrollValueVChanged += this.target_OnScrollValueChanged;
				}
			}
		}

		/// <summary>
		/// Evewnt Handler which is called when the scroll value was changed.
		/// </summary>
		public event TypedEventHandler<CustomScrollBarV, EventArgs> OnScrollValueChanged = null;

		/// <summary>
		/// Creates a new instance of the class <see cref="CustomScrollBarV"/>
		/// </summary>
		public CustomScrollBarV() {
			this.InitializeComponent();
			this.Width = this.InactiveWidth;
		}

		private void CalculateScrollValue() {
			float maxMovementArea = this.Height - this.panelHandle.Height;
			float moved = this.panelHandle.Location.Y;

			this._scrollValue = maxMovementArea == 0 ? 0.0f : (moved / maxMovementArea) * 100.0f;
			this.OnScrollValueChanged?.Invoke(this, EventArgs.Empty);
		}

		private void SetHandleByValue() {
			int maxMovementArea = this.Height - this.panelHandle.Height;
			this.panelHandle.Location = new Point(0, maxMovementArea == 0 ? 0 : (int)(maxMovementArea * (this.ScrollValue / 100.0f)));
		}

		private void SetHandleByVisiblePercent() {
			this.panelHandle.Height = (int)(this.Height * (this.VisiblePercent / 100.0f));
			this.SetHandleByValue();
		}

		protected override void OnPaint(PaintEventArgs e) {
			this.panelHandle.BackColor = this.mouseDown ? this.HandleClickColor : (this.hover ? this.HandleHoverColor : this.HandleColor);
			base.OnPaint(e);
		}

		private void CustomScrollBarV_Resize(object sender, EventArgs e) {
			this.panelHandle.Width = this.Width;
		}

		private void CustomScrollBarV_MouseEnter(object sender, EventArgs e) {
			this.Width = this.ActiveWidth;
		}

		private void CustomScrollBarV_MouseLeave(object sender, EventArgs e) {
			this.Width = this.InactiveWidth;
		}

		private void panelHandle_MouseDown(object sender, MouseEventArgs e) {
			this.mouseDown = true;
			this.yOffset = e.Location.Y;
		}

		private void panelHandle_MouseEnter(object sender, EventArgs e) {
			this.Width = this.ActiveWidth;
			this.hover = true;
		}

		private void panelHandle_MouseLeave(object sender, EventArgs e) {
			this.Width = this.InactiveWidth;
			this.hover = false;
		}

		private void panelHandle_MouseMove(object sender, MouseEventArgs e) {
			if (!this.mouseDown)
				return;

			int newY = this.panelHandle.Location.Y + e.Location.Y - this.yOffset;

			if (newY < 0)
				this.panelHandle.Location = new Point(0, 0);
			else if (newY > this.Height - this.panelHandle.Height)
				this.panelHandle.Location = new Point(0, this.Height - this.panelHandle.Height);
			else
				this.panelHandle.Location = new Point(0, newY);

			this.CalculateScrollValue();
		}

		private void panelHandle_MouseUp(object sender, MouseEventArgs e) {
			this.mouseDown = false;
		}

		private void target_OnScrollValueChanged(IScrollable sender, EventArgs e) {
			if (!this.firing)
				this.ScrollValue = sender.ScrollValueV;
		}

		private void target_OnVisiblePercentChanged(IScrollable sender, EventArgs e) {
			this.VisiblePercent = sender.VisiblePercentV;
		}
	}
}