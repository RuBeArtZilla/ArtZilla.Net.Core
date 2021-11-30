using System;
using System.Collections.Generic;
using System.Linq;
using ArtZilla.Net.Core.Extensions;
using ArtZilla.Net.Core.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions;

[TestClass]
public class IEnumerableExtTests {
	private const IEnumerable<object> NullSequence = null;
	private const IEnumerable<object> EmptySequence = null;

	[TestMethod]
	public void DoBeforeTest() {
		var arr = new[] { 4, 8, 15, 16, 23, 42 };
		var i = 0;
		void action(int j) => ++i;
		arr.DoBefore(action).ToArray();
		Assert.IsTrue(arr.Length == i, $"Wanted={arr.Length}, Actual={i}");

		AssertEx.IsFailWith<ArgumentNullException>(() => NullSequence.DoBefore(o => { }));
		AssertEx.IsFailWith<ArgumentNullException>(() => arr.DoBefore(null));
	}

	[TestMethod]
	public void AppendTest() {
		var ideal = new[] { 4, 8, 15, 16, 23, 42 };
		var test1 = new[] { 4, 8, 15, 16, 23 };
		var test2 = new[] { 4, 8, 15, 16 };
		var test3 = new[] { 4, 8, 15 };

		AssertSameEnumerables(ideal, ideal.Append());
		AssertSameEnumerables(ideal, test1.Append(42));
		AssertSameEnumerables(ideal, test2.Append(23, 42));
		AssertSameEnumerables(ideal, test3.Append(16, 23, 42));

		AssertEx.IsFailWith<ArgumentNullException>(() => NullSequence.Append());
		AssertEx.IsFailWith<ArgumentNullException>(() => NullSequence.Append(new object()));
		AssertEx.IsFailWith<ArgumentNullException>(() => NullSequence.Append(new object(), new object()));
		AssertEx.IsFailWith<ArgumentNullException>(() => NullSequence.Append(new object(), new object(), new object()));
		AssertEx.IsFailWith<ArgumentNullException>(() => EmptySequence.Append(null));
		AssertEx.IsFailWith<ArgumentNullException>(() => EmptySequence.Append(new object(), null));
		AssertEx.IsFailWith<ArgumentNullException>(() => EmptySequence.Append(null, new object()));
		AssertEx.IsFailWith<ArgumentNullException>(() => EmptySequence.Append(null, null));
		AssertEx.IsFailWith<ArgumentNullException>(() => EmptySequence.Append(null, null, null));
		AssertEx.IsFailWith<ArgumentNullException>(() => EmptySequence.Append(new object(), new object(), null));
	}

	[TestMethod]
	public void CombineTest() {
		Assert.AreEqual("4 8 15 16 23 42", new[] { 4, 8, 15, 16, 23, 42 }.Combine(" "));
		Assert.AreEqual("", new[] { null, "", "   ", null }.Combine());

		AssertEx.IsFailWith<ArgumentNullException>(() => NullSequence.Combine());
	}

	[TestMethod]
	public void BufferizeTest() {
		var sequence = new[] { 4, 8, 15, 16, 23, 42 };
		var output1 = sequence.Split(4).ToArray();
		Assert.AreEqual(2, output1.Length);
		Assert.AreEqual(4, output1[0].Length);
		Assert.AreEqual(2, output1[1].Length);
		AssertSameEnumerables(output1[0], new[] { 4, 8, 15, 16 });
		AssertSameEnumerables(output1[1], new[] { 23, 42 });

		var output2 = sequence.Split(3).ToArray();
		Assert.AreEqual(2, output2.Length);
		Assert.AreEqual(3, output2[0].Length);
		Assert.AreEqual(3, output2[1].Length);
		AssertSameEnumerables(output2[0], new[] { 4, 8, 15 });
		AssertSameEnumerables(output2[1], new[] { 16, 23, 42 });

		var output3 = sequence.Split(sequence.Length + 42).ToArray();
		Assert.AreEqual(1, output3.Length);
		AssertSameEnumerables(output3[0], sequence);
		Assert.IsTrue(ReferenceEquals(sequence, output3[0]));

		var output4 = sequence.ToList().Split(sequence.Length + 42).ToArray();
		Assert.AreEqual(1, output4.Length);
		AssertSameEnumerables(output4[0], sequence);

		AssertEx.IsFailWith<ArgumentOutOfRangeException>(() => sequence.Split(0));
	}

	public static void AssertSameEnumerables<T>(IEnumerable<T> first, IEnumerable<T> second) {
		var i1 = first.GetEnumerator();
		var i2 = second.GetEnumerator();
		while (true) {
			var hasNext = i1.MoveNext();
			Assert.AreEqual(hasNext, i2.MoveNext(), "sequences contain different count of elements");
			if (!hasNext)
				return;
			Assert.AreEqual(i1.Current, i2.Current);
		}
	}
}

