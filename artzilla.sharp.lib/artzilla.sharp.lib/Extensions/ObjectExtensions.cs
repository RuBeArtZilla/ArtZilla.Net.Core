using System.Linq;

namespace ArtZilla.Sharp.Lib.Extensions {
	public static class ObjectExtensions {
		/// <summary>
		/// Simple check, that object is null
		/// </summary>
		public static bool IsNull<T>(this T self)
			=> self == null;

		/// <summary>
		/// Simple check, that object is any of values by using the default equality comparer
		/// </summary>
		public static bool IsAnyOf<T>(this T self, params T[] values)
			=> values.Contains(self);
	}
}