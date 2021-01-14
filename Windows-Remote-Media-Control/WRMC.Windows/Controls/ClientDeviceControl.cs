using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WRMC.Core.Networking;
using WRMC.Windows.Networking;
using WRMC.Windows.Properties;

namespace WRMC.Windows.Controls {
	public partial class ClientDeviceControl : UserControl {
		private static Image DISCONNECT_IMAGE = Resources.disconnect_48;
		private static Image DISCONNECT_IMAGE_ACTIVE = Resources.disconnect_48_active;

		private ClientDevice _clientDevice = null;

		public ClientDevice ClientDevice {
			get => this._clientDevice;
			set {
				this._clientDevice = value;

				if (value == null)
					return;

				this.labelDeviceID.Text = this._clientDevice.ID;
				this.labelDeviceName.Text = this._clientDevice.Name;
				this.labelIPAddress.Text = this._clientDevice.IPAddress.ToString();
				this.labelSessionID.Text = this._clientDevice.SessionID.ToString();
			}
		}

		public ClientDeviceControl() {
			this.InitializeComponent();

			this.buttonDisconnect.Click += (s, e) => ConnectionManager.CloseConnection(this.ClientDevice);
			this.buttonDisconnect.MouseEnter += (s, e) => { this.buttonDisconnect.BackgroundImage = DISCONNECT_IMAGE_ACTIVE; };
			this.buttonDisconnect.MouseLeave += (s, e) => { this.buttonDisconnect.BackgroundImage = DISCONNECT_IMAGE; };
		}
	}
}