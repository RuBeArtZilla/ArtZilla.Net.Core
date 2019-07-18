using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ArtZilla.Net.Core.Extensions {
	/// <summary> Some useless string extensions </summary>
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

		/// <summary> Parse source string to dictionary </summary>
		/// <param name="source"></param>
		/// <param name="pairSeparator">separator between key and value</param>
		/// <param name="listSeparator">separator between pairs</param>
		/// <returns></returns>
		public static Dictionary<string, string> ToDictionary(
			this string source,
			string pairSeparator = "=",
			string listSeparator = ";")
			=> source?.Split(new[] { listSeparator }, StringSplitOptions.RemoveEmptyEntries)
				.Select(i => i.Split(new[] { pairSeparator }, 2, StringSplitOptions.None))
				.Where(i => i.Length == 2 && i[0].Length > 0)
				.GroupBy(i => i[0], i => i[1], StringComparer.OrdinalIgnoreCase)
				.ToDictionary(i => i.Key, i => i.First().Trim(), StringComparer.OrdinalIgnoreCase);
		
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

		#region Enframe methods

		/// <summary>
		/// Add <paramref name="prefix"/> and <paramref name="postfix"/> to <paramref name="source"/>
		/// if statement is true otherwise returns empty string.
		/// </summary>
		/// <param name="source">the source string</param>
		/// <param name="condition"></param>
		/// <param name="prefix">prefix string, can be empty</param>
		/// <param name="postfix">postfix string, can be empty</param>
		/// <returns></returns>
		public static string EnframeIf(
			this string source,
			Func<string, bool> condition,
			string prefix = "",
			string postfix = ""
		) {
			Guard.NotNull(condition);

			return condition(source) ? prefix + source + postfix : "";
		}

		/// <summary>
		/// Add <paramref name="prefix"/> and <paramref name="postfix"/> if <paramref name="source"/>
		/// is <see cref="IsGood(string)"/> otherwise returns empty string.
		/// </summary>
		/// <param name="source">the source string</param>
		/// <param name="prefix">prefix string, can be empty</param>
		/// <param name="postfix">postfix string, can be empty</param>
		/// <returns></returns>
		[Obsolete("Renamed to StringExtensions.EnframeText, will be deleted in future release")]
		public static string EnframeGood(this string source, string prefix = "", string postfix = "")
			=> source.IsGood() ? prefix + source + postfix : "";

		/// <summary>
		/// Add <paramref name="prefix"/> and <paramref name="postfix"/> to <paramref name="source"/>
		/// if statement is not <see langword="null" /> otherwise returns empty string.
		/// </summary>
		/// <param name="source">the source string</param>
		/// <param name="prefix">prefix string, can be empty</param>
		/// <param name="postfix">postfix string, can be empty</param>
		/// <returns></returns>
		public static string EnframeNotNull(this string source, string prefix = "", string postfix = "")
			=> source is null ? "" : prefix + source + postfix;

		/// <summary>
		/// Add <paramref name="prefix"/> and <paramref name="postfix"/> to <paramref name="source"/>
		/// if source is not <see langword="null" /> or empty otherwise returns empty string.
		/// </summary>
		/// <param name="source">the source string</param>
		/// <param name="prefix">prefix string, can be empty</param>
		/// <param name="postfix">postfix string, can be empty</param>
		/// <returns></returns>
		public static string EnframeNotEmpty(this string source, string prefix = "", string postfix = "")
			=> string.IsNullOrEmpty(source) ? "" : prefix + source + postfix;

		/// <summary>
		/// Add <paramref name="prefix"/> and <paramref name="postfix"/> to <paramref name="source"/>
		/// if source is not <see langword="null" />, empty, or consists only of white-space characters
		/// otherwise returns empty string.
		/// </summary>
		/// <param name="source">the source string</param>
		/// <param name="prefix">prefix string, can be empty</param>
		/// <param name="postfix">postfix string, can be empty</param>
		/// <returns></returns>
		public static string EnframeText(this string source, string prefix = "", string postfix = "")
			=> string.IsNullOrWhiteSpace(source) ? "" : prefix + source + postfix;

		#endregion

		#region Trim methods

		/// <summary> Method remove prefix if exist </summary>
		/// <param name="source">Source string</param>
		/// <param name="prefix">Prefix string</param>
		/// <param name="comparisonType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">prefix </exception>
		public static string TrimPrefix(this string source, string prefix,
			StringComparison comparisonType = StringComparison.Ordinal)
			=> source?.StartsWith(prefix, comparisonType) == true
				? source.Substring(prefix.Length)
				: source ?? string.Empty;

		/// <summary> Method remove suffix if exist </summary>
		/// <param name="source">Source string</param>
		/// <param name="suffix">Suffix string</param>
		/// <param name="comparisonType"></param>
		/// <returns></returns>
		public static string TrimSuffix(this string source, string suffix,
			StringComparison comparisonType = StringComparison.Ordinal)
			=> source?.EndsWith(suffix, comparisonType) == true
				? source.Remove(source.Length - suffix.Length)
				: source ?? string.Empty;

		#endregion

		#region Parse methods

		/// <summary>
		///	Converts the string representation of a number to its 32-bit signed integer equivalent, or default value
		/// </summary>
		/// <param name="source">A string containing a number to convert.</param>
		/// <param name="defValue">Default value, returned if conversion failed. Default value <see cref="int.MinValue"/></param>
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

		public static double ParseDoubleEx(this string source, NumberStyles ns, IFormatProvider ifp,
			double defValue = double.NaN) {
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

		public static string Extract(
			this string input,
			out string remainder,
			string op,
			string ed,
			StringComparison comparison = StringComparison.Ordinal
		) {
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

		public static string Extract(
			this string input,
			out string remainder,
			string border,
			StringComparison comparison = StringComparison.Ordinal
		) {
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

		public static string Extract(
			this string input,
			string op,
			string ed,
			StringComparison comparison = StringComparison.Ordinal
		) {
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

		public static string Extract(
			this string input,
			string border,
			StringComparison comparison = StringComparison.Ordinal
		) {
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

		public static string ExtractLast(this string input, out string remainder, string op, string ed,
			StringComparison comparison = StringComparison.Ordinal) {
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

		public static string ExtractLast(this string input, out string remainder, string border,
			StringComparison comparison = StringComparison.Ordinal) {
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

		public static string ExtractLast(this string input, string op, string ed,
			StringComparison comparison = StringComparison.Ordinal) {
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

		public static string ExtractLast(this string input, string border,
			StringComparison comparison = StringComparison.Ordinal) {
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

		#region Extract_Word methods

		/// <summary> Extract first word (part of the string before first delimeter). </summary>
		/// <param name="source"></param>
		/// <param name="remainder">Part of source after first delimeter</param>
		/// <param name="delimeter"></param>
		/// <returns></returns>
		public static string ExtractFirstWord(this string source, out string remainder, string delimeter = " ") {
			remainder = string.Empty;
			if (delimeter is null || delimeter.Length == 0)
				throw new ArgumentNullException(nameof(delimeter));

			if (source is null)
				return string.Empty;

			if (source.Length < delimeter.Length)
				return source;

			var i = source.IndexOf(delimeter, StringComparison.Ordinal);
			if (i < 0)
				return source;

			remainder = source.Substring(i + delimeter.Length, source.Length - delimeter.Length - i);
			return source.Remove(i);
		}

		/// <summary> Extract last word (part of the string after last delimeter). </summary>
		/// <param name="source"></param>
		/// <param name="remainder">Part of source before last delimeter</param>
		/// <param name="delimeter"></param>
		/// <returns></returns>
		public static string ExtractLastWord(this string source, out string remainder, string delimeter = " ") {
			remainder = string.Empty;
			if (delimeter is null || delimeter.Length == 0)
				throw new ArgumentNullException(nameof(delimeter));

			if (source is null)
				return string.Empty;

			if (source.Length < delimeter.Length)
				return source;

			var i = source.LastIndexOf(delimeter, StringComparison.Ordinal);
			if (i < 0)
				return source;

			var result = source.Substring(i + delimeter.Length, source.Length - delimeter.Length - i);
			remainder = source.Remove(i);
			return result;
		}

		#endregion

		#region Replace methods

		public static string ReplaceLeft(
			this string source,
			string br,
			Func<string, string> replaceFunc,
			StringComparison comparisonType = StringComparison.OrdinalIgnoreCase
		) => ReplaceLeft(source, br, br, replaceFunc, comparisonType);

		public static string ReplaceLeft(
			this string source,
			string op,
			string ed,
			Func<string, string> replaceFunc,
			StringComparison comparisonType = StringComparison.OrdinalIgnoreCase
		) {
			Guard.NotNull(source, nameof(source));
			Guard.NotNullOrEmpty(op, nameof(op));
			Guard.NotNullOrEmpty(ed, nameof(ed));
			Guard.NotNull(replaceFunc, nameof(replaceFunc));

			var sb = new StringBuilder(source);
			var edPos = 0;
			while (edPos < sb.Length) {
				var opPos = sb.IndexOf(op, edPos, comparisonType);
				if (opPos < 0 || opPos + op.Length > sb.Length)
					return sb.ToString();

				edPos = sb.IndexOf(ed, opPos + op.Length, comparisonType);
				if (edPos < 0)
					return sb.ToString();

				var oldValue = sb.ToString(opPos + op.Length, edPos - opPos - op.Length);
				var newValue = replaceFunc(oldValue);

				sb.Remove(opPos, edPos - opPos + ed.Length);
				sb.Insert(opPos, newValue);
				edPos = opPos + newValue.Length;
			}

			return sb.ToString();
		}

		public static string ReplaceRight(
			this string source,
			string br,
			Func<string, string> replaceFunc,
			StringComparison comparisonType = StringComparison.OrdinalIgnoreCase
		) => ReplaceRight(source, br, br, replaceFunc, comparisonType);

		public static string ReplaceRight(
			this string source,
			string op,
			string ed,
			Func<string, string> replaceFunc,
			StringComparison comparisonType = StringComparison.OrdinalIgnoreCase
		) {
			Guard.NotNull(source, nameof(source));
			Guard.NotNullOrEmpty(op, nameof(op));
			Guard.NotNullOrEmpty(ed, nameof(ed));
			Guard.NotNull(replaceFunc, nameof(replaceFunc));

			var sb = new StringBuilder(source);
			var end1 = op.Length + ed.Length - 1;
			var end2 = op.Length - 1;

			var opPos = sb.Length - 1;
			while (opPos >= end1) {
				var edPos = sb.LastIndexOf(ed, opPos, comparisonType);
				if (edPos < end2)
					return sb.ToString();

				opPos = sb.LastIndexOf(op, edPos - 1, comparisonType);
				if (opPos < 0)
					return sb.ToString();

				var oldValue = sb.ToString(opPos + op.Length, edPos - opPos - op.Length);
				var newValue = replaceFunc(oldValue);

				sb.Remove(opPos, edPos - opPos + ed.Length);
				sb.Insert(opPos, newValue);

				--opPos;
			}

			return sb.ToString();
		}

		#endregion

		/// <summary> Repeat <paramref name="pattern"/> <paramref name="count"/> times </summary>
		/// <param name="pattern"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static string Repeat(this string pattern, uint count) {
			Guard.NotNull(pattern, nameof(pattern));
			if (pattern.Length == 0)
				return string.Empty;

			switch (count) {
				case 0: return string.Empty;
				case 1: return pattern;
				default: {
						var sb = new StringBuilder(pattern.Length * (int) count);
						for (var i = 0; i < count; i++)
							sb.Append(pattern);
						return sb.ToString();
					}
			}
		}

		private static int IndexOf(
			this StringBuilder sb,
			string value,
			int startIndex,
			StringComparison comparisonType = StringComparison.OrdinalIgnoreCase
		) {
			int index;
			var length = value.Length;
			var maxSearchLength = (sb.Length - length) + 1;

			switch (comparisonType) {
				case StringComparison.Ordinal:
				case StringComparison.CurrentCulture:
				case StringComparison.InvariantCulture: {
						for (var i = startIndex; i < maxSearchLength; ++i) {
							if (sb[i].Equals(value[0])) {
								index = 1;
								while (index < length && sb[i + index] == value[index])
									++index;

								if (index == length)
									return i;
							}
						}

						return -1;
					}

				case StringComparison.OrdinalIgnoreCase:
				case StringComparison.CurrentCultureIgnoreCase: {
						var culture = CultureInfo.CurrentCulture;
						for (var i = startIndex; i < maxSearchLength; ++i) {
							if (char.ToUpper(sb[i], culture) == char.ToUpper(value[0], culture)) {
								index = 1;
								while (index < length && char.ToUpper(sb[i + index], culture) == char.ToUpper(value[index], culture))
									++index;

								if (index == length)
									return i;
							}
						}

						return -1;
					}

				case StringComparison.InvariantCultureIgnoreCase: {
						for (var i = startIndex; i < maxSearchLength; ++i) {
							if (char.ToUpperInvariant(sb[i]) == char.ToUpperInvariant(value[0])) {
								index = 1;
								while (index < length && char.ToUpperInvariant(sb[i + index]) == char.ToUpperInvariant(value[index]))
									++index;

								if (index == length)
									return i;
							}
						}

						return -1;
					}

				default:
					throw new ArgumentOutOfRangeException(nameof(comparisonType), comparisonType, null);
			}
		}

		private static int LastIndexOf(
			this StringBuilder sb,
			string value,
			int startIndex,
			StringComparison comparisonType = StringComparison.OrdinalIgnoreCase
		) {
			int count;
			var index = value.Length - 1;

			switch (comparisonType) {
				case StringComparison.Ordinal:
				case StringComparison.CurrentCulture:
				case StringComparison.InvariantCulture: {
						for (var i = startIndex; i >= index; --i) {
							if (sb[i].Equals(value[index])) {
								count = 1;
								while (count <= index && sb[i - count] == value[index - count])
									++count;

								if (count >= index)
									return i - index;
							}
						}

						return -1;
					}

				case StringComparison.OrdinalIgnoreCase:
				case StringComparison.CurrentCultureIgnoreCase: {
						var culture = CultureInfo.CurrentCulture;
						for (var i = startIndex; i >= index; --i) {
							if (char.ToUpper(sb[i], culture) == char.ToUpper(value[index], culture)) {
								count = 1;
								while (count <= index && char.ToUpper(sb[i - count], culture) == char.ToUpper(value[index - count], culture))
									++count;

								if (count >= index)
									return i - index;
							}
						}

						return -1;
					}

				case StringComparison.InvariantCultureIgnoreCase: {
						for (var i = startIndex; i >= index; --i) {
							if (char.ToUpperInvariant(sb[i]) == char.ToUpperInvariant(value[index])) {
								count = 1;
								while (count <= index && char.ToUpperInvariant(sb[i - count]) == char.ToUpperInvariant(value[index - count]))
									++count;

								if (count >= index)
									return i - index;
							}
						}

						return -1;
					}

				default:
					throw new ArgumentOutOfRangeException(nameof(comparisonType), comparisonType, null);
			}
		}
	}
}