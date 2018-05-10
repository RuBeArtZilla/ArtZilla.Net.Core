using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtZilla.Net.Core.Extensions {
	public static class ObjectExtensions {
		/// <summary> Simple check, that object is null </summary>
		public static bool IsNull<T>(this T self) where T : class
			=> self == null;

		/// <summary> Simple check, that object is any of values by using the default equality comparer </summary>
		public static bool IsAnyOf<T>(this T self, params T[] values)
			=> values.Contains(self);

		/// <summary> Simple check, that <paramref name="value"/> is any of <paramref name="values"/> by using a specified <see cref="IEqualityComparer{T}"/> </summary>
		public static bool IsAnyOf<T>(this T value, IEqualityComparer<T> comparer, params T[] values)
			=> values.Contains(value, comparer);

		/// <summary> Return collection, that contain only <paramref name="item"/> </summary>
		/// <typeparam name="T">Type of the <paramref name="item"/></typeparam>
		/// <param name="item">Element of result collection</param>
		/// <returns>Collection that contain only <paramref name="item"/></returns>
		public static IEnumerable<T> ToEnumerable<T>(this T item) {
			yield return item;
		}

		public static bool IsInOpenInterval<T>(this T value, T bound1, T bound2 = default) where T : IComparable<T> {
			var c1 = value.CompareTo(bound1);
			var c2 = value.CompareTo(bound2);

			return (c1 > 0 && c2 < 0) || (c1 < 0 && c2 > 0);
		}

		public static bool IsInClosedInterval<T>(this T value, T bound1, T bound2 = default) where T : IComparable<T> {
			var c1 = value.CompareTo(bound1);
			var c2 = value.CompareTo(bound2);

			return (c1 >= 0 && c2 <= 0) || (c1 <= 0 && c2 >= 0);
		}

		public static T InClosedInterval<T>(this T value, T bound1, T bound2 = default) where T : IComparable<T> {
			var c1 = value.CompareTo(bound1);
			if (c1 == 0)
				return bound1;

			var c2 = value.CompareTo(bound2);
			if (c2 == 0)
				return bound2;

			var c3 = bound1.CompareTo(bound2);
			if (c3 == 0)
				return bound1;

			var b = c3 > 0;
			if (b ^ c1 < 0)
				return bound1;
			if (b ^ c2 > 0)
				return bound2;
			return value;
		}

		// todo: add description
		public delegate void ActionRef<T>(ref T structure) where T : struct;
		public delegate TResult FuncRef<T, out TResult>(ref T structure) where T : struct;

		// todo: add description
		public static ref T With<T>(ref this T structure, ActionRef<T> initializer) where T : struct {
			initializer(ref structure);
			return ref structure;
		}

		// todo: add description
		public static T With<T>(this T obj, Action<T> initializer) where T : class {
			initializer(obj);
			return obj;
		}

		// todo: add description
		public static TResult Use<TResult, TSource>(this TSource source, Func<TSource, TResult> useMethod)
			=> useMethod(source);
	}
}