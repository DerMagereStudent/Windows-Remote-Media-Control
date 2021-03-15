using System;
using System.Globalization;
using System.IO;
using WRMC.Core.Networking;

namespace WRMC.Core {
	public static class Logging {
		private const string HANDLED_EXCEPTIONS_FILE_NAME = "handled_exceptions.txt";
		private const string FATAL_EXCEPTIONS_FILE_NAME = "fatal_exceptions.txt";
		private const string NETWORK_LOG_FILE_NAME = "network_log.txt";

		public static string LoggingPath { get; set; }
		public static Predicate<object> IsAllowed { get; set; }

		public static void Initialize() {
			try {
				if (IsAllowed == null || !IsAllowed.Invoke(null))
					return;

				if (!string.IsNullOrEmpty(LoggingPath) && !Directory.Exists(LoggingPath))
					Directory.CreateDirectory(LoggingPath);

				File.WriteAllText(
					Path.Combine(LoggingPath, NETWORK_LOG_FILE_NAME),
					string.Empty
				);
			} catch (IOException) { }
		}

		public static void HandledException(Exception e) {
			try {
				if (IsAllowed == null || !IsAllowed.Invoke(null))
					return;

				if (!string.IsNullOrEmpty(LoggingPath) && !Directory.Exists(LoggingPath))
					Directory.CreateDirectory(LoggingPath);

				File.AppendAllText(
					Path.Combine(LoggingPath, HANDLED_EXCEPTIONS_FILE_NAME),
					string.Format(
						"-----[{0}] {1}\r\n" +
						"Exception: {2}\r\n" +
						"Type: Handled\r\n" +
						"Message: {3}\r\n" +
						"Stack: \r\n{4}" +
						"\r\n\r\n",

						DateTime.Now.ToString("F", CultureInfo.CreateSpecificCulture("en-US")),
						new string('-', 150),
						e.GetType(),
						e.Message,
						e.StackTrace
					)
				);
			} catch (IOException) { }
		}

		public static void FatalException(Exception e) {
			try {
				if (IsAllowed == null || !IsAllowed.Invoke(null))
					return;

				if (!string.IsNullOrEmpty(LoggingPath) && !Directory.Exists(LoggingPath))
					Directory.CreateDirectory(LoggingPath);

				File.AppendAllText(
					Path.Combine(LoggingPath, FATAL_EXCEPTIONS_FILE_NAME),
					string.Format(
						"-----[{0}] {1}\r\n" +
						"Exception: {2}\r\n" +
						"Type: Fatal\r\n" +
						"Message: {3}\r\n" +
						"Stack: \r\n{4}" +
						"\r\n\r\n",

						DateTime.Now.ToString("F", CultureInfo.CreateSpecificCulture("en-US")),
						new string('-', 150),
						e.GetType(),
						e.Message,
						e.StackTrace
					)
				);
			} catch (IOException) { }
		}

		public static void Request(Request request, bool sent) {
			try {
				if (IsAllowed == null || !IsAllowed.Invoke(null))
					return;

				if (!string.IsNullOrEmpty(LoggingPath) && !Directory.Exists(LoggingPath))
					Directory.CreateDirectory(LoggingPath);

				if (request.Method == Networking.Request.Type.Ping)
					return;

				File.AppendAllText(
					Path.Combine(LoggingPath, NETWORK_LOG_FILE_NAME),
					string.Format(
						"-----[{0}] {1}\r\n" +
						"{2}\r\n" +
						"Method: {3}\r\n" +
						"\r\n\r\n",

						DateTime.Now.ToString("F", CultureInfo.CreateSpecificCulture("en-US")),
						new string('-', 150),
						"Request " + (sent ? "sent" : "received"),
						request.Method.ToString()
					)
				);
			} catch (IOException) { }
		}

		public static void Response(Response response, bool sent) {
			try {
				if (IsAllowed == null || !IsAllowed.Invoke(null))
					return;

				if (!string.IsNullOrEmpty(LoggingPath) && !Directory.Exists(LoggingPath))
					Directory.CreateDirectory(LoggingPath);

				if (response.Method == Networking.Response.Type.Ping)
					return;

				File.AppendAllText(
					Path.Combine(LoggingPath, NETWORK_LOG_FILE_NAME),
					string.Format(
						"-----[{0}] {1}\r\n" +
						"{2}\r\n" +
						"Method: {3}\r\n" +
						"\r\n\r\n",

						DateTime.Now.ToString("F", CultureInfo.CreateSpecificCulture("en-US")),
						new string('-', 150),
						"Response " + (sent ? "sent" : "received"),
						response.Method.ToString()
					)
				);
			} catch (IOException) { }
		}

		public static void Message(Message message, bool sent) {
			try {
				if (IsAllowed == null || !IsAllowed.Invoke(null))
				return;

				if (!string.IsNullOrEmpty(LoggingPath) && !Directory.Exists(LoggingPath))
					Directory.CreateDirectory(LoggingPath);

				File.AppendAllText(
					Path.Combine(LoggingPath, NETWORK_LOG_FILE_NAME),
					string.Format(
						"-----[{0}] {1}\r\n" +
						"{2}\r\n" +
						"Method: {3}\r\n" +
						"\r\n\r\n",

						DateTime.Now.ToString("F", CultureInfo.CreateSpecificCulture("en-US")),
						new string('-', 150),
						"Message " + (sent ? "sent" : "received"),
						message.Method.ToString()
					)
				);
			} catch (IOException) { }
		}
	}
}