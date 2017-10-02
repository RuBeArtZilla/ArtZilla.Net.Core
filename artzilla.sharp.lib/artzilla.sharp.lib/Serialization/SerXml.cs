using System;
using System.IO;
using System.Xml.Serialization;
using ArtZilla.Net.Core.Extensions;

namespace ArtZilla.Net.Core.Serialization {
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
			} catch {
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
			} catch {
				// ignored
			}

			return res;
		}

		/// <summary>
		/// Object serialization to file as XML
		/// </summary>
		/// <param name="file">Path to file</param>
		/// <param name="item">Object to serialize</param>
		/// <param name="append">Adding to end of file if true</param>
		/// <returns>True if success</returns>
		public static bool Save(string file, object item, bool append = false) {
			var serializator = new XmlSerializer(item.GetType());

			CreateIfNotExist(file);

			try {
				using (var fs = new FileStream(file, append ? FileMode.Append : FileMode.Create))
					serializator.Serialize(fs, item);

				return true;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Object serialization to file as XML
		/// </summary>
		/// <param name="file">Path to file</param>
		/// <param name="item">Object to serialize</param>
		/// <param name="append">Adding to end of file if true</param>
		/// <returns>True if success</returns>
		public static bool Save<T>(string file, T item, bool append = false) where T : class {
			var serializator = new XmlSerializer(typeof(T));

			CreateIfNotExist(file);

			try {
				using (var fs = new FileStream(file, append ? FileMode.Append : FileMode.Create))
					serializator.Serialize(fs, item);

				return true;
			} catch {
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
			} catch {
				// ignored
			}
		}
	}
}
