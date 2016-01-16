using System;

namespace artzilla.sharp.lib.Extenstions {
	public static class StringExtensions {
		/// <summary>
		/// Wrapper for <see cref="string.IsNullOrWhiteSpace"/>
		/// </summary>
		public static bool IsBad(this string str) => String.IsNullOrWhiteSpace(str);

		/// <summary>
		/// Wrapper for <see cref="string.Equals(string, System.StringComparison)"/> with InvariantCultureIgnoreCase
		/// </summary>
		public static bool Like(this string s1, string s2) => s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase);

		public static int ParseIntEx(this string s, int defValue = int.MinValue) {
			int val;
			if (s.IsBad()) return defValue;
			return int.TryParse(s, out val) ? val : defValue;
		}

		public static double ParseDoubleEx(this string s, double defValue = double.NaN) {
			double val;
			if (s.IsBad()) return defValue;
			return double.TryParse(s, out val) ? val : defValue;
		}

		public static bool ParseBoolEx(this string s, bool defValue = false) {
			bool val;
			if (s.IsBad()) return defValue;
			return bool.TryParse(s, out val) ? val : defValue;
		}
	}
}