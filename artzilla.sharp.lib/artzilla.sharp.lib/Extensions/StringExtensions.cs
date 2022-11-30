using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ArtZilla.Net.Core.Extensions;

/// Some useless string extensions
public static class StringExtensions {
	/// Wrapper for <see cref="string.IsNullOrWhiteSpace"/>
	public static bool IsBad(this string value)
		=> string.IsNullOrWhiteSpace(value);

	/// Wrapper for is not <see cref="string.IsNullOrWhiteSpace"/>
	public static bool IsGood(this string value)
		=> !string.IsNullOrWhiteSpace(value);

	/// Wrapper for <see cref="string.IsNullOrEmpty"/>
	public static bool IsNullOrEmpty(this string value)
		=> string.IsNullOrEmpty(value);

	/// Negation of <see cref="string.IsNullOrWhiteSpace"/>
	public static bool IsNotNullOrEmpty(this string value)
		=> !string.IsNullOrEmpty(value);

	/// Wrapper for <see cref="string.IsNullOrWhiteSpace"/>
	public static bool IsNullOrWhiteSpace(this string value)
		=> string.IsNullOrWhiteSpace(value);

	/// Negation of <see cref="string.IsNullOrWhiteSpace"/>
	public static bool IsNotNullOrWhiteSpace(this string value)
		=> !string.IsNullOrWhiteSpace(value);

	/// Wrapper for <see cref="string.Equals(string, System.StringComparison)"/> with OrdinalIgnoreCase.
	/// Null values always returns false.
	/// <returns> True if left string equals other string (IgnoreCase), otherwise false. </returns>
	public static bool Like(this string left, string right)
		=> left?.Equals(right, StringComparison.OrdinalIgnoreCase) ?? false;

	/// Parse source string to dictionary
	/// <param name="source"></param>
	/// <param name="pairSeparator">separator between key and value</param>
	/// <param name="listSeparator">separator between pairs</param>
	/// <returns></returns>
	public static Dictionary<string, string> ToDictionary(
		this string source,
		string pairSeparator = "=",
		string listSeparator = ";"
	)
		=> source?.Split(new[] { listSeparator }, StringSplitOptions.RemoveEmptyEntries)
			.Select(i => i.Split(new[] { pairSeparator }, 2, StringSplitOptions.None))
			.Where(i => i.Length == 2 && i[0].Length > 0)
			.GroupBy(i => i[0], i => i[1], StringComparer.OrdinalIgnoreCase)
			.ToDictionary(i => i.Key, i => i.First().Trim(), StringComparer.OrdinalIgnoreCase);

	/// <TODO>add description, that method return combined strings with delimeter, or any not bad string, or empty string.</TODO>
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
	
	/// <TODO>add description, that method return combined strings with delimeter, or any not bad string, or empty string.</TODO>
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
	
	/// Add <paramref name="prefix"/> and <paramref name="postfix"/> to <paramref name="source"/>
	/// if statement is true otherwise returns empty string.
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
	
	/// Add <paramref name="prefix"/> and <paramref name="postfix"/> to <paramref name="source"/>
	/// if statement is not <see langword="null" /> otherwise returns empty string.
	/// <param name="source">the source string</param>
	/// <param name="prefix">prefix string, can be empty</param>
	/// <param name="postfix">postfix string, can be empty</param>
	/// <returns></returns>
	public static string EnframeNotNull(this string source, string prefix = "", string postfix = "")
		=> source is null ? "" : prefix + source + postfix;
	
	/// Add <paramref name="prefix"/> and <paramref name="postfix"/> to <paramref name="source"/>
	/// if source is not <see langword="null" /> or empty otherwise returns empty string.
	/// <param name="source">the source string</param>
	/// <param name="prefix">prefix string, can be empty</param>
	/// <param name="postfix">postfix string, can be empty</param>
	/// <returns></returns>
	public static string EnframeNotEmpty(this string source, string prefix = "", string postfix = "")
		=> string.IsNullOrEmpty(source) ? "" : prefix + source + postfix;
	
	/// Add <paramref name="prefix"/> and <paramref name="postfix"/> to <paramref name="source"/>
	/// if source is not <see langword="null" />, empty, or consists only of white-space characters
	/// otherwise returns empty string.
	/// <param name="source">the source string</param>
	/// <param name="prefix">prefix string, can be empty</param>
	/// <param name="postfix">postfix string, can be empty</param>
	/// <returns></returns>
	public static string EnframeText(this string source, string prefix = "", string postfix = "")
		=> string.IsNullOrWhiteSpace(source) ? "" : prefix + source + postfix;

	#endregion

	#region Trim methods

	/// Method remove prefix if exist
	/// <param name="source">Source string</param>
	/// <param name="prefix">Prefix string</param>
	/// <param name="comparisonType"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException">prefix </exception>
	public static string TrimPrefix(
		this string source,
		string prefix,
		StringComparison comparisonType = StringComparison.Ordinal
	)
		=> source?.StartsWith(prefix, comparisonType) == true
			? source.Substring(prefix.Length)
			: source ?? string.Empty;

	/// Method remove suffix if exist
	/// <param name="source">Source string</param>
	/// <param name="suffix">Suffix string</param>
	/// <param name="comparisonType"></param>
	/// <returns></returns>
	public static string TrimSuffix(
		this string source,
		string suffix,
		StringComparison comparisonType = StringComparison.Ordinal
	)
		=> source?.EndsWith(suffix, comparisonType) == true
			? source.Remove(source.Length - suffix.Length)
			: source ?? string.Empty;

