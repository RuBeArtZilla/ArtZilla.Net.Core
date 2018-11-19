using System;
using System.IO;
using System.Xml.Serialization;
using ArtZilla.Net.Core.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtZilla.Net.Core.Tests.Serialization {
	public static class TestUtils {
		public const string SampleString = "<!--<\"'Hello World\"'>-->";
		public const int SampleNumber = 42;

		public static void TestSerializer<TSerializer>() where TSerializer : ISerializer {
			
		}

		public static void TestOnMemoryStream<TSerializer>(this TSerializer serializer) where TSerializer : ISerializer {
			var src = CreateTestClass();
			using (var stream = new MemoryStream()) {
				serializer.Serialize(stream, src);

				stream.Position = 0;
				Assert.IsTrue(serializer.TryDeserialize(stream, out var o));
				Assert.IsInstanceOfType(o, typeof(TestClass));
				var dst = (TestClass) o;
				AssertSame(src, dst);

				stream.Position = 0;
				o = serializer.Deserialize(stream);
				Assert.IsInstanceOfType(o, typeof(TestClass));
				dst = (TestClass) o;
				AssertSame(src, dst);
			}
		}

		public static void TestConvertable(this ISerializer<TestClass> classSrlzr, ISerializer<TestStruct> structSrlzr) {
			{
				var src = CreateTestClass();
				using (var stream = new MemoryStream()) {
					classSrlzr.SerializeTo(stream, src);
					stream.Position = 0;

					var dst = structSrlzr.DeserializeFrom(stream);
					AssertSame(src, dst);
				}
			}

			{
				var src = CreateTestStruct();
				using (var stream = new MemoryStream()) {
					structSrlzr.SerializeTo(stream, src);
					stream.Position = 0;

					var dst = classSrlzr.DeserializeFrom(stream);
					AssertSame(src, dst);
				}
			}
		}

		public static void AssertSame(TestClass expected, TestClass actual) {
			Assert.AreEqual(expected.Nickname, actual.Nickname);
			Assert.AreEqual(expected.Age, actual.Age);
		}

		public static TestClass CreateTestClass()
			=> new TestClass() {
				Nickname = SampleString,
				Age = SampleNumber
			};

		public static TestStruct CreateTestStruct()
			=> new TestStruct() {
				Nickname = SampleString,
				Age = SampleNumber
			};
	}

	[Serializable]
	public class TestItem {
		public string StrProperty { get; set; }
		public string StrField;

		private string ProtectedProperty { get; set; }
		private string ProtectedField;

		private string PrivateProperty { get; set; }
		private string PrivateField;
	}

	[Serializable, XmlRoot("Developer")]
	public struct TestStruct {
		public string Nickname;
		public int Age;

		public static implicit operator TestClass(TestStruct source)
			=> new TestClass() { Nickname = source.Nickname, Age = source.Age };
	}

	[Serializable, XmlRoot("Developer")]
	public class TestClass {
		public string Nickname { get; set; }
		public int Age { get; set; }
	}

	[TestClass]
	public class XmlSrlzrTests {
		[TestMethod]
		public void SimpleTests() {
			var s = new XmlSrlzr(typeof(TestClass));
			s.TestOnMemoryStream();

			var cs = new XmlSrlzr<TestClass>();
			var ss = new XmlSrlzr<TestStruct>();
			cs.TestConvertable(ss);
		}
	}
}
