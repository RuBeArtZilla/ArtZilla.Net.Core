using System;
using System.Linq;
using System.Reflection;

namespace ArtZilla.Net.Core.Serialization;

internal static class ReflectionExt {
	public static bool HasAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute
		=> propertyInfo.GetCustomAttr<TAttribute>() != null;

	public static TAttribute GetCustomAttr<TAttribute>(this MemberInfo element) where TAttribute : Attribute
		=> (TAttribute) element.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();
}
