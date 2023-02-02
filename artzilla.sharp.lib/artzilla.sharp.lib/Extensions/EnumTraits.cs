using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtZilla.Net.Core.Extensions;

/// From https://aka.ms/csharp_new_0618
/// <typeparam name="TEnum"></typeparam>
public static class EnumTraits<TEnum> where TEnum : struct, Enum {
	/// True, if TEnum contains no elements
	public static bool IsEmpty { get; }

	/// Min value of TEnum
	public static long MinValue { get; }

	/// Max value of TEnum
	public static long MaxValue { get; }

	/// TEnum values
	public static TEnum[] EnumValues { get; }

	static EnumTraits() {
		var type = typeof(TEnum);
		var underlyingType = Enum.GetUnderlyingType(type);

		EnumValues = (TEnum[]) Enum.GetValues(typeof(TEnum));
		ValuesSet = new(EnumValues);

		var longValues =
			EnumValues
				.Select(v => Convert.ChangeType(v, underlyingType))
				.Select(Convert.ToInt64)
				.ToList();

		IsEmpty = longValues.Count == 0;
		if (IsEmpty)
			return;

		var sorted = longValues.OrderBy(v => v).ToList();
		MinValue = sorted.Min();
		MaxValue = sorted.Max();
	}

	// This version is almost an order of magnitude faster then Enum.IsDefined
	/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
	/// <param name="value"></param>
	public static bool IsValid(TEnum value)
		=> ValuesSet.Contains(value);

	static readonly HashSet<TEnum> ValuesSet;
}
