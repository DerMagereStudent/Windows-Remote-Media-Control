using System.IO;

using Newtonsoft.Json;

using WRMC.Core.Networking;
using WRMC.Windows.Media;

namespace WRMC.Windows {
	public class Settings {
		private const string FILE_PATH = "settings.json";

		public static Settings Current { get; set; }

		public FormCloseAction CloseAction { get; set; }
		public MediaSessionExtractor SessionExtractor { get; set; }

		public Settings() {
			this.CloseAction = FormCloseAction.Minimize;
			this.SessionExtractor = new TransportControlsMediaSessionExtractor();
		}

		public static void Save() {
			File.WriteAllText(FILE_PATH, JsonConvert.SerializeObject(Settings.Current, SerializationOptions.DefaultSerializationOptions));
		}

		public static void Load() {
			if (!File.Exists(FILE_PATH)) {
				Settings.Current = new Settings();
				return;
			}

			Settings.Current = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FILE_PATH), SerializationOptions.DefaultSerializationOptions);
		}
	}
}