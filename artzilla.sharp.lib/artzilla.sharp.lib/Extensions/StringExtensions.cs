using System;
using System.Globalization;
using System.Text;

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

		/// <summary>
		/// <TODO>add description, that method return combined strings with delimeter, or any not bad string, or empty string.</TODO>
		/// </summary>
		/// <param name="delimeter"></param>
		/// <param name="item1"></param>
		/// <param name="item2"></param>
		/// <returns></returns>
		public static string Combine(this string item1, string delimeter, string item2) {
			var isGood1 = item1.IsGood();
			var isGood2 = item2.IsGood();

			if (isGood1 && isGood2)
				return item1 + delimeter + item2;
			if (isGood1)
				return item1;
			if (isGood2)
				return item2;
			return string.Empty;
		}

		/// <summary>
		/// <TODO>add description, that method return combined strings with delimeter, or any not bad string, or empty string.</TODO>
		/// </summary>
		/// <param name="delimeter"></param>
		/// <param name="items"></param>
		/// <returns></returns>
		public static string Combine(this string firstItem, string delimeter, params string[] items) {
			if (items is null)
				throw new ArgumentNullException(nameof(items));

			var count = items.Length;
			if (count == 0) {
				if (firstItem.IsGood())
					return firstItem;
				return string.Empty;
			}

			{
				// for 1 item using fast check;
				if (count == 1) {
					var item = items[0];
					if (item is null)
						return firstItem;
					return firstItem + delimeter + item;
				}

				{
					var b = new StringBuilder(firstItem);
					for (var i = 0; i < count; i++) {
						var item = items[i];
						if (item.IsBad())
							continue;

						if (b.Length > 0)
							b.Append(delimeter);
						b.Append(item);
					}
					return b.ToString();
				}
			}
		}

		public static string EnframeGood(this string whatToEnframe, string prefix = "", string postfix = "")
			=> whatToEnframe.IsBad() ? whatToEnframe : prefix + whatToEnframe + postfix;
	}
}