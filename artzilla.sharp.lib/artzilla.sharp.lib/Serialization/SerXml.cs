using System;
using System.IO;
using System.Xml.Serialization;
using ArtZilla.Sharp.Lib.Extensions;

namespace ArtZilla.Sharp.Lib.Serialization {
	public static class SerXml {
		public static object Load(Type type, string file, bool clear = false) {
			// Будет исключение, если тип нельзя сериализовать в XML
			var serializator = new XmlSerializer(type);
			object res = null;

			try {
				if (!File.Exists(file))
					return null;

				using (var fs = new FileStream(file, FileMode.Open)) {
					if (fs.Length == 0)
						return null;

					res = serializator.Deserialize(fs);

					if (clear)
						fs.SetLength(0);
				}
			} catch (Exception ex) {
				// ignored
			}

			return res;
		}

		public static T Load<T>(string file, bool clear = false) where T : class {
			// Будет исключение, если тип нельзя сериализовать в XML
			var serializator = new XmlSerializer(typeof(T));

			T res = null;

			try {
				if (!File.Exists(file))
					return null;

				using (var fs = new FileStream(file, FileMode.Open)) {
					if (fs.Length == 0)
						return null;

					res = serializator.Deserialize(fs) as T;

					if (clear)
						fs.SetLength(0);
				}
			} catch (Exception ex) {
				// ignored
			}

			return res;
		}

		public static bool Save(string file, object item, bool append = false) {
			// Будет исключение, если тип нельзя сериализовать в XML
			var serializator = new XmlSerializer(item.GetType());

			CreateIfNotExist(file);

			try {
				using (var fs = new FileStream(file, append ? FileMode.Append : FileMode.Create))
					serializator.Serialize(fs, item);

				return true;
			} catch (Exception ex) {
				return false;
			}
		}

		public static bool Save<T>(string file, T item, bool append = false) where T : class {
			// Будет исключение, если тип нельзя сериализовать в XML
			var serializator = new XmlSerializer(typeof(T));

			CreateIfNotExist(file);

			try {
				using (var fs = new FileStream(file, append ? FileMode.Append : FileMode.Create))
					serializator.Serialize(fs, item);

				return true;
			} catch (Exception ex) {
				return false;
			}
		}

		private static void CreateIfNotExist(string file) {
			try {
				if (File.Exists(file))
					return;

				var path = Path.GetDirectoryName(file);
				if (path.IsBad())
					return;

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
			} catch (Exception ex) {
				// ignored
			}
		}
	}
}
