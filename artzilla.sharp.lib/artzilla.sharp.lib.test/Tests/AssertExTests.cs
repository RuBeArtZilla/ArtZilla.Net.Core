using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Sharp.Lib.Test.Tests {
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

			throw new AssertFailedException();
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

			throw new AssertFailedException();
		}

		public void NotThrowException() { }

		public void ThrowNullReferenceException() {
			throw new NullReferenceException();
		}

		public void ThrowException() {
			throw new Exception();
		}
	}
}
