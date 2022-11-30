using System;
using System.Collections.Generic;

namespace ArtZilla.Net.Core.Extensions;

/// Extension methods for IList{}
public static class ListExtensions {
	/// Adds an <paramref name="item"/> to the <paramref name="list"/> and return it.
	/// <typeparam name="TListItem">List item type</typeparam>
	/// <typeparam name="TItem">Item type</typeparam>
	/// <param name="list">Represents a collection of objects that can be individually accessed by index.</param>
	/// <param name="item">The object to add to the <paramref name="list"/>.</param>
	/// <returns>Added <paramref name="item"/></returns>
	/// <exception cref="ArgumentNullException"><paramref name="list"/> is null</exception>
	/// <exception cref="NotSupportedException"><paramref name="list"/> is read-only.</exception>
	public static TItem AddReturn<TListItem, TItem>(this IList<TListItem> list, TItem item) where TItem : TListItem {
		Guard.NotNull(list, nameof(list));
		list.Add(item);
		return item;
	}
}
