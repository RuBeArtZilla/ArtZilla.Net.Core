using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtZilla.Net.Core.Extensions {
	public static class IDictionaryExtensions {
		/// <summary>
		/// Gets the value associated with the specified key or default value if key not exist
		/// </summary>
		/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
		/// <param name="dictionary">Represents a generic collection of key/value pairs.</param>
		/// <param name="key">The key whose value to get</param>
		/// <param name="defaultValue">The default value for the type of the value parameter if <paramref name="key"/> not exist</param>
		/// <returns>Value if <paramref name="dictionary"/> contains an element with the specified <paramref name="key"/>; otherwise <paramref name="defaultValue"/></returns>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
		/// <exception cref="NullReferenceException" ><paramref name="dictionary"/> is null</exception>
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
			=> dictionary.TryGetValue(key, out var value) ? value : defaultValue;
	}
}
