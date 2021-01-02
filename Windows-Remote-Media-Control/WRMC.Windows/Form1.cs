using System.Drawing;
using System.Windows.Forms;

namespace WRMC.Windows {
	public partial class Form1 : Form {
		public Form1() {
			this.InitializeComponent();

			for (int i = 0; i < 10; i++)
				this.scrollablePanel1.Controls.Add(new Panel() {
					Location = new Point(i * 200, 0),
					Size = new Size(200, this.scrollablePanel1.Height + 10),
					BackColor = i % 2 == 0 ? Color.DarkGreen : Color.Blue,
					Margin = new Padding(0),
					Padding = new Padding(0)
				});
		}
	}
}
