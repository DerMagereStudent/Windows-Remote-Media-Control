﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WRMC.Core;
using WRMC.Core.Models;

namespace WRMC.Windows.Media {
	public abstract class MediaSessionExtractor {
		public static MediaSessionExtractor Default { get; set; }
		public SynchronizedCollection<MediaSession> Sessions { get; protected set; }

		public abstract event TypedEventHandler<MediaSessionExtractor, EventArgs> OnSessionsChanged;

		public abstract void Initialise();
	}
}