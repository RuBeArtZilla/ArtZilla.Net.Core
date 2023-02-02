using System.Collections.Generic;

namespace ArtZilla.Net.Core;

/// fluent collection builder
public static class Build {
	/// start build collection with initial item
	public static ListBuilder<T> List<T>(T item) => new(item);
	
	/// start build collection with initial items
	public static ListBuilder<T> List<T>(params T[] items) => new(items);
	
	/// start build collection with initial items
	public static ListBuilder<T> List<T>(IEnumerable<T> items) => new(items);

	/// start build array with initial item
	public static ArrayBuilder<T> Array<T>(T item) => new(item);
	
	/// start build array with initial items
	public static ArrayBuilder<T> Array<T>(params T[] items) => new(items);
	
	/// start build array with initial items
	public static ArrayBuilder<T> Array<T>(IEnumerable<T> items) => new(items);

	/// list builder
	/// <typeparam name="T"></typeparam>
	public readonly struct ListBuilder<T> {
		readonly List<T> _items;

		internal ListBuilder(T item) => _items = new() { item };
		
		internal ListBuilder(IEnumerable<T> items) => _items = new(items);

		/// create list with items
		public List<T> Create()
			=> _items;

		/// add item to list
		public ListBuilder<T> With(T item) {
			_items.Add(item);
			return this;
		}

		/// add items to list
		public ListBuilder<T> With(IEnumerable<T> items) {
			_items.AddRange(items);
			return this;
		}
		
		/// add items to list
		public ListBuilder<T> With(params T[] itemsArray) {
			_items.AddRange(itemsArray);
			return this;
		}
	}
	
	/// array builder
	/// <typeparam name="T"></typeparam>
	public readonly struct ArrayBuilder<T>{
		readonly List<T> _items;

		internal ArrayBuilder(T item) => _items = new() { item };
		
		internal ArrayBuilder(IEnumerable<T> items) => _items = new(items);
		
		/// create array with items
		public T[] Create()
			=> _items.ToArray();
		
		/// add item to array
		public ArrayBuilder<T> With(T item) {
			_items.Add(item);
			return this;
		}

		/// add items to array
		public ArrayBuilder<T> With(IEnumerable<T> items) {
			_items.AddRange(items);
			return this;
		}
		
		/// add items to array
		public ArrayBuilder<T> With(params T[] itemsArray) {
			_items.AddRange(itemsArray);
			return this;
		}
	}
}