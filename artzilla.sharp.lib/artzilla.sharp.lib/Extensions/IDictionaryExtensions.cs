using System;
using System.Collections.Generic;

namespace ArtZilla.Net.Core.Extensions;

/// Extension methods for <see cref="IDictionary{TKey,TValue}"/>
public static class DictionaryExtensions {
#if !NET50_OR_GREATER && !NETCORE

	/// Gets the value associated with the specified key or default value if key not exist
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

#endif

	/// Gets the value associated with the specified key or create default value if key not exist (without dictionary change)
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	/// <param name="dictionary">Represents a generic collection of key/value pairs.</param>
	/// <param name="key">The key whose value to get</param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
	/// <exception cref="NullReferenceException" ><paramref name="dictionary"/> is null</exception>
	public static TValue GetValueOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
		=> dictionary.TryGetValue(key, out var value) ? value : new TValue();

	/// Gets the value associated with the specified key or default value if key not exist (without dictionary change)
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	/// <param name="dictionary">Represents a generic collection of key/value pairs.</param>
	/// <param name="key">The key whose value to get</param>
	/// <param name="createFunc"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
	/// <exception cref="NullReferenceException" ><paramref name="dictionary"/> is null</exception>
	public static TValue GetValueOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createFunc)
		=> dictionary.TryGetValue(key, out var value) ? value : createFunc();

	/// Gets the value associated with the specified key or default value if key not exist (without dictionary change)
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	/// <param name="dictionary">Represents a generic collection of key/value pairs.</param>
	/// <param name="key">The key whose value to get</param>
	/// <param name="createFunc"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
	/// <exception cref="NullReferenceException" ><paramref name="dictionary"/> is null</exception>
	public static TValue GetValueOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> createFunc)
		=> dictionary.TryGetValue(key, out var value) ? value : createFunc(key);

	/// Gets the value associated with the specified key or default value if key not exist (with dictionary change)
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	/// <param name="dictionary">Represents a generic collection of key/value pairs.</param>
	/// <param name="key">The key whose value to get</param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
	/// <exception cref="NullReferenceException" ><paramref name="dictionary"/> is null</exception>
	public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default) {
		if (dictionary.TryGetValue(key, out var value))
			return value;
		return dictionary[key] = defaultValue;
	}

	/// Gets the value associated with the specified key or default value if key not exist (with dictionary change)
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	/// <param name="dictionary">Represents a generic collection of key/value pairs.</param>
	/// <param name="key">The key whose value to get</param>
	/// <param name="createFunc"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
	/// <exception cref="NullReferenceException" ><paramref name="dictionary"/> is null</exception>
	public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createFunc) {
		if (dictionary.TryGetValue(key, out var value))
			return value;
		return dictionary[key] = createFunc();
	}

	/// Gets the value associated with the specified key or default value if key not exist (with dictionary change)
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	/// <param name="dictionary">Represents a generic collection of key/value pairs.</param>
	/// <param name="key">The key whose value to get</param>
	/// <param name="createFunc"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
	/// <exception cref="NullReferenceException" ><paramref name="dictionary"/> is null</exception>
	public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> createFunc) {
		if (dictionary.TryGetValue(key, out var value))
			return value;
		return dictionary[key] = createFunc(key);
	}

	/// Gets the value associated with the specified key or default value if key not exist (with dictionary change)
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	/// <param name="dictionary">Represents a generic collection of key/value pairs.</param>
	/// <param name="key">The key whose value to get</param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> is null</exception>
	/// <exception cref="NullReferenceException" ><paramref name="dictionary"/> is null</exception>
	public static TValue GetValueOrAddNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new() {
		if (dictionary.TryGetValue(key, out var value))
			return value;
		return dictionary[key] = new();
	}
}
