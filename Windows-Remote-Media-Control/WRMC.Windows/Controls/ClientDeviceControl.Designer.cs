
namespace WRMC.Windows.Controls {
	partial class ClientDeviceControl {
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
			this.labelSessionID = new System.Windows.Forms.Label();
			this.labelIPAddress = new System.Windows.Forms.Label();
			this.labelDeviceName = new System.Windows.Forms.Label();
			this.labelDeviceID = new System.Windows.Forms.Label();
			this.buttonDisconnect = new WRMC.Windows.Controls.NoFocusCueButton();
			this.SuspendLayout();
			// 
			// labelSessionID
			// 
			this.labelSessionID.AutoEllipsis = true;
			this.labelSessionID.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelSessionID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSessionID.ForeColor = System.Drawing.Color.White;
			this.labelSessionID.Location = new System.Drawing.Point(640, 0);
			this.labelSessionID.Name = "labelSessionID";
			this.labelSessionID.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelSessionID.Size = new System.Drawing.Size(160, 40);
			this.labelSessionID.TabIndex = 11;
			this.labelSessionID.Text = "41381095379666136";
			this.labelSessionID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelIPAddress
			// 
			this.labelIPAddress.AutoEllipsis = true;
			this.labelIPAddress.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelIPAddress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelIPAddress.ForeColor = System.Drawing.Color.White;
			this.labelIPAddress.Location = new System.Drawing.Point(440, 0);
			this.labelIPAddress.Name = "labelIPAddress";
			this.labelIPAddress.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelIPAddress.Size = new System.Drawing.Size(200, 40);
			this.labelIPAddress.TabIndex = 10;
			this.labelIPAddress.Text = "192.168.178.29";
			this.labelIPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelDeviceName
			// 
			this.labelDeviceName.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelDeviceName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDeviceName.ForeColor = System.Drawing.Color.White;
			this.labelDeviceName.Location = new System.Drawing.Point(200, 0);
			this.labelDeviceName.Name = "labelDeviceName";
			this.labelDeviceName.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelDeviceName.Size = new System.Drawing.Size(240, 40);
			this.labelDeviceName.TabIndex = 9;
			this.labelDeviceName.Text = "DESKTOP-RRML6J9";
			this.labelDeviceName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelDeviceID
			// 
			this.labelDeviceID.AutoEllipsis = true;
			this.labelDeviceID.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelDeviceID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDeviceID.ForeColor = System.Drawing.Color.White;
			this.labelDeviceID.Location = new System.Drawing.Point(0, 0);
			this.labelDeviceID.Name = "labelDeviceID";
			this.labelDeviceID.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelDeviceID.Size = new System.Drawing.Size(200, 40);
			this.labelDeviceID.TabIndex = 8;
			this.labelDeviceID.Text = "xRO1z8hWvyTPg2DZ4OvYRtiwrxjP5LdPoT8L6UsPFDg=";
			this.labelDeviceID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonDisconnect
			// 
			this.buttonDisconnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.buttonDisconnect.Cursor = System.Windows.Forms.Cursors.Hand;
			this.buttonDisconnect.FlatAppearance.BorderSize = 0;
			this.buttonDisconnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.buttonDisconnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.buttonDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonDisconnect.Location = new System.Drawing.Point(813, 8);
			this.buttonDisconnect.Name = "buttonDisconnect";
			this.buttonDisconnect.Size = new System.Drawing.Size(24, 24);
			this.buttonDisconnect.TabIndex = 12;
			this.buttonDisconnect.UseVisualStyleBackColor = true;
			// 
			// ClientDeviceControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.buttonDisconnect);
			this.Controls.Add(this.labelSessionID);
			this.Controls.Add(this.labelIPAddress);
			this.Controls.Add(this.labelDeviceName);
			this.Controls.Add(this.labelDeviceID);
			this.Name = "ClientDeviceControl";
			this.Size = new System.Drawing.Size(890, 40);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelSessionID;
		private System.Windows.Forms.Label labelIPAddress;
		private System.Windows.Forms.Label labelDeviceName;
		private System.Windows.Forms.Label labelDeviceID;
		private NoFocusCueButton buttonDisconnect;
	}
}
