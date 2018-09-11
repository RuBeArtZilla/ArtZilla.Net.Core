using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtZilla.Net.Core.Extensions {
	/// <summary>
	/// From https://aka.ms/csharp_new_0618
	/// </summary>
	/// <typeparam name="TEnum"></typeparam>
	public static class EnumTraits<TEnum> where TEnum : struct, Enum {
		/// <summary> True, if TEnum contains no elements </summary>
		public static bool IsEmpty { get; }

		/// <summary> Min value of TEnum </summary>
		public static long MinValue { get; }

		/// <summary> Max value of TEnum </summary>
		public static long MaxValue { get; }

		/// <summary> TEnum values </summary>
		public static TEnum[] EnumValues { get; }

		static EnumTraits() {
			var type = typeof(TEnum);
			var underlyingType = Enum.GetUnderlyingType(type);

			EnumValues = (TEnum[]) Enum.GetValues(typeof(TEnum));
			_valuesSet = new HashSet<TEnum>(EnumValues);

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
		/// <summary> Returns an indication whether a constant with a specified value exists in a specified enumeration. </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsValid(TEnum value) => _valuesSet.Contains(value);

		private static HashSet<TEnum> _valuesSet;
	}
}