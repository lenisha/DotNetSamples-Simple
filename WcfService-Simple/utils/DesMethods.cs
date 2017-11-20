using System;
using System.Security.Cryptography;
using System.Text;

namespace Pivotal
{
	public static class DESMethods
	{
		private const string SECRET_KEY = "lVbDki2d3ouW2H0v2BsJel5/R2GwpVBP";
		private const string SECRET_IV = "f6ALQJAVSpM=";

		public static string EncryptString(string plainText)
		{
			TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = Convert.FromBase64String(SECRET_KEY);
			tripleDES.IV = Convert.FromBase64String(SECRET_IV);

			Byte[] plainBytes = ASCIIEncoding.Unicode.GetBytes(plainText);

			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			CryptoStream encStream = new CryptoStream(ms, tripleDES.CreateEncryptor(), CryptoStreamMode.Write);
			encStream.Write(plainBytes, 0, plainBytes.Length);



			string encryptedText = Convert.ToBase64String(ms.ToArray());

			ms.Flush();
			encStream.FlushFinalBlock();
			ms.Dispose();
			encStream.Dispose();
			tripleDES.Dispose();

			return encryptedText;
		}

		public static string DecryptString(string encryptedText)
		{
			TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = Convert.FromBase64String(SECRET_KEY);
			tripleDES.IV = Convert.FromBase64String(SECRET_IV);

			Byte[] encrBytes = Convert.FromBase64String(encryptedText);

			System.IO.MemoryStream decms = new System.IO.MemoryStream();
			CryptoStream decStream = new CryptoStream(decms, tripleDES.CreateDecryptor(), CryptoStreamMode.Write);
			decStream.Write(encrBytes, 0, encrBytes.Length);

			string decryptedText = ASCIIEncoding.Unicode.GetString(decms.ToArray());

			decms.Flush();
			decStream.Flush();
			//decms.Dispose();
			decStream = null;
			tripleDES.Dispose();

			return decryptedText;
		}
	}
}