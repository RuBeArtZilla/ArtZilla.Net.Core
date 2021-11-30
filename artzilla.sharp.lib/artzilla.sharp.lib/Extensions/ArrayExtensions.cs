using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtZilla.Net.Core.Extensions;

/// <summary> Extensions for arrays </summary>
public static class ArrayExtensions {
	/// <summary> Check whether the specified array is <see langword="null" /> or an empty </summary>
	/// <typeparam name="T">type of elements in array</typeparam>
	/// <param name="array">array to check</param>
	/// <returns>Return <see langword="true" /> if <paramref name="array"/> is <see langword="null" /> or empty otherwise return <see langword="false" /></returns>
	public static bool IsNullOrEmpty<T>(this T[] array) => array is null || array.Length == 0;

	/// <summary> Check whether the specified array is not <see langword="null" /> and not empty </summary>
	/// <typeparam name="T">type of elements in array</typeparam>
	/// <param name="array">array to check</param>
	/// <returns>Return <see langword="false" /> if <paramref name="array"/> is <see langword="null" /> or empty otherwise return <see langword="true" /></returns>
	public static bool IsNotNullOrEmpty<T>(this T[] array) => array?.Length > 0;
}
