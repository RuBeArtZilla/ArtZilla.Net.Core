using System;
using System.Globalization;

namespace ArtZilla.Net.Core.Extensions {
	public static class StringExtensions {
		/// <summary> Wrapper for <see cref="string.IsNullOrWhiteSpace"/> </summary>
		public static bool IsBad(this string value)
			=> string.IsNullOrWhiteSpace(value);

		/// <summary> Wrapper for is not <see cref="string.IsNullOrWhiteSpace"/> </summary>
		public static bool IsGood(this string value)
			=> !string.IsNullOrWhiteSpace(value);

		/// <summary> Wrapper for <see cref="string.Equals(string, System.StringComparison)"/> with OrdinalIgnoreCase </summary>
		public static bool Like(this string left, string right)
			=> left?.Equals(right, StringComparison.OrdinalIgnoreCase) ?? false;

		/// <summary>
		///	Converts the string representation of a number to its 32-bit signed integer equivalent, or default value
		/// </summary>
		/// <param name="source">A string containing a number to convert.</param>
		/// <param name="defValue">Default value, returned if conversion failed. Default value <see cref="Int32.MinValue"/></param>
		/// <returns> When this method returns, result is the 32-bit signed integer value equivalent to the number contained in s, or defValue if can't parse number </returns>
		public static int ParseIntEx(this string source, int defValue = Int32.MinValue) {
			if (source.IsBad())
				return defValue;
			return Int32.TryParse(source, out var val) ? val : defValue;
		}

		public static double ParseDoubleEx(this string source, double defValue = Double.NaN) {
			if (source.IsBad())
				return defValue;
			return Double.TryParse(source, out var val) ? val : defValue;
		}

		public static double ParseDoubleEx(this string source, NumberStyles ns, IFormatProvider ifp, double defValue = Double.NaN) {
			if (source.IsBad())
				return defValue;
			return Double.TryParse(source, ns, ifp, out var val) ? val : defValue;
		}

		public static bool ParseBoolEx(this string source, bool defValue = false) {
			if (source.IsBad())
				return defValue;
			return Boolean.TryParse(source, out var val) ? val : defValue;
		}
	}
}