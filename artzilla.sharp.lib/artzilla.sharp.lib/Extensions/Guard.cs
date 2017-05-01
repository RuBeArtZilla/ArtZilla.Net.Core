using System;

namespace ArtZilla.Sharp.Lib.Extensions {
	public static class Guard {
		public static void Null<T>(T param) {
			if (param != null)
				throw new ArgumentNullException();
		}

		public static void Null<T>(T param, String paramName) {
			if (param != null)
				throw new ArgumentNullException(paramName);
		}

		public static void NotNull<T>(T param) {
			if (param == null)
				throw new ArgumentNullException();
		}

		public static void NotNull<T>(T param, String paramName) {
			if (param == null)
				throw new ArgumentNullException(paramName);
		}

		public static void HasAnyText(String param) {
			if (String.IsNullOrWhiteSpace(param))
				throw new ArgumentException();
		}

		public static void HasAnyText(String param, String paramName) {
			if (String.IsNullOrWhiteSpace(param))
				throw new ArgumentException(paramName);
		}

		public static void NotEmpty<T>(T[] param) {
			if (param == null)
				throw new ArgumentNullException();
			if (param.Length == 0)
				throw new ArgumentException();
		}

		public static void NotEmpty<T>(T[] param, String paramName) {
			if (param == null)
				throw new ArgumentNullException(paramName);
			if (param.Length == 0)
				throw new ArgumentException(paramName);
		}

		public static void InOpenInterval<T>(T value, T minimum, T maximum) where T : IComparable<T> {
			if (value.CompareTo(minimum) <= 0 || value.CompareTo(maximum) >= 0)
				throw new ArgumentOutOfRangeException();
		}

		public static void InOpenInterval<T>(T value, T minimum, T maximum, string paramName) where T : IComparable<T> {
			if (value.CompareTo(minimum) <= 0 || value.CompareTo(maximum) >= 0)
				throw new ArgumentOutOfRangeException(paramName);
		}

		public static void InClosedInterval<T>(T value, T minimum, T maximum) where T : IComparable<T> {
			if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
				throw new ArgumentOutOfRangeException();
		}

		public static void InClosedInterval<T>(T value, T minimum, T maximum, string paramName) where T : IComparable<T> {
			if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
				throw new ArgumentOutOfRangeException(paramName);
		}
	}
}
