using System;
using System.Globalization;

namespace ArtZilla.Net.Core.Extensions;

public static class NumberUtils {
	/// <summary>
	/// todo: add description
	/// method from https://stackoverflow.com/a/33325313
	/// </summary>
	/// <param name="high"></param>
	/// <param name="low"></param>
	/// <returns></returns>
	public static long MakeLong(int high, int low)
		=> (long) high << 32 | (long) (uint) low;

	/// <summary>
	/// todo: add description
	/// </summary>
	/// <param name="high"></param>
	/// <param name="low"></param>
	/// <returns></returns>
	public static long MakeLong(int high, uint low)
		=> (long) high << 32 | low;

	/// <summary>
	/// todo: add description
	/// </summary>
	/// <param name="high"></param>
	/// <param name="low"></param>
	/// <returns></returns>
	public static long MakeLong(uint high, uint low)
		=> (long) high << 32 | low;

	public const long BitsInByte = 8;
	public const long BytesInKilobyte = 1024;
	public const long BytesInMegabyte = 1048576;
	public const long BytesInGigabyte = 1073741824;
	public const long BytesInTerabyte = 1099511627776;

	/// <summary> Usual conversation with 1024 base and invariant culture </summary>
	/// <param name="bytes"></param>
	/// <returns></returns>
	public static string HumanReadableByteCount(this ulong bytes)
		=> bytes < 1024L ? bytes + " B"
		: bytes < 0xfffccccccccccccL >> 40 ? (bytes / (double) BytesInKilobyte).ToString("F1", CultureInfo.InvariantCulture) + " KB"
		: bytes < 0xfffccccccccccccL >> 30 ? (bytes / (double) BytesInMegabyte).ToString("F1", CultureInfo.InvariantCulture) + " MB"
		: bytes < 0xfffccccccccccccL >> 20 ? (bytes / (double) BytesInGigabyte).ToString("F1", CultureInfo.InvariantCulture) + " GB"
		: bytes < 0xfffccccccccccccL >> 10 ? (bytes / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " TB"
		: bytes < 0xfffccccccccccccL ? ((bytes >> 10) / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " PB"
		: ((bytes >> 20) / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " EB";

	/// <summary> Binary conversation with 1024 base and invariant culture </summary>
	/// <param name="bytes"></param>
	/// <returns></returns>
	public static string HumanReadableByteCountBin(this ulong bytes)
		=> bytes < 1024L ? bytes + " B"
			: bytes < 0xfffccccccccccccL >> 40 ? (bytes / (double) BytesInKilobyte).ToString("F1", CultureInfo.InvariantCulture) + " KiB"
			: bytes < 0xfffccccccccccccL >> 30 ? (bytes / (double) BytesInMegabyte).ToString("F1", CultureInfo.InvariantCulture) + " MiB"
			: bytes < 0xfffccccccccccccL >> 20 ? (bytes / (double) BytesInGigabyte).ToString("F1", CultureInfo.InvariantCulture) + " GiB"
			: bytes < 0xfffccccccccccccL >> 10 ? (bytes / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " TiB"
			: bytes < 0xfffccccccccccccL ? ((bytes >> 10) / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " PiB"
			: ((bytes >> 20) / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " EiB";

	/// <summary> SI conversation with 1000 base and invariant culture </summary>
	/// <param name="bytes"></param>
	/// <returns></returns>
	public static string HumanReadableByteCountSI(this ulong bytes)
		=> bytes < 1000L ? bytes + " B"
			: bytes < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " kB"
			: (bytes /= 1000) < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " MB"
			: (bytes /= 1000) < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " GB"
			: (bytes /= 1000) < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " TB"
			: (bytes /= 1000) < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " PB"
			: (bytes / 1e6).ToString("F1", CultureInfo.InvariantCulture) + " EB";
}
