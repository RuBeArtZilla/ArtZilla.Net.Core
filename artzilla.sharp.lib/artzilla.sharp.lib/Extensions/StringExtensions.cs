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

		/// <summary> Wrapper for <see cref="string.IsNullOrEmpty"/> </summary>
		public static bool IsNullOrEmpty(this string value)
			=> string.IsNullOrEmpty(value);

		/// <summary> Negation of <see cref="string.IsNullOrWhiteSpace"/> </summary>
		public static bool IsNotNullOrEmpty(this string value)
			=> !string.IsNullOrEmpty(value);

		/// <summary> Wrapper for <see cref="string.IsNullOrWhiteSpace"/> </summary>
		public static bool IsNullOrWhiteSpace(this string value)
			=> string.IsNullOrWhiteSpace(value);

		/// <summary> Negation of <see cref="string.IsNullOrWhiteSpace"/> </summary>
		public static bool IsNotNullOrWhiteSpace(this string value)
			=> !string.IsNullOrWhiteSpace(value);

		/// <summary>
		/// Wrapper for <see cref="string.Equals(string, System.StringComparison)"/> with OrdinalIgnoreCase.
		/// Null values always returns false.
		/// </summary>
		/// <returns> True if left string equals other string (IgnoreCase), otherwise false. </returns>
		public static bool Like(this string left, string right)
			=> left?.Equals(right, StringComparison.OrdinalIgnoreCase) ?? false;

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

		/// <summary>
		/// Add <paramref name="prefix"/> and <paramref name="postfix"/> if <paramref name="whatToEnframe"/> is <see cref="IsGood(string)"/> otherwise returns empty string.
		/// </summary>
		/// <param name="whatToEnframe">the source string</param>
		/// <param name="prefix">prefix string, can be empty</param>
		/// <param name="postfix">postfix string, can be empty</param>
		/// <returns></returns>
		public static string EnframeGood(this string whatToEnframe, string prefix = "", string postfix = "")
			=> whatToEnframe.IsBad() ? "" : prefix + whatToEnframe + postfix;

		#region Parse methods

		/// <summary>
		///	Converts the string representation of a number to its 32-bit signed integer equivalent, or default value
		/// </summary>
		/// <param name="source">A string containing a number to convert.</param>
		/// <param name="defValue">Default value, returned if conversion failed. Default value <see cref="Int32.MinValue"/></param>
		/// <returns> When this method returns, result is the 32-bit signed integer value equivalent to the number contained in s, or defValue if can't parse number </returns>
		public static int ParseIntEx(this string source, int defValue = int.MinValue) {
			if (source.IsBad())
				return defValue;
			return int.TryParse(source, out var val) ? val : defValue;
		}

		public static double ParseDoubleEx(this string source, double defValue = double.NaN) {
			if (source.IsBad())
				return defValue;
			return double.TryParse(source, out var val) ? val : defValue;
		}

		public static double ParseDoubleEx(this string source, NumberStyles ns, IFormatProvider ifp, double defValue = double.NaN) {
			if (source.IsBad())
				return defValue;
			return double.TryParse(source, ns, ifp, out var val) ? val : defValue;
		}

		public static bool ParseBoolEx(this string source, bool defValue = false) {
			if (source.IsBad())
				return defValue;
			return bool.TryParse(source, out var val) ? val : defValue;
		}
		
		#endregion

		#region Extract methods

		public static string Extract(this string input, out string remainder, string op, string ed, StringComparison comparison = StringComparison.Ordinal) {
			Guard.NotNullOrEmpty(op, nameof(op));
			Guard.NotNullOrEmpty(ed, nameof(ed));

			remainder = input;
			if (input is null)
				return default;

			if (input.Length < op.Length + ed.Length)
				return default;

			var nop = input.IndexOf(op, comparison);
			if (nop < 0)
				return default;

			var ned = input.IndexOf(ed, nop + op.Length, comparison);
			if (ned < 0)
				return default;

			remainder = input.Remove(nop, ned - nop + ed.Length);

			nop += op.Length;
			return input.Substring(nop, ned - nop);
		}

		public static string Extract(this string input, out string remainder, string border, StringComparison comparison = StringComparison.Ordinal) {
			Guard.NotNullOrEmpty(border, nameof(border));

			remainder = input;
			if (input is null)
				return default;

			if (input.Length < border.Length << 1)
				return default;

			var nop = input.IndexOf(border, comparison);
			if (nop < 0)
				return default;

			nop += border.Length;

			var ned = input.IndexOf(border, nop, comparison);
			if (ned < 0)
				return default;

			remainder = input.Remove(nop - border.Length, ned + (border.Length << 1));
			return input.Substring(nop, ned - nop);
		}

		public static string Extract(this string input, string op, string ed, StringComparison comparison = StringComparison.Ordinal) {
			Guard.NotNullOrEmpty(op, nameof(op));
			Guard.NotNullOrEmpty(ed, nameof(ed));

			if (input is null)
				return default;

			if (input.Length < op.Length + ed.Length)
				return default;

			var nop = input.IndexOf(op, comparison);
			if (nop < 0)
				return default;

			nop += op.Length;
			var ned = input.IndexOf(ed, nop, comparison);
			if (ned < 0)
				return default;

			return input.Substring(nop, ned - nop);
		}

		public static string Extract(this string input, string border, StringComparison comparison = StringComparison.Ordinal) {
			Guard.NotNullOrEmpty(border, nameof(border));

			if (input is null)
				return default;

			if (input.Length < border.Length << 1)
				return default;

			var nop = input.IndexOf(border, comparison);
			if (nop < 0)
				return default;

			nop += border.Length;
			var ned = input.IndexOf(border, nop, comparison);
			if (ned < 0)
				return default;

			return input.Substring(nop, ned - nop);
		}

		#endregion

		#region ExtractLast methods

		public static string ExtractLast(this string input, out string remainder, string op, string ed, StringComparison comparison = StringComparison.Ordinal) {
			Guard.NotNullOrEmpty(op, nameof(op));
			Guard.NotNullOrEmpty(ed, nameof(ed));

			remainder = input;
			if (input is null)
				return default;

			if (input.Length < op.Length + ed.Length)
				return default;

			var ned = input.LastIndexOf(ed, comparison);
			if (ned < op.Length)
				return default;

			var nop = input.LastIndexOf(op, ned - 1, comparison);
			if (nop < 0)
				return default;

			remainder = input.Remove(nop, ned - nop + ed.Length);
			nop += op.Length;

			return input.Substring(nop, ned - nop);
		}

		public static string ExtractLast(this string input, out string remainder, string border, StringComparison comparison = StringComparison.Ordinal) {
			Guard.NotNullOrEmpty(border, nameof(border));

			remainder = input;
			if (input is null)
				return default;

			if (input.Length < border.Length << 1)
				return default;

			var ned = input.LastIndexOf(border, comparison);
			if (ned < border.Length)
				return default;

			var nop = input.LastIndexOf(border, ned - 1, comparison);
			if (nop < 0)
				return default;

			remainder = input.Remove(nop, ned - nop + border.Length);
			nop += border.Length;

			return input.Substring(nop, ned - nop);
		}

		public static string ExtractLast(this string input, string op, string ed, StringComparison comparison = StringComparison.Ordinal) {
			Guard.NotNullOrEmpty(op, nameof(op));
			Guard.NotNullOrEmpty(ed, nameof(ed));

			if (input is null)
				return default;

			if (input.Length < op.Length + ed.Length)
				return default;

			var ned = input.LastIndexOf(ed, comparison);
			if (ned < op.Length)
				return default;

			var nop = input.LastIndexOf(op, ned - 1, comparison);
			if (nop < 0)
				return default;

			nop += op.Length;

			return input.Substring(nop, ned - nop);
		}

		public static string ExtractLast(this string input, string border, StringComparison comparison = StringComparison.Ordinal) {
			Guard.NotNullOrEmpty(border, nameof(border));

			if (input is null)
				return default;

			if (input.Length < border.Length << 1)
				return default;

			var ned = input.LastIndexOf(border, comparison);
			if (ned < border.Length)
				return default;

			var nop = input.LastIndexOf(border, ned - 1, comparison);
			if (nop < 0)
				return default;

			nop += border.Length;
			return input.Substring(nop, ned - nop);
		}

		#endregion
	}
}