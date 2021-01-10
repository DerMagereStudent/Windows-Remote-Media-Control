using System;
using System.Runtime.InteropServices;

namespace WRMC.Windows.Native {
	public static class NativeStructs {
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct PROPARRAY {
            public uint cElems;
            public IntPtr pElems;
        }

        public struct PROPERTYKEY {
			Guid fmtid;
			uint pid;

			public PROPERTYKEY(Guid InputId, UInt32 InputPid) {
				this.fmtid = InputId;
				this.pid = InputPid;
			}
		};

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct PROPVARIANT {
            [FieldOffset(0)]
            public ushort varType;
            [FieldOffset(2)]
            public ushort wReserved1;
            [FieldOffset(4)]
            public ushort wReserved2;
            [FieldOffset(6)]
            public ushort wReserved3;

            [FieldOffset(8)]
            public byte bVal;
            [FieldOffset(8)]
            public sbyte cVal;
            [FieldOffset(8)]
            public ushort uiVal;
            [FieldOffset(8)]
            public short iVal;
            [FieldOffset(8)]
            public UInt32 uintVal;
            [FieldOffset(8)]
            public Int32 intVal;
            [FieldOffset(8)]
            public UInt64 ulVal;
            [FieldOffset(8)]
            public Int64 lVal;
            [FieldOffset(8)]
            public float fltVal;
            [FieldOffset(8)]
            public double dblVal;
            [FieldOffset(8)]
            public short boolVal;
            [FieldOffset(8)]
            public IntPtr pclsidVal;
            [FieldOffset(8)]
            public IntPtr pszVal;
            [FieldOffset(8)]
            public IntPtr pwszVal;
            [FieldOffset(8)]
            public IntPtr punkVal;
            [FieldOffset(8)]
            public PROPARRAY ca;
            [FieldOffset(8)]
            public System.Runtime.InteropServices.ComTypes.FILETIME filetime;
        }

        [StructLayout(LayoutKind.Sequential)]
		public struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
	}
}