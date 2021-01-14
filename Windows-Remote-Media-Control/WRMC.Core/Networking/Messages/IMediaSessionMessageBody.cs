using System;
using System.Collections.Generic;
using System.Text;

using WRMC.Core.Models;

namespace WRMC.Core.Networking {
	public interface IMediaSessionMessageBody {
		MediaSession MediaSession { get; set; }
	}
}