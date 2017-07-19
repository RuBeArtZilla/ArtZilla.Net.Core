using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ArtZilla.Net.Core.Plugins {
	[Serializable]
	public class VersionInfo {
		public const string Delimeter = ".";

		public ushort Major { get; set; } = 0;

		public ushort Minor { get; set; } = 0;

		public ushort? Build { get; set; } = null;

		public ushort? Revision { get; set; } = null;

		public VersionInfo() { }

		public VersionInfo(ushort major, ushort minor, ushort? build = null, ushort? revision = null) {
			Major = major;
			Minor = minor;
			Build = build;
			Revision = revision;
		}

		public VersionInfo(FileVersionInfo fvi) {
			Major = (ushort) fvi.FileMajorPart;
			Minor = (ushort) fvi.FileMinorPart;
			Build = (ushort) fvi.FileBuildPart;
			Revision = (ushort) fvi.FilePrivatePart;
		}

		public override string ToString() {
			var str = new StringBuilder(Major + Delimeter + Minor);

			if (Build.HasValue)
				str.Append(Delimeter + Build.Value);

			if (Revision.HasValue)
				str.Append(Delimeter + Revision.Value);

			return str.ToString();
		}

		public ulong ToUlong()
			=> BitConverter.ToUInt64(
				BitConverter.GetBytes(Major)
				            .Concat(BitConverter.GetBytes(Minor))
				            .Concat(Build.HasValue ? BitConverter.GetBytes(Build.Value) : new byte[2] {0, 0})
				            .Concat(Revision.HasValue ? BitConverter.GetBytes(Revision.Value) : new byte[2] {0, 0})
				            .ToArray(), 0);

		public static bool operator >(VersionInfo c1, VersionInfo c2) => c1.ToUlong() > c2.ToUlong();

		public static bool operator >=(VersionInfo c1, VersionInfo c2) => c1.ToUlong() >= c2.ToUlong();

		public static bool operator <(VersionInfo c1, VersionInfo c2) => c1.ToUlong() < c2.ToUlong();

		public static bool operator <=(VersionInfo c1, VersionInfo c2) => c1.ToUlong() <= c2.ToUlong();
	}
}