using System;

namespace ArtZilla.Net.Core.Extensions;

/// Common methods for validating arguments
public static class Guard {
	/// Check that parameter is null otherwise throw exception
	/// <param name="param"></param>
	/// <exception cref="ArgumentNullException">Thrown when parameter not null</exception>
	public static void Null<T>(T param) {
		if (param != null)
			throw new ArgumentNullException();
	}

	/// Check that parameter is null otherwise throw exception
	/// <param name="param"></param>
	/// <param name="paramName"></param>
	/// <exception cref="ArgumentNullException">Thrown when parameter not null</exception>
	public static void Null<T>(T param, string paramName) {
		if (param != null)
			throw new ArgumentNullException(paramName);
	}

	/// Check that parameter is not null otherwise throw exception
	/// <param name="param"></param>
	/// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
	public static void NotNull<T>(T param) {
		if (param == null)
			throw new ArgumentNullException();
	}

	/// Check that parameter is not null otherwise throw exception
	/// <param name="param"></param>
	/// <param name="paramName"></param>
	/// <exception cref="ArgumentNullException">Thrown when parameter is null</exception>
	public static void NotNull<T>(T param, string paramName) {
		if (param == null)
			throw new ArgumentNullException(paramName);
	}

	/// Check that array contain any item
	/// <param name="param"></param>
	/// <exception cref="ArgumentException">Thrown when string is empty</exception>
	/// <exception cref="ArgumentNullException">Thrown when string is null</exception>
	public static void NotNullOrEmpty(string param) {
		if (param == null)
			throw new ArgumentNullException();
		if (param.Length == 0)
			throw new ArgumentException();
	}

	/// Check that array contain any item
	/// <param name="param"></param>
	/// <param name="paramName"></param>
	/// <exception cref="ArgumentException">Thrown when string is empty</exception>
	/// <exception cref="ArgumentNullException">Thrown when string is null</exception>
	public static void NotNullOrEmpty(string param, string paramName) {
		if (param == null)
			throw new ArgumentNullException(paramName);
		if (param.Length == 0)
			throw new ArgumentException("Zero Length", paramName);
	}

	/// Check that parameter has any text (not whitespace) symbol
	/// <param name="param"></param>
	/// <exception cref="ArgumentException">Thrown when parameter don't contain any symbol exclude whitespace</exception>
	public static void HasAnyText(string param) {
		if (string.IsNullOrWhiteSpace(param))
			throw new ArgumentException();
	}

	/// Check that parameter has any text (not whitespace) symbol
	/// <param name="param"></param>
	/// <param name="paramName"></param>
	/// <exception cref="ArgumentException">Thrown when parameter don't contain any symbol exclude whitespace</exception>
	public static void HasAnyText(string param, string paramName) {
		if (string.IsNullOrWhiteSpace(param))
			throw new ArgumentException(paramName);
	}

	/// Check that array contain any item
	/// <typeparam name="T"></typeparam>
	/// <param name="param"></param>
	/// <exception cref="ArgumentException">Thrown when array is empty</exception>
	/// <exception cref="ArgumentNullException">Thrown when array is null</exception>
	public static void NotEmpty<T>(T[] param) {
		if (param == null)
			throw new ArgumentNullException();
		if (param.Length == 0)
			throw new ArgumentException();
	}

	/// Check that array contain any item
	/// <typeparam name="T"></typeparam>
	/// <param name="param"></param>
	/// <param name="paramName"></param>
	/// <exception cref="ArgumentException">Thrown when array is empty</exception>
	/// <exception cref="ArgumentNullException">Thrown when array is null</exception>
	public static void NotEmpty<T>(T[] param, string paramName) {
		if (param == null)
			throw new ArgumentNullException(paramName);
		if (param.Length == 0)
			throw new ArgumentException(paramName);
	}

	/// throw exception if value not in open interval
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <param name="minimum"></param>
	/// <param name="maximum"></param>
	public static void InOpenInterval<T>(T value, T minimum, T maximum) where T : IComparable<T> {
		if (value.CompareTo(minimum) <= 0 || value.CompareTo(maximum) >= 0)
			throw new ArgumentOutOfRangeException();
	}

	/// throw exception if value not in open interval
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <param name="minimum"></param>
	/// <param name="maximum"></param>
	/// <param name="paramName"></param>
	public static void InOpenInterval<T>(T value, T minimum, T maximum, string paramName) where T : IComparable<T> {
		if (value.CompareTo(minimum) <= 0 || value.CompareTo(maximum) >= 0)
			throw new ArgumentOutOfRangeException(paramName);
	}

	/// throw exception if value not in closed interval
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <param name="minimum"></param>
	/// <param name="maximum"></param>
	public static void InClosedInterval<T>(T value, T minimum, T maximum) where T : IComparable<T> {
		if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
			throw new ArgumentOutOfRangeException();
	}

	/// throw exception if value not in closed interval
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <param name="minimum"></param>
	/// <param name="maximum"></param>
	/// <param name="paramName"></param>
	public static void InClosedInterval<T>(T value, T minimum, T maximum, string paramName) where T : IComparable<T> {
		if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
			throw new ArgumentOutOfRangeException(paramName);
	}
}
