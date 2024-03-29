﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtZilla.Net.Core.Extensions;

/// Extension for <see cref="IEnumerable{T}"/>
public static class EnumerableExtensions {
	/// Default size for chunk
	public const int DefaultChunkSize = 0x1 << 10;

	/// Performs the specified action on each element in <see cref="IEnumerable{T}"/> and return collection.
	/// <para>Invoke action only if result was enumerated!</para>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="items">Collection of elements.</param>
	/// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the <see cref="IEnumerable{T}"/>.</param>
	/// <returns>Elements of <paramref name="items"/> collection after <paramref name="action"/> execute.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="items"/> or <paramref name="action"/> is null.</exception>
	public static IEnumerable<T> DoBefore<T>(this IEnumerable<T> items, Action<T> action) {
		if (items is null)
			throw new ArgumentNullException(nameof(items));
		if (action is null)
			throw new ArgumentNullException(nameof(action));
		return InnerForEvery(items, action);

		// local method for fail fast if arguments is null;
		static IEnumerable<T> InnerForEvery(IEnumerable<T> lItems, Action<T> lAction) {
			foreach (var item in lItems) {
				lAction(item);
				yield return item;
			}
		}
	}

#if NETFULL // .net core & .net standard already has this method

	/// Return collection that enumerates <paramref name="items"/> and contain <paramref name="item"/> at the end. 
	/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
	/// <param name="items">Input sequence.</param>
	/// <param name="item">Item after input sequence.</param>
	/// <returns>Return collection that enumerates <paramref name="items"/> and contain <paramref name="item"/> at the end.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
	public static IEnumerable<T> Append<T>(this IEnumerable<T> items, T item) {
		if (items is null)
			throw new ArgumentNullException(nameof(items));

		return InnerAppend(items, item);

		// local method for fail fast if arguments is null;
		static IEnumerable<T> InnerAppend(IEnumerable<T> lItems, T lItem) {
			foreach (var i in lItems)
				yield return i;

			yield return lItem;
		}
	}

#endif

	/// Return collection that enumerates <paramref name="items"/> and contain <paramref name="item0"/> and <paramref name="item1"/> at the end. 
	/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
	/// <param name="items">Input sequence.</param>
	/// <param name="item0">Item after input sequence.</param>
	/// <param name="item1">Item after <paramref name="item0"/>.</param>
	/// <returns>Return collection that enumerates <paramref name="items"/> and contain <paramref name="item0"/> and <paramref name="item1"/> at the end.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
	public static IEnumerable<T> Append<T>(this IEnumerable<T> items, T item0, T item1) {
		if (items is null)
			throw new ArgumentNullException(nameof(items));
		return InnerAppend(items, item0, item1);

		// local method for fail fast if arguments is null;
		static IEnumerable<T> InnerAppend(IEnumerable<T> lItems, T lItem0, T lItem1) {
			foreach (var item in lItems)
				yield return item;

			yield return lItem0;
			yield return lItem1;
		}
	}

	/// Return collection that enumerates <paramref name="items"/> and <paramref name="args"/> at the end. 
	/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
	/// <param name="items">Input sequence.</param>
	/// <param name="args">Items that would be after input sequence.</param>
	/// <returns>Return collection that enumerates <paramref name="items"/> and contain <paramref name="args"/> at the end.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
	public static IEnumerable<T> Append<T>(this IEnumerable<T> items, params T[] args) {
		// TODO: test, maybe this better
		/* IEnumerable<T> InnerAppend() {
			foreach (var item in items)
				yield return item;

			foreach (var item in args)
				yield return item;
		} 

		Guard.NotNull(items, nameof(items));
		Guard.NotNull(args, nameof(args));

		return InnerAppend();	*/

		return items.Concat(args);
	}

	/// Convert items sequence to string
	/// <typeparam name="T">Type of item in sequence</typeparam>
	/// <param name="items">sequence of items</param>
	/// <param name="delimeter">delimeter between each item</param>
	/// <returns></returns>
	public static string Combine<T>(this IEnumerable<T> items, string delimeter = ", ") {
		if (items is null)
			throw new ArgumentNullException(nameof(items));

		var sb = new StringBuilder();
		foreach (var item in items) {
			var str = item?.ToString();
			if (str.IsNullOrWhiteSpace())
				continue;
			if (sb.Length > 0)
				sb.Append(delimeter);
			sb.Append(item);
		}

		return sb.ToString();
	}

	/// Split enumerable by chunks
	/// <typeparam name="T">Type of element in the enumerable</typeparam>
	/// <param name="items">counf of items</param>
	/// <param name="size"></param>
	/// <returns></returns>
	public static IEnumerable<T[]> Split<T>(this IEnumerable<T> items, int size = DefaultChunkSize) {
		if (size <= 0)
			throw new ArgumentOutOfRangeException(nameof(size), "Argument should be positive number");

		switch (items) {
			case T[] array when array.Length < size:
				return array.ToEnumerable();
			case ICollection<T> collection when collection.Count < size:
				return collection.ToArray().ToEnumerable();
			default:
				return InnerSplit(items, size);
		}

		// local method for fail fast if arguments is null;
		static IEnumerable<T[]> InnerSplit(IEnumerable<T> enumerable, int chunkSize) {
			var buffer = new T[chunkSize];
			var index = 0;
			foreach (var item in enumerable) {
				buffer[index++] = item;
				if (index != chunkSize)
					continue;

				yield return buffer;
				buffer = new T[chunkSize]; // creating new array for each chunk
				index = 0;
			}

			if (index == 0)
				yield break;

			Array.Resize(ref buffer, index);
			yield return buffer;
		}
	}

	public static IEnumerable<T> ConditionalWhere<T>(
		this IEnumerable<T> source,
		bool condition,
		Func<T, bool> predicate
	) => condition
			? source.Where(predicate)
			: source;

	public static IEnumerable<T> ConditionalWhere<T>(
		this IEnumerable<T> source,
		bool condition,
		Func<T, int, bool> predicate
	) => condition
		? source.Where(predicate)
		: source;
}
