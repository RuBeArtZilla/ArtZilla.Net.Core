using System.Collections.Generic;
using System.Linq;

namespace ArtZilla.Net.Core.Extensions {
	public static class ObjectExtensions {
		/// <summary>
		/// Simple check, that object is null
		/// </summary>
		public static bool IsNull<T>(this T self)
			=> self == null;

		/// <summary>
		/// Simple check, that object is any of values by using the default equality comparer
		/// </summary>
		public static bool IsAnyOf<T>(this T self, params T[] values)
			=> values.Contains(self);

		/// <summary>
		/// Simple check, that <paramref name="value"/> is any of <paramref name="values"/> by using a specified <see cref="IEqualityComparer{T}"/>
		/// </summary>
		public static bool IsAnyOf<T>(this T value, IEqualityComparer<T> comparer, params T[] values)
			=> values.Contains(value, comparer);

		/// <summary>
		/// Return collection, that contain only <paramref name="item"/>
		/// </summary>
		/// <typeparam name="T">Type of the <paramref name="item"/></typeparam>
		/// <param name="item">Element of result collection</param>
		/// <returns>Collection that contain only <paramref name="item"/></returns>
		public static IEnumerable<T> ToEnumerable<T>(this T item) {
			yield return item;
		}
	}
}