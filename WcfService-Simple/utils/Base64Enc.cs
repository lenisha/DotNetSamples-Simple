using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pivotal
{
	public class Base64Methods
	{
		public static byte[] DecodeCert(string certString)
		{
			// var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(certString);
			byte[] cert = System.Convert.FromBase64String(certString);
			return cert;
		}

		public static string EncodeCert(string certlocation)
		{
			byte[] certbytes = File.ReadAllBytes(certlocation);
			//var plainBytes = System.Text.Encoding.UTF8.GetBytes(cert);
			return System.Convert.ToBase64String(certbytes);
		}
	}
}
