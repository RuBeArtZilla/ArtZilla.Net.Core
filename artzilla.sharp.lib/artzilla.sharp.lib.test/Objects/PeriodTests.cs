using System;
using ArtZilla.Net.Core.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Objects;

[TestClass]
public class PeriodTests {
	[TestMethod]
	public void CtorTest() {
		var dt1 = new DateTime(2001, 1, 1);
		var dt2 = new DateTime(2001, 1, 2);
		var period1 = new Period(dt1, dt2);
		Assert.AreEqual(period1.Begin, dt1);
		Assert.AreEqual(period1.End, dt2);

		var period2 = new Period(dt2, dt1);
		Assert.AreEqual(period1, period2);
		Assert.AreEqual(period1.Begin, period2.Begin);
		Assert.AreEqual(period1.End, period2.End);

		var period3 = new Period(dt1, dt2 - dt1);
		Assert.AreEqual(period1, period3);
		Assert.AreEqual(period1.Begin, period3.Begin);
		Assert.AreEqual(period1.End, period3.End);
	}

	public void MoveTest() {
		var dt1 = new DateTime(2001, 1, 1);
		var dt2 = new DateTime(2001, 1, 2);
		var period1 = new Period(dt1, dt2);
		var period2 = period1.MoveDays(1D);
		Assert.AreEqual(period1.Begin.AddDays(1), period1.Begin);
		Assert.AreEqual(period1.Begin.AddDays(1), period1.Begin);
	}
}
