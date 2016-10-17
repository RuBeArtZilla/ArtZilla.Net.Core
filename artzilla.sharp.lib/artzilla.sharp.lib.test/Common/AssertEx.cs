using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Sharp.Lib.Test {
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
		}
}