	#endregion

	#region Parse methods
	
	///	Converts the string representation of a number to its 32-bit signed integer equivalent, or default value
	/// <param name="source">A string containing a number to convert.</param>
	/// <param name="defValue">Default value, returned if conversion failed. Default value <see cref="int.MinValue"/></param>
	/// <returns> When this method returns, result is the 32-bit signed integer value equivalent to the number contained in s, or defValue if can't parse number </returns>
	public static int ParseIntEx(this string source, int defValue = int.MinValue) {
		if (source.IsBad())
			return defValue;
		return int.TryParse(source, out var val) ? val : defValue;
	}

	///
	public static double ParseDoubleEx(this string source, double defValue = double.NaN) {
		if (source.IsBad())
			return defValue;
		return double.TryParse(source, out var val) ? val : defValue;
	}
	
	///
	public static double ParseDoubleEx(
		this string source,
		NumberStyles ns,
		IFormatProvider ifp,
		double defValue = double.NaN
	) {
		if (source.IsBad())
			return defValue;
		return double.TryParse(source, ns, ifp, out var val) ? val : defValue;
	}
	
	///
	public static bool ParseBoolEx(this string source, bool defValue = false) {
		if (source.IsBad())
			return defValue;
		return bool.TryParse(source, out var val) ? val : defValue;
	}

	#endregion

	#region Extract methods
	
	///
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
	
	///
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
	
	///
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
	
	///
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
	
	///
	public static string ExtractLast(
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
	
	///
	public static string ExtractLast(
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
	
	///
	public static string ExtractLast(
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

		var ned = input.LastIndexOf(ed, comparison);
		if (ned < op.Length)
			return default;

		var nop = input.LastIndexOf(op, ned - 1, comparison);
		if (nop < 0)
			return default;

		nop += op.Length;

		return input.Substring(nop, ned - nop);
	}
	
	///
	public static string ExtractLast(
		this string input,
		string border,
		StringComparison comparison = StringComparison.Ordinal
	) {
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

	/// Extract first word (part of the string before first delimeter).
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

	/// Extract last word (part of the string after last delimeter).
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
	
	///
	public static string ReplaceLeft(
		this string source,
		string br,
		Func<string, string> replaceFunc,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase
	) => ReplaceLeft(source, br, br, replaceFunc, comparisonType);
	
	///
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
	
	///
	public static string ReplaceRight(
		this string source,
		string br,
		Func<string, string> replaceFunc,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase
	) => ReplaceRight(source, br, br, replaceFunc, comparisonType);
	
	///
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

	/// Repeat <paramref name="pattern"/> <paramref name="count"/> times
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

	// alt. wrapper for substring with begin & end position
	/// Retrieves a substring from this instance.
	/// The substring starts at a specified character position to the next position
	/// <param name="source">A string</param>
	/// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
	/// <param name="endIndex">The zero-based ending character position of a substring in this instance.</param>
	/// <returns></returns>
	public static string Cut(this string source, int startIndex, int endIndex)
		=> source.Substring(startIndex, endIndex - startIndex);

	/// The zero-based index of first not a white space character
	/// <param name="source">A string</param>
	/// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
	/// <returns>
	/// The zero-based starting index position of first not a white space character,
	/// or -1 if it is not found or if the current instance equals <see cref="String.Empty" />. </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex" /> is less than zero.</exception>
	public static int GetIndexOfFirstSymbol(this string source, int startIndex = 0) {
		for (var i = startIndex; i < source.Length; i++)
			if (!char.IsWhiteSpace(source, i))
				return i;

		return -1;
	}

	/// The zero-based index of first occurence of any <paramref name="characters"/> in <paramref name="source"/>
	/// <param name="source">A string</param>
	/// <param name="found">Founded character</param>
	/// <param name="characters">Array of characters</param>
	/// <returns></returns>
	public static int GetIndexOfAny(this string source, out char found, params char[] characters)
		=> GetIndexOfAny(source, 0, out found, characters);

	/// The zero-based index of first occurence of any <paramref name="characters"/> in <paramref name="source"/>
	/// <param name="source">A string</param>
	/// <param name="startIndex">The search starting position. The search proceeds from <paramref name="startIndex" /> toward the beginning of <paramref name="source"/>.</param>
	/// <param name="found">Founded character</param>
	/// <param name="characters">Array of characters</param>
	/// <returns></returns>
	public static int GetIndexOfAny(this string source, int startIndex, out char found, params char[] characters) {
		for (var i = startIndex; i < source.Length; ++i)
			for (var j = 0; j < characters.Length; ++j)
				if (source[i] == characters[j]) {
					found = characters[j];
					return i;
				}

		found = default;
		return -1;
	}

	/// Reports the zero-based index of the of the <paramref name="ed"/> bracket in the <paramref name="source"/>.  
	/// <param name="source"></param>
	/// <param name="start"></param>
	/// <param name="op"></param>
	/// <param name="ed"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is less than zero.</exception>
	public static int GetNextBracket(this string source, int start = 0, char op = '(', char ed = ')') {
		var deep = 0;
		for (var i = start; i < source.Length; ++i) {
			var c = source[i];
			if (c == op)
				++deep;
			else if (c == ed && 0 == deep--)
				return i;
		}

		return -1;
	}
}
