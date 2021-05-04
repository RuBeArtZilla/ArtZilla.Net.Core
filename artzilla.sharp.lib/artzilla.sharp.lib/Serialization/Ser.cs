using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ArtZilla.Net.Core.Serialization {
	/// <summary> </summary>
	public interface ISerializer {
		/// <summary> </summary>
		void Serialize(Stream stream, object o);
		
		/// <summary> </summary>
		object Deserialize(Stream stream);
		
		/// <summary> </summary>
		bool TryDeserialize(Stream stream, out object o);
	}
	
	/// <summary> </summary>
	public interface ISerializer<T> : ISerializer {
		/// <summary> </summary>
		void SerializeTo(Stream stream, T o);
		
		/// <summary> </summary>
		T DeserializeFrom(Stream stream);
		
		/// <summary> </summary>
		bool TryDeserializeFrom(Stream stream, out T o);
	}
	
	/// <summary> </summary>
	public interface IStringSerializer : ISerializer {
		/// <summary> </summary>
		string SerializeToString(object o);
		
		/// <summary> </summary>
		object Deserialize(string source);
		
		/// <summary> </summary>
		bool TryDeserialize(string source, out object o);
	}
	
	/// <summary> </summary>
	public interface IStringSerializer<T> : ISerializer<T>, IStringSerializer {
		/// <summary> </summary>
		string SerializeToString(T o);
		
		/// <summary> </summary>
		T DeserializeFrom(string source);
		
		/// <summary> </summary>
		bool TryDeserializeFrom(string source, out T o);
	}
	
	/// <summary> </summary>
	public interface IXmlSerializer : ISerializer {
		/// <summary> </summary>
		XmlDocument SerializeToXml(object o);
		
		/// <summary> </summary>
		void Serialize(XmlWriter xmlWriter, object o);
		
		/// <summary> </summary>
		object Deserialize(XmlDocument xml);
		
		/// <summary> </summary>
		bool TryDeserialize(XmlDocument xml, out object o);
		
		/// <summary> </summary>
		object Deserialize(XmlReader xmlReader);
		
		/// <summary> </summary>
		bool TryDeserialize(XmlReader xmlReader, out object o);
	}
	
	/// <summary> </summary>
	public interface IXmlSerializer<T> : ISerializer<T>, IXmlSerializer {
		/// <summary> </summary>
		XmlDocument SerializeToXml(T o);
		
		/// <summary> </summary>
		void SerializeTo(XmlWriter xmlWriter, T o);
		
		/// <summary> </summary>
		T DeserializeFrom(XmlDocument xml);
		
		/// <summary> </summary>
		bool TryDeserializeFrom(XmlDocument xml, out T o);
		
		/// <summary> </summary>
		T DeserializeFrom(XmlReader xmlReader);
		
		/// <summary> </summary>
		bool TryDeserializeFrom(XmlReader xmlReader, out T o);
	}
	
	/// <summary> </summary>
	public class XmlSrlzr : IXmlSerializer {
		/// <summary> </summary>
		public XmlSrlzr(Type type)
			=> _serializer = new XmlSerializer(type);

		/// <inheritdoc />
		public void Serialize(Stream stream, object o)
			=> _serializer.Serialize(stream, o);

		/// <inheritdoc />
		public object Deserialize(Stream stream)
			=> _serializer.Deserialize(stream);

		/// <inheritdoc />
		public bool TryDeserialize(Stream stream, out object o)
			=> _serializer.TryDo(s => s.Deserialize(stream), out o);

		/// <inheritdoc />
		public XmlDocument SerializeToXml(object o)
			=> throw new NotImplementedException();

		/// <inheritdoc />
		public void Serialize(XmlWriter xmlWriter, object o)
			=> _serializer.Serialize(xmlWriter, o);

		/// <inheritdoc />
		public object Deserialize(XmlDocument xml)
			=> throw new NotImplementedException();

		/// <inheritdoc />
		public bool TryDeserialize(XmlDocument xml, out object o)
			=> throw new NotImplementedException();

		/// <inheritdoc />
		public object Deserialize(XmlReader xmlReader)
			=> _serializer.Deserialize(xmlReader);

		/// <inheritdoc />
		public bool TryDeserialize(XmlReader xmlReader, out object o)
			=> _serializer.TryDo(s => s.Deserialize(xmlReader), out o);

		private readonly XmlSerializer _serializer;
	}
	
	/// <summary> </summary>
	public sealed class XmlSrlzr<T> : XmlSrlzr, IXmlSerializer<T> {
		/// <summary> </summary>
		public XmlSrlzr() : base(typeof(T)) { }

		/// <inheritdoc />
		public void SerializeTo(Stream stream, T o)
			=> Serialize(stream, o: o);

		/// <inheritdoc />
		public T DeserializeFrom(Stream stream)
			=> (T) Deserialize(stream);

		/// <inheritdoc />
		public bool TryDeserializeFrom(Stream stream, out T o) {
			if (TryDeserialize(stream, out var x)) {
				o = (T) x;
				return true;
			}

			o = default;
			return false;
		}

		/// <inheritdoc />
		public XmlDocument SerializeToXml(T o)
			=> base.SerializeToXml(o);

		/// <inheritdoc />
		public void SerializeTo(XmlWriter xmlWriter, T o)
			=> Serialize(xmlWriter, o);

		/// <inheritdoc />
		public T DeserializeFrom(XmlDocument xml)
			=> (T) Deserialize(xml);

		/// <inheritdoc />
		public bool TryDeserializeFrom(XmlDocument xml, out T o) {
			if (TryDeserialize(xml, out var x)) {
				o = (T) x;
				return true;
			}

			o = default;
			return false;
		}

		/// <inheritdoc />
		public T DeserializeFrom(XmlReader xmlReader)
			=> (T) Deserialize(xmlReader);

		/// <inheritdoc />
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