using NAudio.CoreAudioApi;
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
using WRMC.Windows.Native;

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

			MediaSessionExtractor.Default = Settings.Current.SessionExtractor;
			MediaSessionExtractor.Default.OnSessionsChanged += (s, e) => this.UpdateSessionList();

			MediaCommandInvoker.SetInvoker(MediaSessionExtractor.Default.GetType());

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.comboBoxCloseAction.DataSource = Enum.GetValues(typeof(FormCloseAction));
			this.comboBoxCloseAction.SelectedItem = Settings.Current.CloseAction;

			this.customComboBoxSessionExtractor.DisplayMember = "Name";
			this.customComboBoxSessionExtractor.DataSource = MediaCommandInvoker.RegisteredExtractors;

			this.customComboBoxSessionExtractor.SelectedItem = Settings.Current.SessionExtractor.GetType();

			this.notifyIcon.Icon = SystemIcons.Application;

			this.toolStripButtonExit.Click += (s, e) => { Application.Exit(); };
			this.toolStripButtonShowHide.Click += (s, e) => {
				if (this.Visible)
					this.Hide();
				else
					this.Show();

				this.toolStripButtonShowHide.Text = this.Visible ? "Minimize to Tray" : "Show Window";
			};
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
				this.scrollablePanel.Controls.Clear();

				this.scrollablePanel.Invoke(new Action(() => {
					for (int i = 0; i < MediaSessionExtractor.Default.Sessions.Count; i++) {
						try {
							this.scrollablePanel.Controls.Add(new MediaSessionControl(MediaSessionExtractor.Default.Sessions[i]) {
								Location = new Point(0, i * 40),
								Margin = new Padding(0),
								Padding = new Padding(0)
							});
						}
						catch (ArgumentOutOfRangeException) { }
					}

					this.scrollablePanel.Invalidate();
				}));
			}
		}

		private void comboBoxCloseAction_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Current.CloseAction = (FormCloseAction)(sender as CustomComboBox).SelectedItem;
			Settings.Save();
		}

		private void customComboBoxSessionExtractor_SelectionChangeCommitted(object sender, EventArgs e) {
			Type type = (Type)(sender as CustomComboBox).SelectedItem;

			Settings.Current.SessionExtractor = Activator.CreateInstance(type) as MediaSessionExtractor;

			MediaSessionExtractor.Default = Settings.Current.SessionExtractor;
			MediaCommandInvoker.SetInvoker(type);

			Settings.Save();
		}
	}
}