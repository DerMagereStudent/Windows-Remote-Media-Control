using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WRMC.Core;
using WRMC.Core.Models;

namespace WRMC.Windows.Media {
	public abstract class MediaSessionExtractor {
		public static MediaSessionExtractor Default { get; set; }
		public abstract List<MediaSession> Sessions { get; }

		public abstract event TypedEventHandler<MediaSessionExtractor, EventArgs> OnSessionsChanged;
		public abstract event TypedEventHandler<MediaSessionExtractor, EventArgs<MediaSession>> OnSessionChanged;

		public abstract void Initialise();
	}
}