using System;
using System.Collections.Generic;

namespace ArtZilla.Net.Core.Extensions {
	public static class IListExtensions {
		/// <summary>
		/// Adds an <paramref name="item"/> to the <paramref name="list"/> and return it.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the code.</typeparam>
		/// <param name="list">Represents a collection of objects that can be individually accessed by index.</param>
		/// <param name="item">The object to add to the <paramref name="list"/>.</param>
		/// <returns>Added <paramref name="item"/></returns>
		/// <exception cref="ArgumentNullException"><paramref name="list"/> is null</exception>
		/// <exception cref="NotSupportedException"><paramref name="list"/> is read-only.</exception>
		public static T AddReturn<T>(this IList<T> list, T item) {
			Guard.NotNull(list, nameof(list));
			list.Add(item);
			return item;
		}
	}
}
