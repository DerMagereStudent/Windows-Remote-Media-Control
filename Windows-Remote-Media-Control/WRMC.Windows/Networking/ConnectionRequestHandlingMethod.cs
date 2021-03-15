using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRMC.Windows.Networking {
	public enum ConnectionRequestHandlingMethod {
		AcceptAll,
		AcceptKnown,
		Ask,
		RejectAll
	}
}