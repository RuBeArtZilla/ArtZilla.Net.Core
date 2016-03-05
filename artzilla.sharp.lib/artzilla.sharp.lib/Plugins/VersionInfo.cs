using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArtZilla.Sharp.Lib.Plugins {
	[Serializable]
	public class VersionInfo {
		public const char Delimeter = '.';

		public ushort Major { get; set; } = 0;
		public ushort Minor { get; set; } = 0;
		public ushort? Build { get; set; } = null;
		public ushort? Revision { get; set; } = null;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
		/// </summary>
		public VersionInfo() { }

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
		/// </summary>
		public VersionInfo(ushort major, ushort minor, ushort? build = null, ushort? revision = null) {
			Major = major;
			Minor = minor;
			Build = build;
			Revision = revision;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
		/// </summary>
		public VersionInfo(FileVersionInfo fvi) {
			Major = (ushort) fvi.FileMajorPart;
			Minor = (ushort) fvi.FileMinorPart;
			Build = (ushort) fvi.FileBuildPart;
			Revision = (ushort) fvi.FilePrivatePart;
		}

		/// <summary>
		/// Возвращает объект <see cref="T:System.String"/>, который представляет текущий объект <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// Объект <see cref="T:System.String"/>, представляющий текущий объект <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString() {
			var str = Major.ToString() + Delimeter + Minor.ToString();

			if (Build.HasValue)
				str += Delimeter + Build.Value.ToString();

			if (Revision.HasValue)
				str += Delimeter + Revision.Value.ToString();

			return str;
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