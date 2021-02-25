using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using WRMC.Core.Models;
using WRMC.Windows.Controls;
using WRMC.Windows.Media;
using WRMC.Windows.Networking;

namespace WRMC.Windows {
	public partial class Form1 : Form {
		private object updateSessionsLock = new object();

		protected override CreateParams CreateParams {
			get {
				CreateParams param = base.CreateParams;
				param.ExStyle |= 0x02000000;
				return param;
			}
		}

		public Form1() {
			this.InitializeComponent();

			Settings.Load();

			MediaSessionExtractor.Default = Activator.CreateInstance(Settings.Current.SessionExtractor) as MediaSessionExtractor;
			
			MediaSessionExtractor.Default.OnSessionsChanged += (s, e) => {
				this.UpdateSessionList();
				ConnectionManager.SendMediaSessionsChanged(MediaSessionExtractor.Default.Sessions);
			};

			MediaSessionExtractor.Default.OnSessionChanged += (s, e) => {
				this.UpdateSessionList();
				ConnectionManager.SendMediaSessionChanged(e.Data);
			};

			MediaCommandInvoker.SetInvoker(MediaSessionExtractor.Default.GetType());

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.customComboBoxCloseAction.DataSource = Enum.GetValues(typeof(FormCloseAction));
			this.customComboBoxCloseAction.SelectedItem = Settings.Current.CloseAction;

			this.customComboBoxConnectionRequestHandlingMethod.DataSource = Enum.GetValues(typeof(ConnectionRequestHandlingMethod));
			this.customComboBoxConnectionRequestHandlingMethod.SelectedItem = Settings.Current.RequestHandlingMethod;

			this.customComboBoxSessionExtractor.DisplayMember = "Name";
			this.customComboBoxSessionExtractor.DataSource = MediaCommandInvoker.RegisteredExtractors;

			this.customComboBoxSessionExtractor.SelectedItem = Settings.Current.SessionExtractor.GetType();

			this.toolStripButtonExit.Click += (s, e) => { Application.Exit(); };
			this.toolStripButtonShowHide.Click += (s, e) => {
				if (this.Visible)
					this.Hide();
				else
					this.Show();

				this.toolStripButtonShowHide.Text = this.Visible ? "Minimize to Tray" : "Show Window";
			};

			Application.ApplicationExit += (s, e) => {
				this.notifyIcon.Dispose();
			};

			ConnectionManager.OnClientsChanged += (s, e) => this.UpdateDeviceList();
			ConnectionManager.OnConnectionRequestReceived += this.ConnectionManager_OnConnectionRequestReceived;

			Trace.WriteLine(DeviceInformation.IPAddress);
		}

		private void Form1_Load(object sender, EventArgs e) {
			Task.Factory.StartNew(MediaSessionExtractor.Default.Initialise);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.CloseReason != CloseReason.UserClosing)
				return;

			if (Settings.Current.CloseAction == FormCloseAction.Exit)
				return;

			e.Cancel = true;
			this.Hide();
			this.toolStripButtonShowHide.Text = "Show Window";
		}

		private void UpdateSessionList() {
			lock (this.updateSessionsLock) {
				this.scrollablePanelMediaSessions.Controls.Clear();

				this.scrollablePanelMediaSessions.Invoke(new Action(() => {
					for (int i = 0; i < MediaSessionExtractor.Default.Sessions.Count; i++) {
						try {
							this.scrollablePanelMediaSessions.Controls.Add(new MediaSessionControl(MediaSessionExtractor.Default.Sessions[i]) {
								Location = new Point(0, i * 40),
								Margin = new Padding(0),
								Padding = new Padding(0)
							});
						}
						catch (ArgumentOutOfRangeException) { }
					}

					this.scrollablePanelMediaSessions.Invalidate();
				}));
			}
		}

		private void UpdateDeviceList() {
			this.scrollablePanelDevices.Controls.Clear();

			this.scrollablePanelDevices.Invoke(new Action(() => {
				for (int i = 0; i < ConnectionManager.Clients.Count; i++) {
					try {
						this.scrollablePanelDevices.Controls.Add(new ClientDeviceControl() {
							ClientDevice = ConnectionManager.Clients[i],
							Location = new Point(0, i * 40),
							Margin = new Padding(0),
							Padding = new Padding(0)
						});
					}
					catch (ArgumentOutOfRangeException) { }
				}

				this.scrollablePanelDevices.Invalidate();
			}));
		}

		private void comboBoxCloseAction_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Current.CloseAction = (FormCloseAction)(sender as CustomComboBox).SelectedItem;
			Settings.Save();
		}

		private void customComboBoxSessionExtractor_SelectionChangeCommitted(object sender, EventArgs e) {
			Type type = (Type)(sender as CustomComboBox).SelectedItem;

			Settings.Current.SessionExtractor = type;

			MediaSessionExtractor.Default = Activator.CreateInstance(Settings.Current.SessionExtractor) as MediaSessionExtractor;
			MediaCommandInvoker.SetInvoker(type);

			Settings.Save();
		}

		private void customComboBoxConnectionRequestHandlingMethod_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Current.RequestHandlingMethod = (ConnectionRequestHandlingMethod)(sender as CustomComboBox).SelectedItem;
			Settings.Save();
		}

		private void ConnectionManager_OnConnectionRequestReceived(object sender, Core.Networking.ClientRequestEventArgs e) {
			switch (Settings.Current.RequestHandlingMethod) {
				case ConnectionRequestHandlingMethod.AcceptAll:
					ConnectionManager.AcceptConnection(e.Client, e.ClientDevice);
					break;

				case ConnectionRequestHandlingMethod.Ask:
					DialogResult dialogResult = MessageBox.Show("Do you want to accept the connection request from " + e.ClientDevice.Name + "?", "Connectio Requested", MessageBoxButtons.YesNo);
					
					if (dialogResult == DialogResult.Yes)
						ConnectionManager.AcceptConnection(e.Client, e.ClientDevice);

					break;

				case ConnectionRequestHandlingMethod.RejectAll:
					break;
			}
		}
	}
}