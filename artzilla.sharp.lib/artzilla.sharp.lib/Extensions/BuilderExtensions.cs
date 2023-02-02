using System.Collections.Generic;

namespace ArtZilla.Net.Core.Extensions;

/// extension methods for fluent collection builder
public static class BuilderExtensions {
	/// start to build list with item
	public static Build.ListBuilder<T> BuildList<T>(this T item)
		=> Build.List(item);
	
	/// start to build list with item and items
	public static Build.ListBuilder<T> BuildList<T>(this T item, params T[] items)
		=> Build.List(item).With(items);
	
	/// start to build list with items
	public static Build.ListBuilder<T> With<T>(this List<T> items)
		=> Build.List((IEnumerable<T>) items);
	
	/// start to build array with item
	public static Build.ArrayBuilder<T> BuildArray<T>(this T item)
		=> Build.Array(item);

	/// start to build array with items
	public static Build.ArrayBuilder<T> BuildArray<T>(this T item, params T[] items) 
		=> Build.Array(item).With(items);

	/// start to build array with items
	public static Build.ArrayBuilder<T> With<T>(this T[] items)
		=> Build.Array(items);

}