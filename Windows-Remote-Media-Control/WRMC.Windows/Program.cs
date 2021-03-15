using System;
using System.Windows.Forms;

using WRMC.Core;

namespace WRMC.Windows {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Logging.LoggingPath = "";
			Logging.IsAllowed = (o) => true;
			Logging.Initialize();
			AppDomain.CurrentDomain.UnhandledException += (s, e) => Logging.FatalException(e.ExceptionObject as Exception);
			Application.ThreadException += (s, e) => Logging.FatalException(e.Exception);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

			try {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			} catch (Exception e) {
				Logging.FatalException(e);
			}
		}
	}
}
