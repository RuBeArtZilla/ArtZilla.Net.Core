using System;
using System.Linq;
using ArtZilla.Net.Core.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Tests {
	[TestClass]
	public class AssertExTests {
		[TestMethod]
		public void IsFail() {
			AssertEx.IsFail(ThrowException);

			try {
				AssertEx.IsFail(NotThrowException);
			} catch (AssertFailedException) {
				return;
			}

			Assert.Fail();
		}

		[TestMethod]
		public void IsFailWith() {
			AssertEx.IsFailWith<NullReferenceException>(ThrowNullReferenceException);
			
			try {
				AssertEx.IsFailWith<NullReferenceException>(ThrowException);
			} catch (AssertFailedException) {
				try {
					AssertEx.IsFailWith<NullReferenceException>(NotThrowException);
				} catch (AssertFailedException) {
					return;
				}
			}

			Assert.Fail();
		}

		[TestMethod]
		public void IsSameTest() {
			var seqN = default(int[]);
			var seqE = new int[0];
			var seq1 = new[] {4, 8, 15, 16, 23, 42};
			var seq2 = new[] {4, 8, 15, 16, 23};
			var seq3 = new[] {8, 15, 16, 23, 42};
			var seq4 = new[] {8, 15, 16, 23};

			seq1.IsSame(seq1);
			seq1.IsSame(seq1.ToArray());
			seqE.IsSame(seqE);
			AssertEx.IsFail(() => seq1.IsSame(seq2));
			AssertEx.IsFail(() => seq2.IsSame(seq1));
			AssertEx.IsFail(() => seq1.IsSame(seq3));
			AssertEx.IsFail(() => seq3.IsSame(seq1));
			AssertEx.IsFail(() => seq1.IsSame(seq4));
			AssertEx.IsFail(() => seq4.IsSame(seq1));
			AssertEx.IsFail(() => seq1.IsSame(seqN));
			AssertEx.IsFail(() => seq1.IsSame(seqE));
			AssertEx.IsFail(() => seqN.IsSame(seq1));
			AssertEx.IsFail(() => seqE.IsSame(seq1));
		}

		public void NotThrowException() { }

		public void ThrowNullReferenceException() => throw new NullReferenceException();

		public void ThrowException() => throw new Exception();
	}
}
