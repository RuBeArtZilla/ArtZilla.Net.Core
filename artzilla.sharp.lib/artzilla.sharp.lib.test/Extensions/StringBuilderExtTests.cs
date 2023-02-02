using System.Text;
using ArtZilla.Net.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Extensions;

[TestClass]
public class StringBuilderExtTests {
	[TestMethod]
	public void AppendListTest() {
		int[] list = { 4, 8, 15, 16, 23, 42 };
		StringBuilder sb = new("{");
		
		var expected = "{ 4, 8, 15, 16, 23, 42 }";
		var actual = sb.AppendList(list, ", ", " ", " }").ToString();
		Assert.AreEqual(expected, actual);

		actual = sb.Clear().AppendList(list).ToString();
		expected = "4815162342";
		Assert.AreEqual(expected, actual);
	}
}