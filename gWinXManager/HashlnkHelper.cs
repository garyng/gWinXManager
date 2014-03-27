using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gWinXManager.ShellProvider;

namespace gWinXManager
{
	class HashlnkHelper
	{
		private string _strFilePath;
		private IShellItem2 _isiShell;
		private string _strSalt = "do not prehash links.  this should only be done by the user.";

		public static Exception PathNotFound = new Exception("Specific file does not exist.");
		public static Exception TargetNotFound = new Exception("Failed to retrieve target arguments.");
		public static Exception HashFailed = new Exception("Failed to hash data.");
		public static Exception PropertyStoreFailed = new Exception("Failed to get property store.")

		public HashlnkHelper(string FilePath)
		{
			 _strFilePath = FilePath;
		}

		private IShellItem2 createShellItem(string filepath)
		{
			IShellItem2 isi2;
			APIs.SHCreateItemFromParsingName(filepath, IntPtr.Zero, typeof(IShellItem2).GUID, out isi2);
			return isi2;
		}

		private string getShortcutTarget(IShellItem2 isi)
		{
			int hr;
			string target;
			hr = isi.GetString(PropertyKeys.PKEY_Link_TargetParsingPath, out target);
			if (!checkResult(hr))
			{
				throw PathNotFound;
			}
			return target;
		}

		private string getShortcutArgs(IShellItem2 isi)
		{
			int hr;
			string args;
			hr = isi.GetString(PropertyKeys.PKEY_Link_Arguments, out args);
			if (!checkResult(hr))
			{
				throw TargetNotFound;
			}
			return args;
		}

		private UInt32 createHash(IShellItem2 isi ,string target, string args, string salt)
		{
			string strHash = target;
			if (args != null)
			{
				strHash += args;
			}
			strHash += args;
			strHash = strHash.ToLower();

			byte[] blob = Encoding.Unicode.GetBytes(strHash);
			byte[] hashBuffer = new byte[4];

			int hr;
			hr = APIs.HashData(blob, blob.Length, hashBuffer, hashBuffer.Length);
			if (!checkResult(hr))
			{
				throw HashFailed;
			}
			UInt32 hash = BitConverter.ToUInt32(hashBuffer, 0);
			return hash;
		}

		private IPropertyStore getPropertyStore(IShellItem2 isi)
		{
			IPropertyStore ips;
			int hr;
			hr = isi.GetPropertyStore(GETPROPERTYSTOREFLAGS.GPS_READWRITE, typeof(IPropertyStore).GUID, out ips);
			if (!checkResult(hr))
			{
				throw PropertyStoreFailed;
			}
			return ips;
		}

		private bool checkResult(int hResult)
		{
			return hResult == APIs.S_OK;
		}

		public void CreateHashlnk()
		{
			IShellItem2 isi;
			isi = createShellItem(_strFilePath);

			string target = getShortcutTarget(isi);
			string args = getShortcutArgs(isi);
			UInt32 hash = createHash(isi, target, args, _strSalt);



		}


	}
}
