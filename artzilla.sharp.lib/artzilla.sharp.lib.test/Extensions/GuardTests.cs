using System;
using ArtZilla.Net.Core.Extensions;
using ArtZilla.Net.Core.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions {
	[TestClass]
	public class GuardTests {
		[TestMethod]
		public void TestIsNotNull() {
			Int32? x = null;
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.NotNull(x));
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.NotNull(x, nameof(x)));

			x = Constants.MagicNumber;
			Guard.NotNull(x);
			Guard.NotNull(x, nameof(x));
		}

		[TestMethod]
		public void TestIsNull() {
			Int32? x = null;
			Guard.Null(x);
			Guard.Null(x, nameof(x));

			x = Constants.MagicNumber;
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.Null(x));
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.Null(x, nameof(x)));
		}

		[TestMethod]
		public void TestIsAnyText() {
			Guard.HasAnyText(Constants.TestString);
			Guard.HasAnyText(Constants.TestString, nameof(Constants.TestString));

			Guard.HasAnyText(Constants.TestStringEx);
			Guard.HasAnyText(Constants.TestStringEx, nameof(Constants.TestStringEx));

			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(Constants.NullString));
			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(Constants.NullString, nameof(Constants.NullString)));

			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(Constants.EmptyString));
			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(Constants.EmptyString, nameof(Constants.EmptyString)));

			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(Constants.WhitespacesString));
			AssertEx.IsFailWith<ArgumentException>(() => Guard.HasAnyText(Constants.WhitespacesString, nameof(Constants.WhitespacesString)));
		}

		[TestMethod]
		public void TestIsNotEmpty() {
			Guard.NotEmpty(Constants.MagicArray);
			Guard.NotEmpty(Constants.MagicArray, nameof(Constants.MagicArray));

			AssertEx.IsFailWith<ArgumentException>(() => Guard.NotEmpty(Constants.EmptyArray));
			AssertEx.IsFailWith<ArgumentException>(() => Guard.NotEmpty(Constants.EmptyArray, nameof(Constants.EmptyArray)));

			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.NotEmpty(Constants.NullArray));
			AssertEx.IsFailWith<ArgumentNullException>(() => Guard.NotEmpty(Constants.NullArray, nameof(Constants.NullArray)));
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
