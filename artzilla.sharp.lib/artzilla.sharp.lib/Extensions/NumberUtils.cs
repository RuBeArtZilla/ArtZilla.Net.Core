using System.Globalization;

namespace ArtZilla.Net.Core.Extensions;

/// 
public static class NumberUtils {
	/// todo: add description
	/// method from https://stackoverflow.com/a/33325313
	/// <param name="high"></param>
	/// <param name="low"></param>
	/// <returns></returns>
	public static long MakeLong(int high, int low)
		=> (long) high << 32 | (long) (uint) low;

	/// todo: add description
	/// <param name="high"></param>
	/// <param name="low"></param>
	/// <returns></returns>
	public static long MakeLong(int high, uint low)
		=> (long) high << 32 | low;

	/// todo: add description
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

	/// Usual conversation with 1024 base and invariant culture
	/// <param name="bytes"></param>
	public static string HumanReadableByteCount(this ulong bytes)
		=> bytes < 1024L ? bytes + " B"
		: bytes < 0xfffccccccccccccL >> 40 ? (bytes / (double) BytesInKilobyte).ToString("F1", CultureInfo.InvariantCulture) + " KB"
		: bytes < 0xfffccccccccccccL >> 30 ? (bytes / (double) BytesInMegabyte).ToString("F1", CultureInfo.InvariantCulture) + " MB"
		: bytes < 0xfffccccccccccccL >> 20 ? (bytes / (double) BytesInGigabyte).ToString("F1", CultureInfo.InvariantCulture) + " GB"
		: bytes < 0xfffccccccccccccL >> 10 ? (bytes / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " TB"
		: bytes < 0xfffccccccccccccL ? ((bytes >> 10) / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " PB"
		: ((bytes >> 20) / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " EB";

	/// Binary conversation with 1024 base and invariant culture
	/// <param name="bytes"></param>
	public static string HumanReadableByteCountBin(this ulong bytes)
		=> bytes < 1024L ? bytes + " B"
			: bytes < 0xfffccccccccccccL >> 40 ? (bytes / (double) BytesInKilobyte).ToString("F1", CultureInfo.InvariantCulture) + " KiB"
			: bytes < 0xfffccccccccccccL >> 30 ? (bytes / (double) BytesInMegabyte).ToString("F1", CultureInfo.InvariantCulture) + " MiB"
			: bytes < 0xfffccccccccccccL >> 20 ? (bytes / (double) BytesInGigabyte).ToString("F1", CultureInfo.InvariantCulture) + " GiB"
			: bytes < 0xfffccccccccccccL >> 10 ? (bytes / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " TiB"
			: bytes < 0xfffccccccccccccL ? ((bytes >> 10) / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " PiB"
			: ((bytes >> 20) / (double) BytesInTerabyte).ToString("F1", CultureInfo.InvariantCulture) + " EiB";

	/// SI conversation with 1000 base and invariant culture
	/// <param name="bytes"></param>
	public static string HumanReadableByteCountSI(this ulong bytes)
		=> bytes < 1000L ? bytes + " B"
			: bytes < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " kB"
			: (bytes /= 1000) < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " MB"
			: (bytes /= 1000) < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " GB"
			: (bytes /= 1000) < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " TB"
			: (bytes /= 1000) < 999_950L ? (bytes / 1e3).ToString("F1", CultureInfo.InvariantCulture) + " PB"
			: (bytes / 1e6).ToString("F1", CultureInfo.InvariantCulture) + " EB";
}
