using System;
using System.Collections.Generic;
using System.Text;

namespace WRMC.Core.Models {
	public class DirectoryItem {
		public string Path { get; set; }
		public string Name { get; set; }
		public bool IsFolder { get; set; }
		public byte[] Icon { get; set; }
	}
}