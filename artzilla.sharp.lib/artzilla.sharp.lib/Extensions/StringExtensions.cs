using System;
using System.Globalization;

namespace ArtZilla.Net.Core.Extensions {
	public static class StringExtensions {
		/// <summary> Wrapper for <see cref="string.IsNullOrWhiteSpace"/> </summary>
		public static Boolean IsBad(this String str)
			=> String.IsNullOrWhiteSpace(str);

		/// <summary> Wrapper for is not <see cref="string.IsNullOrWhiteSpace"/> </summary>
		public static Boolean IsGood(this String str)
			=> !String.IsNullOrWhiteSpace(str);

		/// <summary> Wrapper for <see cref="string.Equals(string, System.StringComparison)"/> with OrdinalIgnoreCase </summary>
		public static Boolean Like(this String s1, String s2) 
			=> s1?.Equals(s2, StringComparison.OrdinalIgnoreCase) ?? false;

		/// <summary>
		///		Converts the string representation of a number to its 32-bit signed integer equivalent, or default value
		/// </summary>
		/// <param name="s">A string containing a number to convert.</param>
		/// <param name="defValue">Default value, returned if conversion failed. Default value <see cref="Int32.MinValue"/></param>
		/// <returns> When this method returns, result is the 32-bit signed integer value equivalent to the number contained in s, or defValue if can't parse number </returns>
		public static Int32 ParseIntEx(this String s, Int32 defValue = Int32.MinValue) {
			Int32 val;
			if (s.IsBad()) return defValue;
			return Int32.TryParse(s, out val) ? val : defValue;
		}

		public static Double ParseDoubleEx(this String s, Double defValue = Double.NaN) {
			Double val;
			if (s.IsBad()) return defValue;
			
			return Double.TryParse(s, out val) ? val : defValue;
		}

		public static Double ParseDoubleEx(this String s, NumberStyles ns, IFormatProvider ifp, Double defValue = Double.NaN) {
			Double val;
			if (s.IsBad()) return defValue;

			return Double.TryParse(s, ns, ifp, out val) ? val : defValue;
		}

		public static Boolean ParseBoolEx(this String s, Boolean defValue = false) {
			Boolean val;
			if (s.IsBad()) return defValue;
			return Boolean.TryParse(s, out val) ? val : defValue;
		}
	}
}