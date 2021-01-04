using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WRMC.Windows.Controls {
	public class CustomComboBox : ComboBox {
		private Color _highlightColor;

		private Brush backColorBrush;
		private Brush highlightColorBrush;
		private Brush foreColorBrush;

		public override Color BackColor {
			get => base.BackColor;
			set {
				base.BackColor = value;
				this.backColorBrush?.Dispose();
				this.backColorBrush = new SolidBrush(this.BackColor);
			}
		}

		public override Color ForeColor {
			get => base.ForeColor;
			set {
				base.ForeColor = value;
				this.foreColorBrush?.Dispose();
				this.foreColorBrush = new SolidBrush(this.ForeColor);
			}
		}

		/// <summary>
		/// The back color of any hovered item.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(0)]
		[Category("Appearance")]
		[Description("The back color of any hovered item.")]
		public Color ItemHighlightColor {
			get => this._highlightColor;
			set {
				this._highlightColor = value;
				this.highlightColorBrush?.Dispose();
				this.highlightColorBrush = new SolidBrush(this.ItemHighlightColor);
			}
		}

		public CustomComboBox() {
			this.DrawMode = DrawMode.OwnerDrawFixed;
		}

		protected override void OnDrawItem(DrawItemEventArgs e) {
			if (e.Index < 0) {
				base.OnDrawItem(e);
				return;
			}

			if (e.State.HasFlag(DrawItemState.Selected) && !e.State.HasFlag(DrawItemState.ComboBoxEdit)) {
				e.DrawBackground();
				e.Graphics.FillRectangle(this.highlightColorBrush, e.Bounds);
				Font f = this.Font;
				e.Graphics.DrawString(this.Items[e.Index].ToString(), f, this.foreColorBrush, e.Bounds, StringFormat.GenericDefault);
			}
			else {
				e.DrawBackground();
				e.Graphics.FillRectangle(this.backColorBrush, e.Bounds);
				Font f = this.Font;
				e.Graphics.DrawString(this.Items[e.Index].ToString(), f, this.foreColorBrush, e.Bounds, StringFormat.GenericDefault);
			}
		}
	}
}