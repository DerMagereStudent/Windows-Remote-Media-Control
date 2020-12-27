using System;
using System.Collections.Generic;
using System.Text;

namespace WRMC.Core.Models {
	/// <summary>
	/// Represents a process which was suspended by the OS.
	/// </summary>
	public class SuspendedProcess {
		/// <summary>
		/// The id of the process.
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// The name of the process.
		/// </summary>
		public string Name { get; set; }
	}
}