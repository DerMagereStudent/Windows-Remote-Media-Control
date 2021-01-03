using System.Drawing;
using System.Windows.Forms;

namespace WRMC.Windows {
	public partial class Form1 : Form {
		public Form1() {
			this.InitializeComponent();
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

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				this.Hide();
				this.toolStripButtonShowHide.Text = "Show Window";
			}
		}
	}
}