using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using WRMC.Core.Models;
using WRMC.Windows.Controls;
using WRMC.Windows.Media;

namespace WRMC.Windows {
	public partial class Form1 : Form {
		protected override CreateParams CreateParams {
			get {
				CreateParams param = base.CreateParams;
				//param.ExStyle |= 0x02000000;
				return param;
			}
		}

		public Form1() {
			this.InitializeComponent();

			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.comboBoxCloseAction.DataSource = Enum.GetValues(typeof(FormCloseAction));
			this.comboBoxCloseAction.SelectedItem = FormCloseAction.Minimize;

			this.notifyIcon.Icon = SystemIcons.Application;

			this.toolStripButtonExit.Click += (s, e) => { Application.Exit(); };
			this.toolStripButtonShowHide.Click += (s, e) => {
				if (this.Visible)
					this.Hide();
				else
					this.Show();

				this.toolStripButtonShowHide.Text = this.Visible ? "Minimize to Tray" : "Show Window";
			};

			MediaSessionExtractor.Default = new TransportControlsMediaSessionExtractor();
			MediaSessionExtractor.Default.OnSessionsChanged += (s, e) => this.UpdateSessionList();
			//MediaSessionExtractor.Default.Initialise();
		}

		private void Form1_Load(object sender, System.EventArgs e) {
			MediaSessionExtractor.Default.Initialise();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				this.Hide();
				this.toolStripButtonShowHide.Text = "Show Window";
			}
		}

		protected override void OnPaint(PaintEventArgs e) {
			this.DoubleBuffered = true;
			base.OnPaint(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			this.DoubleBuffered = true;
			base.OnPaintBackground(e);
		}

		private void UpdateSessionList() {
			this.scrollablePanel.Controls.Clear();

			this.scrollablePanel.Invoke(new Action(() => {
				for (int i = 0; i < MediaSessionExtractor.Default.Sessions.Count; i++) {
					try {
						this.scrollablePanel.Controls.Add(new MediaSessionControl() {
							MediaSession = MediaSessionExtractor.Default.Sessions[i],
							Location = new Point(0, i * 40),
							Margin = new Padding(0),
							Padding = new Padding(0)
						});
					} catch (ArgumentOutOfRangeException) { }
				}
			}));
		}
	}
}