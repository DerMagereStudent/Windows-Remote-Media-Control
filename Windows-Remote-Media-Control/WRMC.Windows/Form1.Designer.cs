
namespace WRMC.Windows {
	partial class Form1 {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.scrollablePanel1 = new WRMC.Windows.Controls.ScrollablePanel();
			this.customScrollBarH1 = new WRMC.Windows.Controls.CustomScrollBarH();
			this.SuspendLayout();
			// 
			// scrollablePanel1
			// 
			this.scrollablePanel1.BackColor = System.Drawing.SystemColors.Control;
			this.scrollablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scrollablePanel1.Location = new System.Drawing.Point(0, 0);
			this.scrollablePanel1.Margin = new System.Windows.Forms.Padding(0);
			this.scrollablePanel1.Name = "scrollablePanel1";
			this.scrollablePanel1.ScrollBarH = this.customScrollBarH1;
			this.scrollablePanel1.Size = new System.Drawing.Size(800, 444);
			this.scrollablePanel1.TabIndex = 0;
			// 
			// customScrollBarH1
			// 
			this.customScrollBarH1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.customScrollBarH1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.customScrollBarH1.HandleClickColor = System.Drawing.Color.Yellow;
			this.customScrollBarH1.HandleColor = System.Drawing.Color.Gray;
			this.customScrollBarH1.HandleHoverColor = System.Drawing.Color.DarkGray;
			this.customScrollBarH1.Location = new System.Drawing.Point(0, 444);
			this.customScrollBarH1.Name = "customScrollBarH1";
			this.customScrollBarH1.ScrollValue = 0F;
			this.customScrollBarH1.Size = new System.Drawing.Size(800, 6);
			this.customScrollBarH1.TabIndex = 1;
			this.customScrollBarH1.Target = this.scrollablePanel1;
			this.customScrollBarH1.VisiblePercent = 0F;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Red;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.scrollablePanel1);
			this.Controls.Add(this.customScrollBarH1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private Controls.ScrollablePanel scrollablePanel1;
		private Controls.CustomScrollBarH customScrollBarH1;
	}
}

