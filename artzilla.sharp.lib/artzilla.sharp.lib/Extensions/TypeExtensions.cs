using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArtZilla.Sharp.Lib.Extensions {
	public static class TypeExtensions {
		public static IEnumerable<PropertyInfo> GetReadableNonIndexerProperties(this Type type) {
			var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			return properties.Where(p => p.CanRead && p.GetIndexParameters().Length == 0);
		}

		public static Type GetIEnumerableInterface(this Type type)
				=> type.GetInterface("IEnumerable`1");

		public static bool IsImplementIEnumerable(this Type type)
				=> type.GetInterface("IEnumerable`1") != null;

		/// <summary>
		/// Return that null can be assigned to instance of this type.
		/// </summary>
		public static bool IsNullable(this Type type)
			=> (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			|| !type.IsValueType;
	}
}