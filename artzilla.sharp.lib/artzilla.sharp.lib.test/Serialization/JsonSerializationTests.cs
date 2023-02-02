#if NET50_OR_GREATER

using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace ArtZilla.Net.Core.Tests.Serialization;

[TestClass]
public class JsonSerializationTests {
	[TestMethod]
	public void PeriodTest() {
		var expected = Period.PreviousMonth().MoveDays(15);
		var json = JsonSerializer.Serialize(expected);
		var actual = JsonSerializer.Deserialize<Period>(json);
		Debug.WriteLine("{0} => {1} => {2}", expected, json, actual);
	}
}

#endif