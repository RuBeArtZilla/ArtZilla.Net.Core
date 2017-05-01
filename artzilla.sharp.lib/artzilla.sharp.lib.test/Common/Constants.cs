using System;

namespace ArtZilla.Sharp.Lib.Test.Common {
	public static class Constants {
		public const Int32 MagicNumber = 42;

		public const String NullString = null;
		public const String EmptyString = "";
		public const String WhitespaceString = " ";
		public const String WhitespacesString = "     ";
		public const String TestString = "Hello World!";
		public const String TestStringEx = "Привет 日本!";

		public static Int32[] NullArray = null;
		public static Int32[] EmptyArray = { };
		public static Int32[] MagicArray = { 4, 8, 15, 16, 23, 42 };
	}
}
