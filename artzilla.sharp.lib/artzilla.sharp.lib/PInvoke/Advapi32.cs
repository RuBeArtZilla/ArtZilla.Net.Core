using System;
using System.Runtime.InteropServices;

namespace ArtZilla.Net.Core.PInvoke {

	public class Advapi32 {
		[Flags]
		public enum ContextFlags : long {
			Newkeyset = 0x8,
			Deletekeyset = 0x10,
			MachineKeyset = 0x20,
			Silent = 0x40,
			DefaultContainerOptional = 0x80,
			VerifyContext = 0xF0000000,
		}

		public enum CAlg : long {
			TriDES = 0x00006603,
			TriDES_112 = 0x00006609,
			AES = 0x00006611,
			AES_128 = 0x0000660e,
			AES_192 = 0x0000660f,
			AES_256 = 0x00006610,
			AGREEDKEY_ANY = 0x0000aa03,
			CYLINK_MEK = 0x0000660c,
			DES = 0x00006601,
			DESX = 0x00006604,
			DH_EPHEM = 0x0000aa02,
			DH_SF = 0x0000aa01,
			DSS_SIGN = 0x00002200,
			ECDH = 0x0000aa05,
			ECDH_EPHEM = 0x0000ae06,
			ECDSA = 0x00002203,
			ECMQV = 0x0000a001,
			HASH_REPLACE_OWF = 0x0000800b,
			HUGHES_MD5 = 0x0000a003,
			HMAC = 0x00008009,
			KEA_KEYX = 0x0000aa04,
			MAC = 0x00008005,
			MD2 = 0x00008001,
			MD4 = 0x00008002,
			MD5 = 0x00008003,
			NO_SIGN = 0x00002000,
			OID_INFO_CNG_ONLY = 0xffffffff,
			OID_INFO_PARAMETERS = 0xfffffffe,
			PCT1_MASTER = 0x00004c04,
			RC2 = 0x00006602,
			RC4 = 0x00006801,
			RC5 = 0x0000660d,
			RSA_KEYX = 0x0000a400,
			RSA_SIGN = 0x00002400,
			SCHANNEL_ENC_KEY = 0x00004c07,
			SCHANNEL_MAC_KEY = 0x00004c03,
			SCHANNEL_MASTER_HASH = 0x00004c02,
			SEAL = 0x00006802,
			SHA = 0x00008004,
			SHA1 = 0x00008004,
			SHA_256 = 0x0000800c,
			SHA_384 = 0x0000800d,
			SHA_512 = 0x0000800e,
			SKIPJACK = 0x0000660a,
			SSL2_MASTER = 0x00004c05,
			SSL3_MASTER = 0x00004c01,
			SSL3_SHAMD5 = 0x00008008,
			TEK = 0x0000660b,
			TLS1_MASTER = 0x00004c06,
			TLS1PRF = 0x0000800a,
		}

		public enum CryptProvider {
			RSAFull = 1
		}

		public static bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider,
																						CryptProvider prov, ContextFlags flags)
			=> CryptAcquireContext(ref hProv, pszContainer, pszProvider, (uint) prov, (uint) flags);


		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider,
																									 uint dwProvType, uint dwFlags);

		public static bool CryptReleaseContext(IntPtr hProv)
			=> CryptReleaseContext(hProv, 0);

		[DllImport("Advapi32.dll", EntryPoint = "CryptReleaseContext", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool CryptReleaseContext(IntPtr hProv, Int32 dwFlags /* Reserved. Must be 0.*/);

		public static bool CryptCreateHash(IntPtr hProv, CAlg cAlg, IntPtr hKey, uint dwFlags, ref IntPtr phHash)
			=> CryptCreateHash(hProv, (uint) cAlg, hKey, dwFlags, ref phHash);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CryptCreateHash(IntPtr hProv, uint algId, IntPtr hKey, uint dwFlags, ref IntPtr phHash);

		public static bool CryptHashData(IntPtr hHash, string data, uint flags = 0) {
			var bData =  System.Text.Encoding.ASCII.GetBytes(data);
			return CryptHashData(hHash, bData, (uint) bData.Length, flags);
		}

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool CryptHashData(IntPtr hHash, byte[] pbData, uint dataLen, uint flags);

		public static bool CryptDeriveKey(IntPtr hProv, CAlg alg, IntPtr hBaseData, int flags, ref IntPtr phKey)
			=> CryptDeriveKey(hProv, (int) alg, hBaseData, flags, ref phKey);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CryptDeriveKey(IntPtr hProv, int algId, IntPtr hBaseData, int flags, ref IntPtr phKey);

		[Flags]
		public enum DecryptFlag {
			None = 0,
			Oaep = 0x00000040,
			DecryptRSANoPaddingCheck = 0x00000020,
		}

		public static bool CryptDecrypt(IntPtr hKey, IntPtr hHash, byte[] pbData,
		                                ref uint pdwDataLen, bool final = false, DecryptFlag flag = DecryptFlag.None)
			=> CryptDecrypt(hKey, hHash, final ? -1 : 0, (uint) flag, pbData, ref pdwDataLen);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CryptDecrypt(IntPtr hKey, IntPtr hHash, int final, uint dwFlags, byte[] pbData,
																					 ref uint pdwDataLen);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool CryptDestroyKey(IntPtr phKey);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool CryptDestroyHash(IntPtr hHash);
	}
}