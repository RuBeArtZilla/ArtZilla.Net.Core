using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Common {
	public static class AssertEx {
		public static void IsFail(Action a) {
			try {
				a?.Invoke();
			} catch {
				return;
			}

			throw new AssertFailedException("No exception was throwed");
		}

		public static void IsFailWith<T>(Action a) where T : Exception {
			try {
				a?.Invoke();
			} catch (T) {
				return;
			} catch (Exception e) {
				throw new AssertFailedException("Throwed wrong exception: " + e.Message, e);
			}

			throw new AssertFailedException("No exception was throwed");
		}

		public static void IsSame<T>(this IEnumerable<T> enumerable0, IEnumerable<T> enumerable1) {
			if (ReferenceEquals(enumerable0, enumerable1))
				return;
			using var i0 = enumerable0.GetEnumerator();
			using var i1 = enumerable1.GetEnumerator();
			while (true) {
				var hasNext = i0.MoveNext();
				Assert.AreEqual(hasNext, i1.MoveNext(), "sequences contain different count of elements");
				if (!hasNext)
					return;
				Assert.AreEqual(i0.Current, i1.Current);
			}
		}
	}
}