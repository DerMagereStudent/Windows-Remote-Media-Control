
namespace WRMC.Windows.Controls {
	partial class MediaSessionControl {
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
			this.labelProcessName = new System.Windows.Forms.Label();
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelArtist = new System.Windows.Forms.Label();
			this.buttonPrevious = new WRMC.Windows.Controls.NoFocusCueButton();
			this.buttonPlayPause = new WRMC.Windows.Controls.NoFocusCueButton();
			this.buttonNext = new WRMC.Windows.Controls.NoFocusCueButton();
			this.SuspendLayout();
			// 
			// labelProcessName
			// 
			this.labelProcessName.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelProcessName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelProcessName.ForeColor = System.Drawing.Color.White;
			this.labelProcessName.Location = new System.Drawing.Point(0, 0);
			this.labelProcessName.Name = "labelProcessName";
			this.labelProcessName.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelProcessName.Size = new System.Drawing.Size(205, 40);
			this.labelProcessName.TabIndex = 1;
			this.labelProcessName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelTitle
			// 
			this.labelTitle.AutoEllipsis = true;
			this.labelTitle.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTitle.ForeColor = System.Drawing.Color.White;
			this.labelTitle.Location = new System.Drawing.Point(205, 0);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelTitle.Size = new System.Drawing.Size(395, 40);
			this.labelTitle.TabIndex = 2;
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelArtist
			// 
			this.labelArtist.AutoEllipsis = true;
			this.labelArtist.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelArtist.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelArtist.ForeColor = System.Drawing.Color.White;
			this.labelArtist.Location = new System.Drawing.Point(600, 0);
			this.labelArtist.Name = "labelArtist";
			this.labelArtist.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelArtist.Size = new System.Drawing.Size(165, 40);
			this.labelArtist.TabIndex = 3;
			this.labelArtist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonPrevious
			// 
			this.buttonPrevious.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.buttonPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
			this.buttonPrevious.FlatAppearance.BorderSize = 0;
			this.buttonPrevious.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.buttonPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonPrevious.Location = new System.Drawing.Point(774, 8);
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.Size = new System.Drawing.Size(24, 24);
			this.buttonPrevious.TabIndex = 4;
			this.buttonPrevious.UseVisualStyleBackColor = true;
			// 
			// buttonPlayPause
			// 
			this.buttonPlayPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.buttonPlayPause.Cursor = System.Windows.Forms.Cursors.Hand;
			this.buttonPlayPause.FlatAppearance.BorderSize = 0;
			this.buttonPlayPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.buttonPlayPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.buttonPlayPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonPlayPause.Location = new System.Drawing.Point(809, 9);
			this.buttonPlayPause.Name = "buttonPlayPause";
			this.buttonPlayPause.Size = new System.Drawing.Size(22, 22);
			this.buttonPlayPause.TabIndex = 5;
			this.buttonPlayPause.UseVisualStyleBackColor = true;
			// 
			// buttonNext
			// 
			this.buttonNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.buttonNext.Cursor = System.Windows.Forms.Cursors.Hand;
			this.buttonNext.FlatAppearance.BorderSize = 0;
			this.buttonNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.buttonNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonNext.Location = new System.Drawing.Point(842, 8);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(24, 24);
			this.buttonNext.TabIndex = 6;
			this.buttonNext.UseVisualStyleBackColor = true;
			// 
			// MediaSessionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonPlayPause);
			this.Controls.Add(this.buttonPrevious);
			this.Controls.Add(this.labelArtist);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.labelProcessName);
			this.DoubleBuffered = true;
			this.Name = "MediaSessionControl";
			this.Size = new System.Drawing.Size(890, 40);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Label labelProcessName;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Label labelArtist;
		private WRMC.Windows.Controls.NoFocusCueButton buttonPrevious;
		private WRMC.Windows.Controls.NoFocusCueButton buttonPlayPause;
		private WRMC.Windows.Controls.NoFocusCueButton buttonNext;
	}
}
