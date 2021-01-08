
namespace WRMC.Windows.Controls {
	partial class ScrollablePanel {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.panelInner = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panelInner
			// 
			this.panelInner.AutoSize = true;
			this.panelInner.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panelInner.BackColor = System.Drawing.Color.Transparent;
			this.panelInner.Location = new System.Drawing.Point(0, 0);
			this.panelInner.Margin = new System.Windows.Forms.Padding(0);
			this.panelInner.Name = "panelInner";
			this.panelInner.Size = new System.Drawing.Size(0, 0);
			this.panelInner.TabIndex = 0;
			this.panelInner.Resize += new System.EventHandler(this.ScrollablePanel_Resize);
			// 
			// ScrollablePanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelInner);
			this.DoubleBuffered = true;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ScrollablePanel";
			this.Resize += new System.EventHandler(this.ScrollablePanel_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panelInner;
	}
}
