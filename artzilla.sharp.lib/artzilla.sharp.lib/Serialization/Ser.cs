using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ArtZilla.Net.Core.Extensions;

namespace ArtZilla.Net.Core.Serialization {
	public interface ISerializer {
		void Serialize(Stream stream, object o);

		object Deserialize(Stream stream);

		bool TryDeserialize(Stream stream, out object o);
	}

	public interface ISerializer<T> : ISerializer {
		void SerializeTo(Stream stream, T o);

		T DeserializeFrom(Stream stream);

		bool TryDeserializeFrom(Stream stream, out T o);
	}

	public interface IStringSerializer : ISerializer {
		string SerializeToString(object o);

		object Deserialize(string source);

		bool TryDeserialize(string source, out object o);
	}

	public interface IStringSerializer<T> : ISerializer<T>, IStringSerializer {
		string SerializeToString(T o);

		T DeserializeFrom(string source);

		bool TryDeserializeFrom(string source, out T o);
	}

	public interface IXmlSerializer : ISerializer {
		XmlDocument SerializeToXml(object o);

		void Serialize(XmlWriter xmlWriter, object o);

		object Deserialize(XmlDocument xml);

		bool TryDeserialize(XmlDocument xml, out object o);

		object Deserialize(XmlReader xmlReader);

		bool TryDeserialize(XmlReader xmlReader, out object o);
	}

	public interface IXmlSerializer<T> : ISerializer<T>, IXmlSerializer {
		XmlDocument SerializeToXml(T o);

		void SerializeTo(XmlWriter xmlWriter, T o);

		T DeserializeFrom(XmlDocument xml);

		bool TryDeserializeFrom(XmlDocument xml, out T o);

		T DeserializeFrom(XmlReader xmlReader);

		bool TryDeserializeFrom(XmlReader xmlReader, out T o);
	}

	public class XmlSrlzr : IXmlSerializer {
		public XmlSrlzr(Type type)
			=> _serializer = new XmlSerializer(type);

		public void Serialize(Stream stream, object o)
			=> _serializer.Serialize(stream, o);

		public object Deserialize(Stream stream)
			=> _serializer.Deserialize(stream);

		public bool TryDeserialize(Stream stream, out object o)
			=> _serializer.TryDo(s => s.Deserialize(stream), out o);

		public XmlDocument SerializeToXml(object o)
			=> throw new NotImplementedException();

		public void Serialize(XmlWriter xmlWriter, object o)
			=> _serializer.Serialize(xmlWriter, o);

		public object Deserialize(XmlDocument xml)
			=> throw new NotImplementedException();

		public bool TryDeserialize(XmlDocument xml, out object o)
			=> throw new NotImplementedException();

		public object Deserialize(XmlReader xmlReader)
			=> _serializer.Deserialize(xmlReader);

		public bool TryDeserialize(XmlReader xmlReader, out object o)
			=> _serializer.TryDo(s => s.Deserialize(xmlReader), out o);

		private readonly XmlSerializer _serializer;
	}

	public sealed class XmlSrlzr<T> : XmlSrlzr, IXmlSerializer<T> {
		public XmlSrlzr() : base(typeof(T)) { }

		public void SerializeTo(Stream stream, T o)
			=> Serialize(stream, o: o);

		public T DeserializeFrom(Stream stream)
			=> (T) Deserialize(stream);

		public bool TryDeserializeFrom(Stream stream, out T o) {
			if (TryDeserialize(stream, out var x)) {
				o = (T) x;
				return true;
			}

			o = default;
			return false;
		}

		public XmlDocument SerializeToXml(T o)
			=> base.SerializeToXml(o);

		public void SerializeTo(XmlWriter xmlWriter, T o)
			=> Serialize(xmlWriter, o);

		public T DeserializeFrom(XmlDocument xml)
			=> (T) Deserialize(xml);

		public bool TryDeserializeFrom(XmlDocument xml, out T o) {
			if (TryDeserialize(xml, out var x)) {
				o = (T) x;
				return true;
			}

			o = default;
			return false;
		}

		public T DeserializeFrom(XmlReader xmlReader)
			=> (T) Deserialize(xmlReader);

		public bool TryDeserializeFrom(XmlReader xmlReader, out T o) {
			if (TryDeserialize(xmlReader, out var x)) {
				o = (T) x;
				return true;
			}

			o = default;
			return false;
		}
	}
}