
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
			this.labelConnectionRequestHandlingMethod = new System.Windows.Forms.Label();
			this.labelSessionExtractor = new System.Windows.Forms.Label();
			this.labelCloseAction = new System.Windows.Forms.Label();
			this.panelDevices = new System.Windows.Forms.Panel();
			this.panelSessions = new System.Windows.Forms.Panel();
			this.panelSessionsHeader = new System.Windows.Forms.Panel();
			this.labelHeaderArtist = new System.Windows.Forms.Label();
			this.labelHeaderTitle = new System.Windows.Forms.Label();
			this.labelHeaderProcessName = new System.Windows.Forms.Label();
			this.labelHeaderProcessID = new System.Windows.Forms.Label();
			this.panelMenuPlaceHolder = new System.Windows.Forms.Panel();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripButtonShowHide = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonExit = new System.Windows.Forms.ToolStripMenuItem();
			this.scrollablePanelMediaSessions = new WRMC.Windows.Controls.ScrollablePanel();
			this.customScrollBarVMediaSessions = new WRMC.Windows.Controls.CustomScrollBarV();
			this.customComboBoxConnectionRequestHandlingMethod = new WRMC.Windows.Controls.CustomComboBox();
			this.customComboBoxSessionExtractor = new WRMC.Windows.Controls.CustomComboBox();
			this.customComboBoxCloseAction = new WRMC.Windows.Controls.CustomComboBox();
			this.buttonSettings = new WRMC.Windows.Controls.BringToFrontButton();
			this.buttonDevices = new WRMC.Windows.Controls.BringToFrontButton();
			this.buttonSessions = new WRMC.Windows.Controls.BringToFrontButton();
			this.buttonMinimize = new WRMC.Windows.Controls.WindowsDefaultTitleBarButton();
			this.buttonClose = new WRMC.Windows.Controls.WindowsDefaultTitleBarButton();
			this.formDragControl = new WRMC.Windows.Controls.FormDragControl();
			this.panelDevicesHeader = new System.Windows.Forms.Panel();
			this.labelHeaderSessionID = new System.Windows.Forms.Label();
			this.labelHeaderIPAddress = new System.Windows.Forms.Label();
			this.labelDeviceName = new System.Windows.Forms.Label();
			this.labelHeaderDeviceID = new System.Windows.Forms.Label();
			this.scrollablePanelDevices = new WRMC.Windows.Controls.ScrollablePanel();
			this.customScrollBarVDevices = new WRMC.Windows.Controls.CustomScrollBarV();
			this.panelTitleBar.SuspendLayout();
			this.panelMenu.SuspendLayout();
			this.panelSettings.SuspendLayout();
			this.panelDevices.SuspendLayout();
			this.panelSessions.SuspendLayout();
			this.panelSessionsHeader.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			this.panelDevicesHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelTitleBar
			// 
			this.panelTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(75)))), ((int)(((byte)(105)))));
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
			this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(69)))));
			this.panelMenu.Controls.Add(this.buttonSettings);
			this.panelMenu.Controls.Add(this.buttonDevices);
			this.panelMenu.Controls.Add(this.buttonSessions);
			this.panelMenu.Controls.Add(this.panelMenuPlaceHolder);
			this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelMenu.Location = new System.Drawing.Point(0, 29);
			this.panelMenu.Name = "panelMenu";
			this.panelMenu.Size = new System.Drawing.Size(200, 636);
			this.panelMenu.TabIndex = 1;
			// 
			// panelSettings
			// 
			this.panelSettings.Controls.Add(this.labelConnectionRequestHandlingMethod);
			this.panelSettings.Controls.Add(this.customComboBoxConnectionRequestHandlingMethod);
			this.panelSettings.Controls.Add(this.labelSessionExtractor);
			this.panelSettings.Controls.Add(this.customComboBoxSessionExtractor);
			this.panelSettings.Controls.Add(this.labelCloseAction);
			this.panelSettings.Controls.Add(this.customComboBoxCloseAction);
			this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSettings.Location = new System.Drawing.Point(200, 29);
			this.panelSettings.Name = "panelSettings";
			this.panelSettings.Size = new System.Drawing.Size(890, 636);
			this.panelSettings.TabIndex = 4;
			// 
			// labelConnectionRequestHandlingMethod
			// 
			this.labelConnectionRequestHandlingMethod.AutoSize = true;
			this.labelConnectionRequestHandlingMethod.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelConnectionRequestHandlingMethod.ForeColor = System.Drawing.Color.White;
			this.labelConnectionRequestHandlingMethod.Location = new System.Drawing.Point(16, 87);
			this.labelConnectionRequestHandlingMethod.Name = "labelConnectionRequestHandlingMethod";
			this.labelConnectionRequestHandlingMethod.Size = new System.Drawing.Size(183, 17);
			this.labelConnectionRequestHandlingMethod.TabIndex = 5;
			this.labelConnectionRequestHandlingMethod.Text = "Connection Request Handling:";
			// 
			// labelSessionExtractor
			// 
			this.labelSessionExtractor.AutoSize = true;
			this.labelSessionExtractor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSessionExtractor.ForeColor = System.Drawing.Color.White;
			this.labelSessionExtractor.Location = new System.Drawing.Point(16, 53);
			this.labelSessionExtractor.Name = "labelSessionExtractor";
			this.labelSessionExtractor.Size = new System.Drawing.Size(269, 17);
			this.labelSessionExtractor.TabIndex = 3;
			this.labelSessionExtractor.Text = "Session Extraction and Command Invokation:";
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
			this.panelDevices.Controls.Add(this.scrollablePanelDevices);
			this.panelDevices.Controls.Add(this.panelDevicesHeader);
			this.panelDevices.Controls.Add(this.customScrollBarVDevices);
			this.panelDevices.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelDevices.Location = new System.Drawing.Point(200, 29);
			this.panelDevices.Name = "panelDevices";
			this.panelDevices.Size = new System.Drawing.Size(890, 636);
			this.panelDevices.TabIndex = 4;
			// 
			// panelSessions
			// 
			this.panelSessions.Controls.Add(this.scrollablePanelMediaSessions);
			this.panelSessions.Controls.Add(this.panelSessionsHeader);
			this.panelSessions.Controls.Add(this.customScrollBarVMediaSessions);
			this.panelSessions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSessions.Location = new System.Drawing.Point(200, 29);
			this.panelSessions.Name = "panelSessions";
			this.panelSessions.Size = new System.Drawing.Size(890, 636);
			this.panelSessions.TabIndex = 3;
			// 
			// panelSessionsHeader
			// 
			this.panelSessionsHeader.Controls.Add(this.labelHeaderArtist);
			this.panelSessionsHeader.Controls.Add(this.labelHeaderTitle);
			this.panelSessionsHeader.Controls.Add(this.labelHeaderProcessName);
			this.panelSessionsHeader.Controls.Add(this.labelHeaderProcessID);
			this.panelSessionsHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelSessionsHeader.Location = new System.Drawing.Point(0, 0);
			this.panelSessionsHeader.Name = "panelSessionsHeader";
			this.panelSessionsHeader.Size = new System.Drawing.Size(890, 40);
			this.panelSessionsHeader.TabIndex = 2;
			// 
			// labelHeaderArtist
			// 
			this.labelHeaderArtist.AutoEllipsis = true;
			this.labelHeaderArtist.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelHeaderArtist.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeaderArtist.ForeColor = System.Drawing.Color.White;
			this.labelHeaderArtist.Location = new System.Drawing.Point(600, 0);
			this.labelHeaderArtist.Name = "labelHeaderArtist";
			this.labelHeaderArtist.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelHeaderArtist.Size = new System.Drawing.Size(180, 40);
			this.labelHeaderArtist.TabIndex = 7;
			this.labelHeaderArtist.Text = "Artist";
			this.labelHeaderArtist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelHeaderTitle
			// 
			this.labelHeaderTitle.AutoEllipsis = true;
			this.labelHeaderTitle.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeaderTitle.ForeColor = System.Drawing.Color.White;
			this.labelHeaderTitle.Location = new System.Drawing.Point(260, 0);
			this.labelHeaderTitle.Name = "labelHeaderTitle";
			this.labelHeaderTitle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelHeaderTitle.Size = new System.Drawing.Size(340, 40);
			this.labelHeaderTitle.TabIndex = 6;
			this.labelHeaderTitle.Text = "Title";
			this.labelHeaderTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelHeaderProcessName
			// 
			this.labelHeaderProcessName.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelHeaderProcessName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeaderProcessName.ForeColor = System.Drawing.Color.White;
			this.labelHeaderProcessName.Location = new System.Drawing.Point(100, 0);
			this.labelHeaderProcessName.Name = "labelHeaderProcessName";
			this.labelHeaderProcessName.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelHeaderProcessName.Size = new System.Drawing.Size(160, 40);
			this.labelHeaderProcessName.TabIndex = 5;
			this.labelHeaderProcessName.Text = "Process Name";
			this.labelHeaderProcessName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelHeaderProcessID
			// 
			this.labelHeaderProcessID.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelHeaderProcessID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeaderProcessID.ForeColor = System.Drawing.Color.White;
			this.labelHeaderProcessID.Location = new System.Drawing.Point(0, 0);
			this.labelHeaderProcessID.Name = "labelHeaderProcessID";
			this.labelHeaderProcessID.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelHeaderProcessID.Size = new System.Drawing.Size(100, 40);
			this.labelHeaderProcessID.TabIndex = 4;
			this.labelHeaderProcessID.Text = "Process IDs";
			this.labelHeaderProcessID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelMenuPlaceHolder
			// 
			this.panelMenuPlaceHolder.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelMenuPlaceHolder.Location = new System.Drawing.Point(0, 0);
			this.panelMenuPlaceHolder.Name = "panelMenuPlaceHolder";
			this.panelMenuPlaceHolder.Size = new System.Drawing.Size(200, 70);
			this.panelMenuPlaceHolder.TabIndex = 3;
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
			// scrollablePanelMediaSessions
			// 
			this.scrollablePanelMediaSessions.BackColor = System.Drawing.Color.Transparent;
			this.scrollablePanelMediaSessions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scrollablePanelMediaSessions.Location = new System.Drawing.Point(0, 40);
			this.scrollablePanelMediaSessions.Margin = new System.Windows.Forms.Padding(0);
			this.scrollablePanelMediaSessions.Name = "scrollablePanelMediaSessions";
			this.scrollablePanelMediaSessions.ScrollBarV = this.customScrollBarVMediaSessions;
			this.scrollablePanelMediaSessions.Size = new System.Drawing.Size(890, 596);
			this.scrollablePanelMediaSessions.TabIndex = 1;
			// 
			// customScrollBarVMediaSessions
			// 
			this.customScrollBarVMediaSessions.Dock = System.Windows.Forms.DockStyle.Right;
			this.customScrollBarVMediaSessions.HandleClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(57)))), ((int)(((byte)(80)))));
			this.customScrollBarVMediaSessions.HandleColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(69)))));
			this.customScrollBarVMediaSessions.HandleHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(43)))), ((int)(((byte)(60)))));
			this.customScrollBarVMediaSessions.InactiveWidth = 12;
			this.customScrollBarVMediaSessions.Location = new System.Drawing.Point(890, 0);
			this.customScrollBarVMediaSessions.Name = "customScrollBarVMediaSessions";
			this.customScrollBarVMediaSessions.ScrollValue = 0F;
			this.customScrollBarVMediaSessions.Size = new System.Drawing.Size(0, 636);
			this.customScrollBarVMediaSessions.TabIndex = 0;
			this.customScrollBarVMediaSessions.Target = this.scrollablePanelMediaSessions;
			this.customScrollBarVMediaSessions.VisiblePercent = 100F;
			// 
			// customComboBoxConnectionRequestHandlingMethod
			// 
			this.customComboBoxConnectionRequestHandlingMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(43)))), ((int)(((byte)(60)))));
			this.customComboBoxConnectionRequestHandlingMethod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.customComboBoxConnectionRequestHandlingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.customComboBoxConnectionRequestHandlingMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.customComboBoxConnectionRequestHandlingMethod.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customComboBoxConnectionRequestHandlingMethod.ForeColor = System.Drawing.Color.White;
			this.customComboBoxConnectionRequestHandlingMethod.FormattingEnabled = true;
			this.customComboBoxConnectionRequestHandlingMethod.ItemHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(57)))), ((int)(((byte)(80)))));
			this.customComboBoxConnectionRequestHandlingMethod.Location = new System.Drawing.Point(446, 84);
			this.customComboBoxConnectionRequestHandlingMethod.Name = "customComboBoxConnectionRequestHandlingMethod";
			this.customComboBoxConnectionRequestHandlingMethod.Size = new System.Drawing.Size(121, 26);
			this.customComboBoxConnectionRequestHandlingMethod.TabIndex = 4;
			this.customComboBoxConnectionRequestHandlingMethod.SelectionChangeCommitted += new System.EventHandler(this.customComboBoxConnectionRequestHandlingMethod_SelectionChangeCommitted);
			// 
			// customComboBoxSessionExtractor
			// 
			this.customComboBoxSessionExtractor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(43)))), ((int)(((byte)(60)))));
			this.customComboBoxSessionExtractor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.customComboBoxSessionExtractor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.customComboBoxSessionExtractor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.customComboBoxSessionExtractor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customComboBoxSessionExtractor.ForeColor = System.Drawing.Color.White;
			this.customComboBoxSessionExtractor.FormattingEnabled = true;
			this.customComboBoxSessionExtractor.ItemHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(57)))), ((int)(((byte)(80)))));
			this.customComboBoxSessionExtractor.Location = new System.Drawing.Point(294, 50);
			this.customComboBoxSessionExtractor.Name = "customComboBoxSessionExtractor";
			this.customComboBoxSessionExtractor.Size = new System.Drawing.Size(273, 26);
			this.customComboBoxSessionExtractor.TabIndex = 2;
			this.customComboBoxSessionExtractor.SelectionChangeCommitted += new System.EventHandler(this.customComboBoxSessionExtractor_SelectionChangeCommitted);
			// 
			// customComboBoxCloseAction
			// 
			this.customComboBoxCloseAction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(43)))), ((int)(((byte)(60)))));
			this.customComboBoxCloseAction.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.customComboBoxCloseAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.customComboBoxCloseAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.customComboBoxCloseAction.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customComboBoxCloseAction.ForeColor = System.Drawing.Color.White;
			this.customComboBoxCloseAction.FormattingEnabled = true;
			this.customComboBoxCloseAction.ItemHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(57)))), ((int)(((byte)(80)))));
			this.customComboBoxCloseAction.Location = new System.Drawing.Point(446, 14);
			this.customComboBoxCloseAction.Name = "customComboBoxCloseAction";
			this.customComboBoxCloseAction.Size = new System.Drawing.Size(121, 26);
			this.customComboBoxCloseAction.TabIndex = 0;
			this.customComboBoxCloseAction.SelectionChangeCommitted += new System.EventHandler(this.comboBoxCloseAction_SelectionChangeCommitted);
			// 
			// buttonSettings
			// 
			this.buttonSettings.Dock = System.Windows.Forms.DockStyle.Top;
			this.buttonSettings.FlatAppearance.BorderSize = 0;
			this.buttonSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(232)))), ((int)(((byte)(166)))));
			this.buttonSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(57)))), ((int)(((byte)(80)))));
			this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonSettings.ForeColor = System.Drawing.Color.White;
			this.buttonSettings.Location = new System.Drawing.Point(0, 170);
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
			this.buttonDevices.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(232)))), ((int)(((byte)(166)))));
			this.buttonDevices.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(57)))), ((int)(((byte)(80)))));
			this.buttonDevices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonDevices.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonDevices.ForeColor = System.Drawing.Color.White;
			this.buttonDevices.Location = new System.Drawing.Point(0, 120);
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
			this.buttonSessions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(232)))), ((int)(((byte)(166)))));
			this.buttonSessions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(57)))), ((int)(((byte)(80)))));
			this.buttonSessions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSessions.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonSessions.ForeColor = System.Drawing.Color.White;
			this.buttonSessions.Location = new System.Drawing.Point(0, 70);
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
			this.buttonMinimize.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(46)))), ((int)(((byte)(65)))));
			this.buttonMinimize.ClickIconColor = System.Drawing.Color.White;
			this.buttonMinimize.Dock = System.Windows.Forms.DockStyle.Right;
			this.buttonMinimize.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(57)))), ((int)(((byte)(80)))));
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
			// panelDevicesHeader
			// 
			this.panelDevicesHeader.Controls.Add(this.labelHeaderSessionID);
			this.panelDevicesHeader.Controls.Add(this.labelHeaderIPAddress);
			this.panelDevicesHeader.Controls.Add(this.labelDeviceName);
			this.panelDevicesHeader.Controls.Add(this.labelHeaderDeviceID);
			this.panelDevicesHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelDevicesHeader.Location = new System.Drawing.Point(0, 0);
			this.panelDevicesHeader.Name = "panelDevicesHeader";
			this.panelDevicesHeader.Size = new System.Drawing.Size(884, 40);
			this.panelDevicesHeader.TabIndex = 8;
			// 
			// labelHeaderSessionID
			// 
			this.labelHeaderSessionID.AutoEllipsis = true;
			this.labelHeaderSessionID.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelHeaderSessionID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeaderSessionID.ForeColor = System.Drawing.Color.White;
			this.labelHeaderSessionID.Location = new System.Drawing.Point(640, 0);
			this.labelHeaderSessionID.Name = "labelHeaderSessionID";
			this.labelHeaderSessionID.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelHeaderSessionID.Size = new System.Drawing.Size(160, 40);
			this.labelHeaderSessionID.TabIndex = 7;
			this.labelHeaderSessionID.Text = "Session ID";
			this.labelHeaderSessionID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelHeaderIPAddress
			// 
			this.labelHeaderIPAddress.AutoEllipsis = true;
			this.labelHeaderIPAddress.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelHeaderIPAddress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeaderIPAddress.ForeColor = System.Drawing.Color.White;
			this.labelHeaderIPAddress.Location = new System.Drawing.Point(440, 0);
			this.labelHeaderIPAddress.Name = "labelHeaderIPAddress";
			this.labelHeaderIPAddress.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelHeaderIPAddress.Size = new System.Drawing.Size(200, 40);
			this.labelHeaderIPAddress.TabIndex = 6;
			this.labelHeaderIPAddress.Text = "IP Address";
			this.labelHeaderIPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelDeviceName
			// 
			this.labelDeviceName.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelDeviceName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDeviceName.ForeColor = System.Drawing.Color.White;
			this.labelDeviceName.Location = new System.Drawing.Point(200, 0);
			this.labelDeviceName.Name = "labelDeviceName";
			this.labelDeviceName.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelDeviceName.Size = new System.Drawing.Size(240, 40);
			this.labelDeviceName.TabIndex = 5;
			this.labelDeviceName.Text = "Device Name";
			this.labelDeviceName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelHeaderDeviceID
			// 
			this.labelHeaderDeviceID.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelHeaderDeviceID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeaderDeviceID.ForeColor = System.Drawing.Color.White;
			this.labelHeaderDeviceID.Location = new System.Drawing.Point(0, 0);
			this.labelHeaderDeviceID.Name = "labelHeaderDeviceID";
			this.labelHeaderDeviceID.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.labelHeaderDeviceID.Size = new System.Drawing.Size(200, 40);
			this.labelHeaderDeviceID.TabIndex = 4;
			this.labelHeaderDeviceID.Text = "Device ID";
			this.labelHeaderDeviceID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// scrollablePanelDevices
			// 
			this.scrollablePanelDevices.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scrollablePanelDevices.Location = new System.Drawing.Point(0, 40);
			this.scrollablePanelDevices.Margin = new System.Windows.Forms.Padding(0);
			this.scrollablePanelDevices.Name = "scrollablePanelDevices";
			this.scrollablePanelDevices.Size = new System.Drawing.Size(884, 596);
			this.scrollablePanelDevices.TabIndex = 9;
			// 
			// customScrollBarVDevices
			// 
			this.customScrollBarVDevices.Dock = System.Windows.Forms.DockStyle.Right;
			this.customScrollBarVDevices.HandleClickColor = System.Drawing.Color.Empty;
			this.customScrollBarVDevices.HandleColor = System.Drawing.Color.Empty;
			this.customScrollBarVDevices.HandleHoverColor = System.Drawing.Color.Empty;
			this.customScrollBarVDevices.Location = new System.Drawing.Point(884, 0);
			this.customScrollBarVDevices.Name = "customScrollBarVDevices";
			this.customScrollBarVDevices.ScrollValue = 0F;
			this.customScrollBarVDevices.Size = new System.Drawing.Size(6, 636);
			this.customScrollBarVDevices.TabIndex = 10;
			this.customScrollBarVDevices.VisiblePercent = 0F;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(43)))), ((int)(((byte)(60)))));
			this.ClientSize = new System.Drawing.Size(1090, 665);
			this.Controls.Add(this.panelDevices);
			this.Controls.Add(this.panelSessions);
			this.Controls.Add(this.panelSettings);
			this.Controls.Add(this.panelMenu);
			this.Controls.Add(this.panelTitleBar);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panelTitleBar.ResumeLayout(false);
			this.panelMenu.ResumeLayout(false);
			this.panelSettings.ResumeLayout(false);
			this.panelSettings.PerformLayout();
			this.panelDevices.ResumeLayout(false);
			this.panelSessions.ResumeLayout(false);
			this.panelSessionsHeader.ResumeLayout(false);
			this.contextMenuStrip.ResumeLayout(false);
			this.panelDevicesHeader.ResumeLayout(false);
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
		private Controls.CustomComboBox customComboBoxCloseAction;
		private System.Windows.Forms.Label labelCloseAction;
		private Controls.ScrollablePanel scrollablePanelMediaSessions;
		private Controls.CustomScrollBarV customScrollBarVMediaSessions;
		private System.Windows.Forms.Panel panelSessionsHeader;
		private System.Windows.Forms.Label labelHeaderArtist;
		private System.Windows.Forms.Label labelHeaderTitle;
		private System.Windows.Forms.Label labelHeaderProcessName;
		private System.Windows.Forms.Label labelHeaderProcessID;
		private System.Windows.Forms.Panel panelMenuPlaceHolder;
		private System.Windows.Forms.Label labelSessionExtractor;
		private Controls.CustomComboBox customComboBoxSessionExtractor;
		private System.Windows.Forms.Label labelConnectionRequestHandlingMethod;
		private Controls.CustomComboBox customComboBoxConnectionRequestHandlingMethod;
		private System.Windows.Forms.Panel panelDevicesHeader;
		private System.Windows.Forms.Label labelHeaderSessionID;
		private System.Windows.Forms.Label labelHeaderIPAddress;
		private System.Windows.Forms.Label labelDeviceName;
		private System.Windows.Forms.Label labelHeaderDeviceID;
		private Controls.ScrollablePanel scrollablePanelDevices;
		private Controls.CustomScrollBarV customScrollBarVDevices;
	}
}