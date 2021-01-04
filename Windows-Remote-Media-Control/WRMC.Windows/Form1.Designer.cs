
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
			this.components = new System.ComponentModel.Container();
			this.panelTitleBar = new System.Windows.Forms.Panel();
			this.panelMenu = new System.Windows.Forms.Panel();
			this.panelSettings = new System.Windows.Forms.Panel();
			this.labelCloseAction = new System.Windows.Forms.Label();
			this.panelDevices = new System.Windows.Forms.Panel();
			this.panelSessions = new System.Windows.Forms.Panel();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripButtonShowHide = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonExit = new System.Windows.Forms.ToolStripMenuItem();
			this.scrollablePanel = new WRMC.Windows.Controls.ScrollablePanel();
			this.customScrollBarV = new WRMC.Windows.Controls.CustomScrollBarV();
			this.comboBoxCloseAction = new WRMC.Windows.Controls.CustomComboBox();
			this.buttonSettings = new WRMC.Windows.Controls.BringToFrontButton();
			this.buttonDevices = new WRMC.Windows.Controls.BringToFrontButton();
			this.buttonSessions = new WRMC.Windows.Controls.BringToFrontButton();
			this.buttonMinimize = new WRMC.Windows.Controls.WindowsDefaultTitleBarButton();
			this.buttonClose = new WRMC.Windows.Controls.WindowsDefaultTitleBarButton();
			this.formDragControl = new WRMC.Windows.Controls.FormDragControl();
			this.panelTitleBar.SuspendLayout();
			this.panelMenu.SuspendLayout();
			this.panelSettings.SuspendLayout();
			this.panelSessions.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelTitleBar
			// 
			this.panelTitleBar.BackColor = System.Drawing.Color.Black;
			this.panelTitleBar.Controls.Add(this.buttonMinimize);
			this.panelTitleBar.Controls.Add(this.buttonClose);
			this.panelTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTitleBar.Location = new System.Drawing.Point(0, 0);
			this.panelTitleBar.Name = "panelTitleBar";
			this.panelTitleBar.Size = new System.Drawing.Size(1090, 29);
			this.panelTitleBar.TabIndex = 0;
			// 
			// panelMenu
			// 
			this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
			this.panelMenu.Controls.Add(this.buttonSettings);
			this.panelMenu.Controls.Add(this.buttonDevices);
			this.panelMenu.Controls.Add(this.buttonSessions);
			this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelMenu.Location = new System.Drawing.Point(0, 29);
			this.panelMenu.Name = "panelMenu";
			this.panelMenu.Size = new System.Drawing.Size(200, 636);
			this.panelMenu.TabIndex = 1;
			// 
			// panelSettings
			// 
			this.panelSettings.Controls.Add(this.labelCloseAction);
			this.panelSettings.Controls.Add(this.comboBoxCloseAction);
			this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSettings.Location = new System.Drawing.Point(200, 29);
			this.panelSettings.Name = "panelSettings";
			this.panelSettings.Size = new System.Drawing.Size(890, 636);
			this.panelSettings.TabIndex = 4;
			// 
			// labelCloseAction
			// 
			this.labelCloseAction.AutoSize = true;
			this.labelCloseAction.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCloseAction.ForeColor = System.Drawing.Color.White;
			this.labelCloseAction.Location = new System.Drawing.Point(16, 17);
			this.labelCloseAction.Name = "labelCloseAction";
			this.labelCloseAction.Size = new System.Drawing.Size(113, 17);
			this.labelCloseAction.TabIndex = 1;
			this.labelCloseAction.Text = "FormClose Action:";
			// 
			// panelDevices
			// 
			this.panelDevices.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelDevices.Location = new System.Drawing.Point(200, 29);
			this.panelDevices.Name = "panelDevices";
			this.panelDevices.Size = new System.Drawing.Size(890, 636);
			this.panelDevices.TabIndex = 4;
			// 
			// panelSessions
			// 
			this.panelSessions.Controls.Add(this.scrollablePanel);
			this.panelSessions.Controls.Add(this.customScrollBarV);
			this.panelSessions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSessions.Location = new System.Drawing.Point(200, 29);
			this.panelSessions.Name = "panelSessions";
			this.panelSessions.Size = new System.Drawing.Size(890, 636);
			this.panelSessions.TabIndex = 3;
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
			this.notifyIcon.Text = "Windows Remote Media Control";
			this.notifyIcon.Visible = true;
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonShowHide,
            this.toolStripSeparator,
            this.toolStripButtonExit});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(162, 54);
			// 
			// toolStripButtonShowHide
			// 
			this.toolStripButtonShowHide.Name = "toolStripButtonShowHide";
			this.toolStripButtonShowHide.Size = new System.Drawing.Size(161, 22);
			this.toolStripButtonShowHide.Text = "Minimize to Tray";
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(158, 6);
			// 
			// toolStripButtonExit
			// 
			this.toolStripButtonExit.Name = "toolStripButtonExit";
			this.toolStripButtonExit.Size = new System.Drawing.Size(161, 22);
			this.toolStripButtonExit.Text = "Exit";
			// 
			// scrollablePanel
			// 
			this.scrollablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scrollablePanel.Location = new System.Drawing.Point(0, 0);
			this.scrollablePanel.Margin = new System.Windows.Forms.Padding(0);
			this.scrollablePanel.Name = "scrollablePanel";
			this.scrollablePanel.ScrollBarV = this.customScrollBarV;
			this.scrollablePanel.Size = new System.Drawing.Size(884, 636);
			this.scrollablePanel.TabIndex = 1;
			// 
			// customScrollBarV
			// 
			this.customScrollBarV.Dock = System.Windows.Forms.DockStyle.Right;
			this.customScrollBarV.HandleClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
			this.customScrollBarV.HandleColor = System.Drawing.Color.Black;
			this.customScrollBarV.HandleHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.customScrollBarV.InactiveWidth = 12;
			this.customScrollBarV.Location = new System.Drawing.Point(884, 0);
			this.customScrollBarV.Name = "customScrollBarV";
			this.customScrollBarV.ScrollValue = 0F;
			this.customScrollBarV.Size = new System.Drawing.Size(6, 636);
			this.customScrollBarV.TabIndex = 0;
			this.customScrollBarV.Target = this.scrollablePanel;
			this.customScrollBarV.VisiblePercent = 100F;
			// 
			// comboBoxCloseAction
			// 
			this.comboBoxCloseAction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
			this.comboBoxCloseAction.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.comboBoxCloseAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCloseAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboBoxCloseAction.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBoxCloseAction.ForeColor = System.Drawing.Color.White;
			this.comboBoxCloseAction.FormattingEnabled = true;
			this.comboBoxCloseAction.ItemHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.comboBoxCloseAction.Location = new System.Drawing.Point(154, 14);
			this.comboBoxCloseAction.Name = "comboBoxCloseAction";
			this.comboBoxCloseAction.Size = new System.Drawing.Size(121, 26);
			this.comboBoxCloseAction.TabIndex = 0;
			// 
			// buttonSettings
			// 
			this.buttonSettings.Dock = System.Windows.Forms.DockStyle.Top;
			this.buttonSettings.FlatAppearance.BorderSize = 0;
			this.buttonSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
			this.buttonSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonSettings.ForeColor = System.Drawing.Color.White;
			this.buttonSettings.Location = new System.Drawing.Point(0, 100);
			this.buttonSettings.Name = "buttonSettings";
			this.buttonSettings.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
			this.buttonSettings.Size = new System.Drawing.Size(200, 50);
			this.buttonSettings.TabIndex = 1;
			this.buttonSettings.Target = this.panelSettings;
			this.buttonSettings.Text = "Settings";
			this.buttonSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonSettings.UseVisualStyleBackColor = true;
			// 
			// buttonDevices
			// 
			this.buttonDevices.Dock = System.Windows.Forms.DockStyle.Top;
			this.buttonDevices.FlatAppearance.BorderSize = 0;
			this.buttonDevices.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
			this.buttonDevices.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.buttonDevices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonDevices.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonDevices.ForeColor = System.Drawing.Color.White;
			this.buttonDevices.Location = new System.Drawing.Point(0, 50);
			this.buttonDevices.Name = "buttonDevices";
			this.buttonDevices.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
			this.buttonDevices.Size = new System.Drawing.Size(200, 50);
			this.buttonDevices.TabIndex = 2;
			this.buttonDevices.Target = this.panelDevices;
			this.buttonDevices.Text = "Devices";
			this.buttonDevices.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonDevices.UseVisualStyleBackColor = true;
			// 
			// buttonSessions
			// 
			this.buttonSessions.Dock = System.Windows.Forms.DockStyle.Top;
			this.buttonSessions.FlatAppearance.BorderSize = 0;
			this.buttonSessions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
			this.buttonSessions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.buttonSessions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSessions.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonSessions.ForeColor = System.Drawing.Color.White;
			this.buttonSessions.Location = new System.Drawing.Point(0, 0);
			this.buttonSessions.Name = "buttonSessions";
			this.buttonSessions.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
			this.buttonSessions.Size = new System.Drawing.Size(200, 50);
			this.buttonSessions.TabIndex = 0;
			this.buttonSessions.Target = this.panelSessions;
			this.buttonSessions.Text = "Sessions";
			this.buttonSessions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonSessions.UseVisualStyleBackColor = true;
			// 
			// buttonMinimize
			// 
			this.buttonMinimize.ButtonType = WRMC.Windows.Controls.WindowsDefaultTitleBarButton.Type.Minimize;
			this.buttonMinimize.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
			this.buttonMinimize.ClickIconColor = System.Drawing.Color.White;
			this.buttonMinimize.Dock = System.Windows.Forms.DockStyle.Right;
			this.buttonMinimize.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.buttonMinimize.HoverIconColor = System.Drawing.Color.White;
			this.buttonMinimize.IconColor = System.Drawing.Color.White;
			this.buttonMinimize.Location = new System.Drawing.Point(1000, 0);
			this.buttonMinimize.Name = "buttonMinimize";
			this.buttonMinimize.Size = new System.Drawing.Size(45, 29);
			this.buttonMinimize.TabIndex = 1;
			this.buttonMinimize.Text = "windowsDefaultTitleBarButton2";
			this.buttonMinimize.UseVisualStyleBackColor = true;
			// 
			// buttonClose
			// 
			this.buttonClose.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(112)))), ((int)(((byte)(122)))));
			this.buttonClose.ClickIconColor = System.Drawing.Color.White;
			this.buttonClose.Dock = System.Windows.Forms.DockStyle.Right;
			this.buttonClose.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
			this.buttonClose.HoverIconColor = System.Drawing.Color.White;
			this.buttonClose.IconColor = System.Drawing.Color.White;
			this.buttonClose.Location = new System.Drawing.Point(1045, 0);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(45, 29);
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "windowsDefaultTitleBarButton1";
			this.buttonClose.UseVisualStyleBackColor = true;
			// 
			// formDragControl
			// 
			this.formDragControl.Target = this.panelTitleBar;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
			this.ClientSize = new System.Drawing.Size(1090, 665);
			this.Controls.Add(this.panelSessions);
			this.Controls.Add(this.panelSettings);
			this.Controls.Add(this.panelDevices);
			this.Controls.Add(this.panelMenu);
			this.Controls.Add(this.panelTitleBar);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += this.Form1_Load;
			this.panelTitleBar.ResumeLayout(false);
			this.panelMenu.ResumeLayout(false);
			this.panelSettings.ResumeLayout(false);
			this.panelSettings.PerformLayout();
			this.panelSessions.ResumeLayout(false);
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelTitleBar;
		private Controls.WindowsDefaultTitleBarButton buttonClose;
		private Controls.WindowsDefaultTitleBarButton buttonMinimize;
		private Controls.FormDragControl formDragControl;
		private System.Windows.Forms.Panel panelMenu;
		private Controls.BringToFrontButton buttonSessions;
		private Controls.BringToFrontButton buttonSettings;
		private Controls.BringToFrontButton buttonDevices;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem toolStripButtonShowHide;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem toolStripButtonExit;
		private System.Windows.Forms.Panel panelSessions;
		private System.Windows.Forms.Panel panelDevices;
		private System.Windows.Forms.Panel panelSettings;
		private Controls.CustomComboBox comboBoxCloseAction;
		private System.Windows.Forms.Label labelCloseAction;
		private Controls.ScrollablePanel scrollablePanel;
		private Controls.CustomScrollBarV customScrollBarV;
	}
}