using System;
using System.Globalization;
using System.Text;

namespace ArtZilla.Net.Core.Extensions;

/// <summary> Some useless string builder extensions </summary>
public static class StringBuilderExtensions {

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
}
