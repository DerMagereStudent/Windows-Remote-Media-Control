using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using WRMC.Core;

namespace WRMC.Windows.Controls {
	public partial class ScrollablePanel : UserControl, IScrollable {
		public class ControlCollection : Control.ControlCollection {
			private Panel container;

			//public override int Count => this.container.Controls.Count;

			public ControlCollection(Control owner, Panel container) : base(owner) {
				this.container = container;
			}

			public override void Add(Control value) {
				if (this.container == value)
					base.Add(value);
				else
					this.container.Controls.Add(value);
			}

			public override void Remove(Control value) {
				if (this.container == value)
					base.Remove(value);
				else
					this.container.Controls.Remove(value);
			}

			public override void Clear() {
				this.container?.Invoke(new Action(() => {
					for (int i = this.container.Controls.Count - 1; i >= 0; i--) {
						Control c = this.container.Controls[i];
						this.container.Controls.Remove(c);
						c.Dispose();
					}

					this.container.Controls.Clear();
				}));
			}
		}

		private float _visiblePercentH = 0.0f;
		private float _visiblePercentV = 0.0f;

		private float _scrollValueH = 0.0f;
		private float _scrollValueV = 0.0f;

		private CustomScrollBarH _scrollBarH;
		private CustomScrollBarV _scrollBarV;

		private bool firingScrollBarH;
		private bool firingScrollBarV;

		protected override CreateParams CreateParams {
			get {
				CreateParams param = base.CreateParams;
				param.ExStyle |= 0x02000000;
				return param;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DefaultValue(0.0f)]
		public float VisiblePercentH => this._visiblePercentH;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DefaultValue(0.0f)]
		public float VisiblePercentV => this._visiblePercentV;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DefaultValue(0.0f)]
		public float ScrollValueH {
			get => this._scrollValueH;
			set {
				this.firingScrollBarH = true;
				this._scrollValueH = Math.Max(Math.Min(100.0f, value), 0.0f);
				this.SetLocationByScrollValue();
				this.OnScrollValueHChanged?.Invoke(this, EventArgs.Empty);
				this.firingScrollBarH = false;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DefaultValue(0.0f)]
		public float ScrollValueV {
			get => this._scrollValueV;
			set {
				this.firingScrollBarV = true;
				this._scrollValueV = Math.Max(Math.Min(100.0f, value), 0.0f);
				this.SetLocationByScrollValue();
				this.OnScrollValueVChanged?.Invoke(this, EventArgs.Empty);
				this.firingScrollBarV = false;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(0.5f)]
		[Category("Behaviour")]
		[Description("The value the mouse wheel delta is multiplied with when scrolling.")]
		public float ScrollSpeed { get; set; } = 0.5f;

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("The horizontal scroll bar, whose values ​​should be synchronized with those of the scrollable control.")]
		public CustomScrollBarH ScrollBarH {
			get => this._scrollBarH;
			set {
				if (this._scrollBarH != null)
					this._scrollBarH.OnScrollValueChanged -= this.scrollBarH_OnScrollValueChanged;

				this._scrollBarH = value;

				if (this._scrollBarH != null)
					this._scrollBarH.OnScrollValueChanged += this.scrollBarH_OnScrollValueChanged;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("The vertical scroll bar, whose values ​​should be synchronized with those of the scrollable control.")]
		public CustomScrollBarV ScrollBarV {
			get => this._scrollBarV;
			set {
				if (this._scrollBarV != null)
					this._scrollBarV.OnScrollValueChanged -= this.scrollBarV_OnScrollValueChanged;

				this._scrollBarV = value;

				if (this._scrollBarV != null)
					this._scrollBarV.OnScrollValueChanged += this.scrollBarV_OnScrollValueChanged;
			}
		}

		public event TypedEventHandler<IScrollable, EventArgs> OnVisiblePercentHChanged;
		public event TypedEventHandler<IScrollable, EventArgs> OnVisiblePercentVChanged;
		public event TypedEventHandler<IScrollable, EventArgs> OnScrollValueHChanged;
		public event TypedEventHandler<IScrollable, EventArgs> OnScrollValueVChanged;

		protected override Control.ControlCollection CreateControlsInstance() => new ControlCollection(this, this.panelInner);

		public ScrollablePanel() {
			this.InitializeComponent();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.MouseWheel += this.ScrollablePanel_MouseWheel;
		}

		private void SetScrollValueByLocation() {
			if (this.VisiblePercentV.Equals(100.0f)) {
				this._scrollValueV = 0.0f;
				return;
			}

			float valueV = Math.Abs(this.panelInner.Location.Y) / (float)Math.Abs(this.Height - this.panelInner.Height) * 100.0f;
			this.ScrollValueV = Math.Max(Math.Min(100.0f, valueV), 0.0f);

			float valueH = Math.Abs(this.panelInner.Location.X) / (float)Math.Abs(this.Width - this.panelInner.Width) * 100.0f;
			this.ScrollValueH = Math.Max(Math.Min(100.0f, valueH), 0.0f);

			this.OnScrollValueHChanged?.Invoke(this, EventArgs.Empty);
		}

		private void SetLocationByScrollValue() {
			this.panelInner.Location = new Point(this.VisiblePercentH.Equals(100.0f) ? 0 : (int)((this.Width - this.panelInner.Width) * (this.ScrollValueH / 100.0f)), this.panelInner.Location.Y);
			this.panelInner.Location = new Point(this.panelInner.Location.X, this.VisiblePercentV.Equals(100.0f) ? 0 : (int)((this.Height - this.panelInner.Height) * (this.ScrollValueV / 100.0f)));
			this.Refresh();
		}

		private void scrollBarH_OnScrollValueChanged(CustomScrollBarH sender, EventArgs e) {
			if (this.firingScrollBarH)
				return;

			this.ScrollValueH = sender.ScrollValue;
		}

		private void scrollBarV_OnScrollValueChanged(CustomScrollBarV sender, EventArgs e) {
			if (this.firingScrollBarV)
				return;

			this.ScrollValueV = sender.ScrollValue;
		}

		private void ScrollablePanel_MouseWheel(object sender, MouseEventArgs e) {
			if (this.VisiblePercentV.Equals(100.0f))
				return;

			int totalWheelDelta = (int)(e.Delta * this.ScrollSpeed);
			int newY = this.panelInner.Location.Y + totalWheelDelta;

			if (newY > 0)
				this.panelInner.Location = new Point(this.panelInner.Location.X, 0);
			else if (newY + this.panelInner.Height < this.Height)
				this.panelInner.Location = new Point(this.panelInner.Location.X, this.Height - this.panelInner.Height);
			else
				this.panelInner.Location = new Point(this.panelInner.Location.X, newY);

			this.SetScrollValueByLocation();
		}

		private void ScrollablePanel_Resize(object sender, EventArgs e) {
			this._visiblePercentH = Math.Max(Math.Min(100.0f, this.Width / (float)this.panelInner.Width * 100.0f), 0.0f);
			this.OnVisiblePercentHChanged?.Invoke(this, EventArgs.Empty);

			this._visiblePercentV = Math.Max(Math.Min(100.0f, this.Height / (float)this.panelInner.Height * 100.0f), 0.0f);
			this.OnVisiblePercentVChanged?.Invoke(this, EventArgs.Empty);

			if (this.ScrollValueV.Equals(0.0f) || this.ScrollValueV.Equals(100.0f))
				this.SetLocationByScrollValue();
			else
				this.SetScrollValueByLocation();
		}
	}
}