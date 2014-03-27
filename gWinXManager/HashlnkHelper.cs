using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using gWinXManager.ShellProvider;
using MS.WindowsAPICodePack.Internal;

namespace gWinXManager
{
	class HashlnkHelper
	{
		#region Public 
		
		public static Exception PathNotFound = new Exception("Specific file does not exist.");
		public static Exception TargetNotFound = new Exception("Failed to retrieve target arguments.");
		public static Exception HashFailed = new Exception("Failed to hash data.");
		public static Exception PropertyStoreFailed = new Exception("Failed to get property store.");
		public static Exception PropertySetFailed = new Exception("Failed to set property store value.");
		public static Exception PropertyCommitFailed = new Exception("Failed to write changes to .lnk.");

		#endregion

		#region Private

		private string _strFilePath;
		private string _strSalt = "do not prehash links.  this should only be done by the user.";

		#endregion

		#region Struct
		
		struct PathGUID
		{
			public string path;
			public string GUID;
		}

		#endregion

		#region Const
		
		const string FOLDERID_ProgramFiles = "{905e63b6-c1bf-494e-b29c-65b732d3d21a}";
		const string FOLDERID_System = "{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}";
		const string FOLDERID_Windows = "{F38BF404-1D43-42F2-9305-67DE0B28FC23}";

		#endregion

		public HashlnkHelper(string FilePath)
		{
			 _strFilePath = FilePath;
		}

		public void Create()
		{
			IShellItem2 isi;
			isi = createShellItem(_strFilePath);

			string target = getShortcutTarget(isi);
			string args = getShortcutArgs(isi);

			target = generalizePath(target);

			UInt32 hash = createHash(isi, target, args, _strSalt);

			IPropertyStore ips = getPropertyStore(isi);
			setHash(ips, hash);

			Marshal.FinalReleaseComObject(ips);
			Marshal.FinalReleaseComObject(isi);
			
		}

		#region Private Func
		
		private IShellItem2 createShellItem(string filepath)
		{
			IShellItem2 isi2;
			APIs.SHCreateItemFromParsingName(filepath, IntPtr.Zero, typeof(IShellItem2).GUID, out isi2);
			return isi2;
		}

		private UInt32 createHash(IShellItem2 isi, string target, string args, string salt)
		{
			string strHash = target;
			if (args != null)
			{
				strHash += args;
			}
			strHash += salt;
			strHash = strHash.ToLower();

			byte[] blob = Encoding.Unicode.GetBytes(strHash);
			byte[] hashBuffer = new byte[4];

			int hr;
			hr = APIs.HashData(blob, blob.Length, hashBuffer, hashBuffer.Length);
			checkResult(hr, HashFailed);

			UInt32 hash = BitConverter.ToUInt32(hashBuffer, 0);
			return hash;
		}

		private string getShortcutTarget(IShellItem2 isi)
		{
			int hr;
			string target;
			hr = isi.GetString(PropertyKeys.PKEY_Link_TargetParsingPath, out target);
			checkResult(hr, PathNotFound);
			return target;
		}

		private string getShortcutArgs(IShellItem2 isi)
		{
			int hr;
			string args;
			hr = isi.GetString(PropertyKeys.PKEY_Link_Arguments, out args);
			if (hr != APIs.S_OK && hr != APIs.HRESULT_FROM_WIN32(APIs.ERROR_NOT_FOUND))
			{
				throw TargetNotFound;
			}

			return args;
		}

		private string getProgramFilesPath()
		{
			if (Environment.Is64BitOperatingSystem)
			{
				return Environment.GetEnvironmentVariable("ProgramW6432");
			}
			else
			{
				return Environment.GetEnvironmentVariable("ProgramFiles");
			}
		}

		private string generalizePath(string filepath)
		{
			PathGUID[] pg = new PathGUID[3]
			{
				new PathGUID()
				{
					path = getProgramFilesPath(),
					GUID = FOLDERID_ProgramFiles
				},
				new PathGUID()
				{
					path = Environment.GetEnvironmentVariable("SystemRoot") + "\\System32",
					GUID = FOLDERID_System
				},
				new PathGUID()
				{
					path = Environment.GetEnvironmentVariable("SystemRoot"),
					GUID = FOLDERID_Windows
				}
			};
			string generalizedPath = filepath;
			CompareInfo comp = CultureInfo.InvariantCulture.CompareInfo;
			for (int i = 0; i < pg.Count(); i++)
			{
				if (comp.IsPrefix(filepath, pg[i].path, CompareOptions.IgnoreCase))
				{
					generalizedPath = pg[i].GUID + generalizedPath.Substring(pg[i].path.Length);
					break;
				}
			}
			return generalizedPath;
		}

		private IPropertyStore getPropertyStore(IShellItem2 isi)
		{
			IPropertyStore ips;
			int hr;
			hr = isi.GetPropertyStore(GETPROPERTYSTOREFLAGS.GPS_READWRITE, typeof(IPropertyStore).GUID, out ips);
			checkResult(hr, PropertyStoreFailed);
			return ips;
		}

		private void setHash(IPropertyStore ips, UInt32 hash)
		{
			PropVariant pv = new PropVariant(hash);
			pv.VarType = VarEnum.VT_UI4;

			int hr;
			hr = (int)ips.SetValue(PropertyKeys.PKEY_WinX_Hash, pv);
			checkResult(hr, PropertySetFailed);

			hr = (int)ips.Commit();
			checkResult(hr, PropertyCommitFailed);

		}

		private void checkResult(int hResult, Exception ex)
		{
			if (hResult != APIs.S_OK)
			{
				throw ex;
			}
		}

		#endregion


	}
}
