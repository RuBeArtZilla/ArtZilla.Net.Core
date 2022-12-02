using System.Collections.Generic;

namespace ArtZilla.Net.Core;

/// 
public static class Build {
	///
	public static ListBuilder<T> List<T>(T item) => new(item);

	///
	public static ListBuilder<T> List<T>(params T[] items) => new(items);

	///
	public static ListBuilder<T> List<T>(IEnumerable<T> items) => new(items);

	///
	public static ArrayBuilder<T> Array<T>(T item) => new(item);

	///
	public static ArrayBuilder<T> Array<T>(params T[] items) => new(items);

	///
	public static ArrayBuilder<T> Array<T>(IEnumerable<T> items) => new(items);


	/// 
	/// <typeparam name="T"></typeparam>
	public readonly struct ListBuilder<T> {
		readonly List<T> _items;

		internal ListBuilder(T item) => _items = new() { item };
		
		internal ListBuilder(IEnumerable<T> items) => _items = new(items);

		/// 
		public List<T> Create()
			=> _items;

		///
		public ListBuilder<T> With(T item) {
			_items.Add(item);
			return this;
		}

		///
		public ListBuilder<T> With(IEnumerable<T> items) {
			_items.AddRange(items);
			return this;
		}
		
		///
		public ListBuilder<T> With(params T[] itemsArray) {
			_items.AddRange(itemsArray);
			return this;
		}
	}
	
	/// 
	/// <typeparam name="T"></typeparam>
	public readonly struct ArrayBuilder<T>{
		readonly List<T> _items;

		///
		internal ArrayBuilder(T item) => _items = new() { item };
		
		///
		internal ArrayBuilder(IEnumerable<T> items) => _items = new(items);
		
		///
		public T[] Create()
			=> _items.ToArray();
		
		///
		public ArrayBuilder<T> With(T item) {
			_items.Add(item);
			return this;
		}

		///
		public ArrayBuilder<T> With(IEnumerable<T> items) {
			_items.AddRange(items);
			return this;
		}
		
		///
		public ArrayBuilder<T> With(params T[] itemsArray) {
			_items.AddRange(itemsArray);
			return this;
		}
	}
	
}