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
	class Hashlnk
	{

		#region Private

		private string _strFilePath;
		private uint _iHash;

		#endregion

		#region Struct

		struct PathGUID
		{
			public string path;
			public string GUID;
		}

		#endregion

		#region Const

		private const string FOLDERID_ProgramFiles = "{905e63b6-c1bf-494e-b29c-65b732d3d21a}";
		private const string FOLDERID_System = "{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}";
		private const string FOLDERID_Windows = "{F38BF404-1D43-42F2-9305-67DE0B28FC23}";
		private const string _strSalt = "do not prehash links.  this should only be done by the user.";

		#endregion

		#region Public Func

		public Hashlnk(string filepath)
		{
			_strFilePath = filepath;
		}

		public void Create()
		{
			using (lnkHelper lh = new lnkHelper(_strFilePath))
			{
				string target = lh.TargetPath;
				string args = lh.Arguments;

				target = generalizePath(target);

				UInt32 hash = createHash(target, args, _strSalt);
				_iHash = hash;

				lh.CreatePropertyStore();
				PropVariant pv = new PropVariant(hash);
				pv.VarType = VarEnum.VT_UI4;
				lh.SetPropertyStoreValue(PropertyKeys.PKEY_WinX_Hash, pv);
			}
		}

		#endregion

		#region Public Prop

		public uint Hash
		{
			get
			{
				return _iHash;
			}
		}

		#endregion

		#region Private Func

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

		private UInt32 createHash(string target, string args, string salt)
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
			checkResult(hr, Exceptions.HashFailed);

			UInt32 hash = BitConverter.ToUInt32(hashBuffer, 0);
			return hash;
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
