using System;
using ArtZilla.Sharp.Lib.Extensions;
using ArtZilla.Sharp.Lib.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ArtZilla.Sharp.Lib.Test.Common.Constants;

namespace ArtZilla.Sharp.Lib.Test.Extensions {
	[TestClass]
	public class GuardTests {
		[TestMethod]
		public void TestIsNotNull() {
			Int32? x = null;
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.NotNull(x));
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.NotNull(x, nameof(x)));

			x = MagicNumber;
			Guard.NotNull(x);
			Guard.NotNull(x, nameof(x));
		}

		[TestMethod]
		public void TestIsNull() {
			Int32? x = null;
			Guard.Null(x);
			Guard.Null(x, nameof(x));

			x = MagicNumber;
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.Null(x));
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.Null(x, nameof(x)));
		}

		[TestMethod]
		public void TestIsAnyText() {
			Guard.HasAnyText(TestString);
			Guard.HasAnyText(TestString, nameof(TestString));

			Guard.HasAnyText(TestStringEx);
			Guard.HasAnyText(TestStringEx, nameof(TestStringEx));

			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(NullString));
			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(NullString, nameof(NullString)));

			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(EmptyString));
			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(EmptyString, nameof(EmptyString)));

			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(WhitespacesString));
			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(WhitespacesString, nameof(WhitespacesString)));
		}

		[TestMethod]
		public void TestIsNotEmpty() {
			Guard.NotEmpty(MagicArray);
			Guard.NotEmpty(MagicArray, nameof(MagicArray));

			AssertEx.IsFailWith<ArgumentException>(() => Guard.NotEmpty(EmptyArray));
			AssertEx.IsFailWith<ArgumentException>(() => Guard.NotEmpty(EmptyArray, nameof(EmptyArray)));

			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.NotEmpty(NullArray));
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.NotEmpty(NullArray, nameof(NullArray)));
		}

		[TestMethod]
		public void TestInOpenInterval() {
			Guard.InOpenInterval(Val, Min, Max);
			Guard.InOpenInterval(Val, Min, Max, nameof(Val));

			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InOpenInterval(Min, Min, Max));
			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InOpenInterval(Min, Min, Max, nameof(Min)));

			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InOpenInterval(Max, Min, Max));
			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InOpenInterval(Max, Min, Max, nameof(Max)));

			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InOpenInterval(EMin, Min, Max));
			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InOpenInterval(EMin, Min, Max, nameof(EMin)));

			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InOpenInterval(EMax, Min, Max));
			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InOpenInterval(EMax, Min, Max, nameof(EMax)));
		}

		[TestMethod]
		public void TestInClosedInterval() {
			Guard.InClosedInterval(Val, Min, Max);
			Guard.InClosedInterval(Val, Min, Max, nameof(Val));

			Guard.InClosedInterval(Min, Min, Max);
			Guard.InClosedInterval(Min, Min, Max, nameof(Min));

			Guard.InClosedInterval(Max, Min, Max);
			Guard.InClosedInterval(Max, Min, Max, nameof(Max));

			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InClosedInterval(EMin, Min, Max));
			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InClosedInterval(EMin, Min, Max, nameof(EMin)));

			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InClosedInterval(EMax, Min, Max));
			AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => Guard.InClosedInterval(EMax, Min, Max, nameof(EMax)));
		}

		private const Int32 Val = 5;
		private const Int32 Max = 10;
		private const Int32 Min = 0;
		private const Int32 EMin = Min - 1;
		private const Int32 EMax = Max + 1;
	}
}
