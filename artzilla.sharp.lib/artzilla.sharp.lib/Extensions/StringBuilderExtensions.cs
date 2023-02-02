using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ArtZilla.Net.Core.Interfaces;

namespace ArtZilla.Net.Core.Extensions;

/// some useless string builder extensions
public static class StringBuilderExtensions {
	///
	public static int IndexOf(
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

	///
	public static int LastIndexOf(
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

	/// call <see cref="IToStringBuilder.ToStringBuilder"/> and return StringBuilder
	public static StringBuilder AppendEx<T>(this StringBuilder sb, T obj) where T : IToStringBuilder {
		obj.ToStringBuilder(sb);
		return sb;
	}

	/// call <see cref="IToStringBuilder.ToStringBuilder"/> and return StringBuilder
	public static StringBuilder AppendLineEx<T>(this StringBuilder sb, T obj) where T : IToStringBuilder {
		obj.ToStringBuilder(sb);
		return sb.AppendLine();
	}

	/// return string that contain result of a method <see cref="IToStringBuilder"/>
	public static string ToStringViaStringBuilder<T>(this T obj) where T : IToStringBuilder
		=> new StringBuilder().AppendEx(obj).ToString();

	// todo: add tests
	/// conditional append
	public static StringBuilder AppendIf(
		this StringBuilder sb, 
		bool condition, 
		string trueText = "",
		string falseText = ""
	) => sb.Append(condition ? trueText : falseText);

	// todo: add tests
	/// append text with prefix and suffix only if text is not null or empty
	public static StringBuilder AppendText(this StringBuilder sb, string? text, string prefix = "", string suffix = "")
		=> string.IsNullOrEmpty(text)
			? sb
			: sb.Append(prefix).Append(text).Append(suffix);

	// todo: add tests
	/// conditional append
	public static StringBuilder AppendFormatIf(
		this StringBuilder sb, 
		bool condition, 
		string format, 
		params object[] args
	) => condition
		? sb.AppendFormat(format, args)
		: sb;

	// todo: add tests
	/// append list
	public static StringBuilder AppendList<T>(
		this StringBuilder sb,
		#if NET40
		IList<T> items,
		#else
		IReadOnlyList<T> items,
		#endif
		string separator = "",
		string prefix = "",
		string suffix = "",
		bool enframeIfEmpty = false
	) {
		if (items.Count == 0)
			return enframeIfEmpty 
				? sb.Append(prefix).Append(suffix)
				: sb;

		sb.Append(prefix)
		  .Append(items[0]);

		for (var index = 1; index < items.Count; ++index)
			sb.Append(separator)
			  .Append(items[index]);

		return sb.Append(suffix);
	}

}
