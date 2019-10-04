using System;
using System.Linq;
using System.Reflection;

namespace ArtZilla.Net.Core.Serialization {
	internal static class ReflectionExt {
		public static bool HasAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute 
			=> propertyInfo.GetCustomAttr<TAttribute>() != null;

		public static TAttribute GetCustomAttr<TAttribute>(this MemberInfo element) where TAttribute : Attribute
			=> (TAttribute) element.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();
	}

	/*
	internal class SpecialSerializer : IXmlSerializer, IStringSerializer, ISerializer {
		private string _className;
		private string _nsName;

		public bool FormatOutputXml { get; set; }

		public SpecialSerializer(Type type) : this(type, type) { }
		public SpecialSerializer(Type baseType, Type instType) {
			InType = baseType;
			OutType = instType;

			var members = new List<Member>();
			
			var iprops = baseType.GetProperties().Where(p => p.GetCustomAttr<XmlIgnoreAttribute>() is null).ToArray();
			var ifields = baseType.GetFields().Where(p => p.GetCustomAttr<XmlIgnoreAttribute>() is null).ToArray();

			members.AddRange(iprops.Select(p => {
				var attr = p.GetCustomAttr<XmlAttributeAttribute>();
				return new Member() {
					IsAttribute = attr != null,
					IsProperty = true,
					PropName = p.Name,
					TargName = attr?.AttributeName ?? p.Name,
				};
			}));

			Members = members.ToArray();
		}

		public Type InType { get; }
		public Type OutType { get; }

		public string SerializeToString(object o) => throw new NotImplementedException();

		public object Deserialize(string source) {
			var xml = new XmlDocument();
			xml.LoadXml(source);
			return Deserialize(xml);
		}

		public bool TryDeserialize(string source, out object o) => throw new NotImplementedException();

		public void Serialize(Stream stream, object o) => throw new NotImplementedException();

		public object Deserialize(Stream stream) => throw new NotImplementedException();

		public bool TryDeserialize(Stream stream, out object o) => throw new NotImplementedException();

		public XmlDocument SerializeToXml(object o) => throw new NotImplementedException();

		public void Serialize(XmlWriter xmlWriter, object o) => throw new NotImplementedException();

		public object Deserialize(XmlDocument xml) {
			var name = xml.Name;
			var child = xml.FirstChild;

			return null;
		}

		public bool TryDeserialize(XmlDocument xml, out object o) => throw new NotImplementedException();

		public object Deserialize(XmlReader xmlReader) => throw new NotImplementedException();

		public bool TryDeserialize(XmlReader xmlReader, out object o) => throw new NotImplementedException();

		protected class Member {
			public string PropName;
			public string TargName;

			public bool IsAttribute;
			public bool IsProperty;
		}

		protected readonly Member[] Members;
	}

	internal sealed class SpecialSerializer<T> : SpecialSerializer, IXmlSerializer<T>, IStringSerializer<T>, ISerializer<T> {
		public SpecialSerializer(Type type) : base(type) { }
		public SpecialSerializer(Type baseType, Type instType) : base(baseType, instType) { }
		public string SerializeToString(T o) => throw new NotImplementedException();

		public T DeserializeFrom(string source) => throw new NotImplementedException();

		public bool TryDeserializeFrom(string source, out T o) => throw new NotImplementedException();

		public void SerializeTo(Stream stream, T o) => throw new NotImplementedException();

		public T DeserializeFrom(Stream stream) => throw new NotImplementedException();

		public bool TryDeserializeFrom(Stream stream, out T o) => throw new NotImplementedException();

		public XmlDocument SerializeToXml(T o) => throw new NotImplementedException();

		public void SerializeTo(XmlWriter xmlWriter, T o) => throw new NotImplementedException();

		public T DeserializeFrom(XmlDocument xml) => throw new NotImplementedException();

		public bool TryDeserializeFrom(XmlDocument xml, out T o) => throw new NotImplementedException();

		public T DeserializeFrom(XmlReader xmlReader) => throw new NotImplementedException();

		public bool TryDeserializeFrom(XmlReader xmlReader, out T o) => throw new NotImplementedException();
	}*/
}