using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace WRMC.Windows {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			AppDomain.CurrentDomain.UnhandledException += (s, e) => LogException(e.ExceptionObject as Exception);
			Application.ThreadException += (s, e) => LogException(e.Exception);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

			try {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			} catch (Exception e) {
				LogException(e);
			}
		}

		private static void LogException(Exception e) {
			string filePath = "error_log.txt";

			File.AppendAllText(filePath,
				string.Format(
					"-----[{0}] {1}\r\n" +
					"Exception: {2}\r\n" +
					"Message: {3}\r\n" +
					"Stack: \r\n{4}" +
					"\r\n\r\n",

					DateTime.Now.ToString("F", CultureInfo.CreateSpecificCulture("en-US")),
					new string('-', 100),
					e.GetType(),
					e.Message,
					e.StackTrace
				)
			);
		}
	}
}
