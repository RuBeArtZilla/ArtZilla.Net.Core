using System.Xml.Serialization;
using ArtZilla.Net.Core.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Serialization;

[TestClass]
public class SpecialSerializerTests {
	private interface IWaifu {
		[XmlAttribute]
		string Name { get; set; }
		int Charm { get; set; }
	}

	private class Waifu : IWaifu {
		public string Name { get; set; }
		public int Charm { get; set; }
	}

	private const string HomuraWaifuText = "<?xml version=\"1.0\"?><Waifu Name=\"Homura\"><Charm>9999</Charm></Waifu>";

	/* not yet ready
	[TestMethod]
	public void FromStringTest() {
		var ser = new SpecialSerializer(typeof(IWaifu), typeof(Waifu));
		var waifu = ser.Deserialize(HomuraWaifuText) as IWaifu;

		Assert.IsNotNull(waifu, "deserialization from string failed");
		Assert.AreEqual("Homura", waifu.Name);
		Assert.AreEqual(9999, waifu.Charm);

		var text = ser.SerializeToString(waifu);
		Assert.AreEqual(HomuraWaifuText, text);
	}*/
}
