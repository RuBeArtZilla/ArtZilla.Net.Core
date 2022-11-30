using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ArtZilla.Net.Core.Serialization;

/// 
public interface ISerializer {
	/// 
	void Serialize(Stream stream, object o);

	/// 
	object Deserialize(Stream stream);

	/// 
	bool TryDeserialize(Stream stream, out object o);
}

/// 
public interface ISerializer<T> : ISerializer {
	/// 
	void SerializeTo(Stream stream, T o);

	/// 
	T DeserializeFrom(Stream stream);

	/// 
	bool TryDeserializeFrom(Stream stream, out T o);
}

/// 
public interface IStringSerializer : ISerializer {
	/// 
	string SerializeToString(object o);

	/// 
	object Deserialize(string source);

	/// 
	bool TryDeserialize(string source, out object o);
}

/// 
public interface IStringSerializer<T> : ISerializer<T>, IStringSerializer {
	/// 
	string SerializeToString(T o);

	/// 
	T DeserializeFrom(string source);

	/// 
	bool TryDeserializeFrom(string source, out T o);
}

/// 
public interface IXmlSerializer : ISerializer {
	/// 
	XmlDocument SerializeToXml(object o);

	/// 
	void Serialize(XmlWriter xmlWriter, object o);

	/// 
	object Deserialize(XmlDocument xml);

	/// 
	bool TryDeserialize(XmlDocument xml, out object o);

	/// 
	object Deserialize(XmlReader xmlReader);

	/// 
	bool TryDeserialize(XmlReader xmlReader, out object o);
}

/// 
public interface IXmlSerializer<T> : ISerializer<T>, IXmlSerializer {
	/// 
	XmlDocument SerializeToXml(T o);

	/// 
	void SerializeTo(XmlWriter xmlWriter, T o);

	/// 
	T DeserializeFrom(XmlDocument xml);

	/// 
	bool TryDeserializeFrom(XmlDocument xml, out T o);

	/// 
	T DeserializeFrom(XmlReader xmlReader);

	/// 
	bool TryDeserializeFrom(XmlReader xmlReader, out T o);
}

/// 
public class XmlSrlzr : IXmlSerializer {
	/// 
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

/// 
public sealed class XmlSrlzr<T> : XmlSrlzr, IXmlSerializer<T> {
	/// 
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
