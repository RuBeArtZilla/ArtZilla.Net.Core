using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ArtZilla.Net.Core.Extensions;

namespace ArtZilla.Net.Core.Serialization;

/// <summary> </summary>
[Obsolete]
public static class SerBi {
	/// <summary> </summary>
	public static List<T> Load<T>(string file, bool clear = false) where T : class {
		var serializator = new BinaryFormatter();
		var res = new List<T>();

		try {
			if (!File.Exists(file))
				return res;

			using var fs = new FileStream(file, FileMode.Open);
			if (fs.Length == 0) return res;

			do {
				if (serializator.Deserialize(fs) is T o)
					res.Add(o);
			} while (fs.Position < fs.Length);

			if (clear) fs.SetLength(0);
		} catch {
			// ignored
		}

		return res;
	}

	/// <summary> </summary>
	public static bool Save<T>(string file, IEnumerable<T> items, bool append = false) where T : class {
		var serializator = new BinaryFormatter();
		CreateIfNotExist(file);

		try {
			using var fs = new FileStream(file, append ? FileMode.Append : FileMode.Create);
			foreach (var item in items)
				serializator.Serialize(fs, item);

			return true;
		} catch {
			return false;
		}
	}

	/// <summary> </summary>
	public static bool Save<T>(string file, T item, bool append = false) where T : class {
		var serializator = new BinaryFormatter();
		CreateIfNotExist(file);

		try {
			using var fs = new FileStream(file, append ? FileMode.Append : FileMode.Create);
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
