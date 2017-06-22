using System;
using System.Linq;

namespace ArtZilla.Net.Core.Experimental {
	public struct VersionInfo {
		public const String Delimeter = ".";

		public UInt16 Major { get; } 

		public UInt16 Minor { get; } 

		public UInt16 Build { get; }

		public UInt16 Revision { get; }

		public VersionInfo(UInt64 value) : this() {
			var b = BitConverter.GetBytes(value);
			Major = BitConverter.ToUInt16(b, 0);
			Minor = BitConverter.ToUInt16(b, 2);
			Build = BitConverter.ToUInt16(b, 4);
			Revision = BitConverter.ToUInt16(b, 6);
		}

		public VersionInfo(String value) : this() {
			if (value == null) 
				throw new ArgumentNullException();

			var v = value.Split(new String[] {Delimeter}, StringSplitOptions.RemoveEmptyEntries);
			if (v.Length != 4)
				throw new ArgumentException("Argument value is not valid");

			Major = UInt16.Parse(v[0]);
			Minor = UInt16.Parse(v[1]);
			Build = UInt16.Parse(v[2]);
			Revision = UInt16.Parse(v[3]);
		}

		public VersionInfo(UInt16 major = 1, UInt16 minor = 0, UInt16 build = 0, UInt16 revision = 0) {
			Major = major;
			Minor = minor;
			Build = build;
			Revision = revision;
		}


		public UInt64 Value()
			=> BitConverter.ToUInt64(
				BitConverter
					.GetBytes(Major)
					.Concat(BitConverter.GetBytes(Minor))
					.Concat(BitConverter.GetBytes(Build))
					.Concat(BitConverter.GetBytes(Revision))
					.ToArray(), 0);

		public override String ToString() => 
			Major + Delimeter + Minor + Delimeter + Build + Delimeter + Revision;

		public static Boolean operator ==(VersionInfo c1, VersionInfo c2) => c1.Value() == c2.Value();
		public static Boolean operator !=(VersionInfo c1, VersionInfo c2) => c1.Value() != c2.Value();
		public static Boolean operator >(VersionInfo c1, VersionInfo c2) => c1.Value() > c2.Value();
		public static Boolean operator >=(VersionInfo c1, VersionInfo c2) => c1.Value() >= c2.Value();
		public static Boolean operator <(VersionInfo c1, VersionInfo c2) => c1.Value() < c2.Value();
		public static Boolean operator <=(VersionInfo c1, VersionInfo c2) => c1.Value() <= c2.Value();
	}
}