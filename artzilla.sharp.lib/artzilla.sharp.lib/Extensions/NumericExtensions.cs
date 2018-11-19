namespace ArtZilla.Net.Core.Extensions {
	public static class NumberUtils {
		/// <summary>
		/// todo: add description
		/// method from https://stackoverflow.com/a/33325313
		/// </summary>
		/// <param name="high"></param>
		/// <param name="low"></param>
		/// <returns></returns>
		public static long MakeLong(int high, int low)
			=> (long) high << 32 | (long)(uint)low;

		/// <summary>
		/// todo: add description
		/// </summary>
		/// <param name="high"></param>
		/// <param name="low"></param>
		/// <returns></returns>
		public static long MakeLong(int high, uint low)
			=> (long) high << 32 | low;

		/// <summary>
		/// todo: add description
		/// </summary>
		/// <param name="high"></param>
		/// <param name="low"></param>
		/// <returns></returns>
		public static long MakeLong(uint high, uint low)
			=> (long) high << 32 | low;
	}
}