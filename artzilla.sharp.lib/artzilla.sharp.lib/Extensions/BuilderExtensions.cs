using System.Collections.Generic;

namespace ArtZilla.Net.Core.Extensions;

///
public static class BuilderExtensions {
	///
	public static Build.ListBuilder<T> BuildList<T>(this T item)
		=> Build.List(item);

	///
	public static Build.ListBuilder<T> BuildList<T>(this T item, params T[] items)
		=> Build.List(item).With(items);

	///
	public static Build.ListBuilder<T> With<T>(this List<T> items)
		=> Build.List((IEnumerable<T>) items);

	///
	public static Build.ArrayBuilder<T> BuildArray<T>(this T item)
		=> Build.Array(item);

	///
	public static Build.ArrayBuilder<T> BuildArray<T>(this T item, params T[] items) 
		=> Build.Array(item).With(items);

	///
	public static Build.ArrayBuilder<T> With<T>(this T[] items)
		=> Build.Array(items);

}