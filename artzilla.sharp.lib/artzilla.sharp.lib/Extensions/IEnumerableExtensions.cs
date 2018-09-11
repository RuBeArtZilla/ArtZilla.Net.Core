using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtZilla.Net.Core.Extensions {
	public static class EnumerableExtensions {
		/// <summary>
		/// Performs the specified action on each element in <see cref="IEnumerable{T}"/> and return collection.
		/// <para>Invoke action only if result was enumerated!</para>
		/// </summary>
		/// <typeparam name="T">The type of elements.</typeparam>
		/// <param name="items">Collection of elements.</param>
		/// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the <see cref="IEnumerable{T}"/>.</param>
		/// <returns>Elements of <paramref name="items"/> collection after <paramref name="action"/> execute.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="items"/> or <paramref name="action"/> is null.</exception>
		public static IEnumerable<T> DoBefore<T>(this IEnumerable<T> items, Action<T> action) {
			// local method for fail fast if arguments is null;
			IEnumerable<T> InnerForEvery() {
				foreach (var item in items) {
					action(item);
					yield return item;
				}
			}

			Guard.NotNull(items, nameof(items));
			Guard.NotNull(action, nameof(action));
			return InnerForEvery();
		}

		/// <summary>
		/// Return collection that enumerates <paramref name="items"/> and contain <paramref name="item0"/> at the end. 
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="items">Input sequence.</param>
		/// <param name="item0">Item after input sequence.</param>
		/// <returns>Return collection that enumerates <paramref name="items"/> and contain <paramref name="item0"/> at the end.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> items, T item0) {
			IEnumerable<T> InnerAppend() {
				foreach (var item in items)
					yield return item;

				yield return item0;
			}

			Guard.NotNull(items, nameof(items));
			return InnerAppend();
		}

		/// <summary>
		/// Return collection that enumerates <paramref name="items"/> and contain <paramref name="item0"/> and <paramref name="item1"/> at the end. 
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="items">Input sequence.</param>
		/// <param name="item0">Item after input sequence.</param>
		/// <param name="item1">Item after <paramref name="item0"/>.</param>
		/// <returns>Return collection that enumerates <paramref name="items"/> and contain <paramref name="item0"/> and <paramref name="item1"/> at the end.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> items, T item0, T item1) {
			IEnumerable<T> InnerAppend() {
				foreach (var item in items)
					yield return item;

				yield return item0;
				yield return item1;
			}

			Guard.NotNull(items, nameof(items));

			return InnerAppend();
		}

		/// <summary>
		/// Return collection that enumerates <paramref name="items"/> and <paramref name="args"/> at the end. 
		/// </summary>
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
	}
}
