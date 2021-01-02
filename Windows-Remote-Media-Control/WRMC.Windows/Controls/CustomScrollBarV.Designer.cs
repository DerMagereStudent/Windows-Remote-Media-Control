
namespace WRMC.Windows.Controls {
	partial class CustomScrollBarV {
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
			this.panelHandle = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panelHandle
			// 
			this.panelHandle.Location = new System.Drawing.Point(0, 0);
			this.panelHandle.Name = "panelHandle";
			this.panelHandle.Size = new System.Drawing.Size(12, 100);
			this.panelHandle.TabIndex = 0;
			this.panelHandle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelHandle_MouseDown);
			this.panelHandle.MouseEnter += new System.EventHandler(this.panelHandle_MouseEnter);
			this.panelHandle.MouseLeave += new System.EventHandler(this.panelHandle_MouseLeave);
			this.panelHandle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelHandle_MouseMove);
			this.panelHandle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelHandle_MouseUp);
			// 
			// CustomScrollBarV
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelHandle);
			this.Name = "CustomScrollBarV";
			this.Size = new System.Drawing.Size(12, 150);
			this.MouseEnter += new System.EventHandler(this.CustomScrollBarV_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.CustomScrollBarV_MouseLeave);
			this.Resize += new System.EventHandler(this.CustomScrollBarV_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelHandle;
	}
}
