using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using gWinXManager.ShellProvider;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;

namespace gWinXManager
{
	class lnkHelper : IDisposable
	{
		#region Public

		public static Exception PathNotFound = new Exception("Specific file does not exist.");
		public static Exception GetArgsFailed = new Exception("Failed to retrieve target's arguments.");
		public static Exception GetDesFailed = new Exception("Failed to retrieve target description.");
		public static Exception GetIconLocFailed = new Exception("Failed to retrieve target's icon location.");
		public static Exception PropertyStoreFailed = new Exception("Failed to get property store.");
		public static Exception PropertySetFailed = new Exception("Failed to set property store value.");
		public static Exception PropertyCommitFailed = new Exception("Failed to write changes to .lnk.");
		public static Exception SetTargetFailed = new Exception("Failed to set shortcut's target path.");
		public static Exception SetDesFailed = new Exception("Failed to set shortcut's description.");
		public static Exception SetArgFailed = new Exception("Failed to set shortcut's arguments.");
		
		#endregion

		#region Private

		private string _strFilePath;
		private IShellItem2 _isiShell;
		private IPropertyStore _ipsStore;

		private string _strTarget;
		private string _strArgs;
		private string _strDes;
		private int _iIconIndex;
		private string _strIconLoc;

		private bool _disposed = false;

		private IShellLinkW _islwShell = (IShellLinkW)new CShellLink();
		private IPersistFile _ipfFile;
		private bool _isNew = false;

		#endregion

		#region Public Func

		/// <summary>
		/// Load a lnk file from the filepath
		/// </summary>
		/// <param name="filepath">Filepath to the lnk file</param>
		public lnkHelper(string filepath)
		{
			_ipfFile = _islwShell as IPersistFile;
			_ipfFile.Load(filepath, STGM.STGM_READ);

			_strFilePath = filepath;
			loadLnk();

		}

		/// <summary>
		/// Create a new lnk file
		/// </summary>
		public lnkHelper()
		{
			_ipfFile = _islwShell as IPersistFile;
			_isNew = true;
		}

		public void CreatePropertyStore()
		{
			_ipsStore = getPropertyStore(_isiShell);
		}

		public void SetPropertyStoreValue(PropertyKey pk, PropVariant pv)
		{
			setPropertyStoreValue(_ipsStore, pk, pv);
		}

		/// <summary>
		/// Save the created shortcut
		/// </summary>
		/// <param name="filename"></param>
		public void Save(string filename)
		{
			saveShortcut(filename);
		}

		#endregion

		#region Public Props

		public string FilePath
		{
			get
			{
				return _strFilePath;
			}
		}

		public string TargetPath
		{
			get
			{
				return _strTarget;
			}
			set
			{
				if (_isNew)
				{
					int hr = (int)_islwShell.SetPath(value);
					checkResult(hr, SetTargetFailed);
				}
			}
		}

		public string Description
		{
			get
			{
				return _strDes;
			}
			set
			{
				if (_isNew)
				{
					int hr = (int)_islwShell.SetDescription(value);
					checkResult(hr, SetDesFailed);
				}
			}
		}

		public string Arguments
		{
			get
			{
				return _strArgs;
			}
			set
			{
				if (_isNew)
				{
					int hr = (int)_islwShell.SetArguments(value);
					checkResult(hr, SetArgFailed);
				}
			}
		}

		public int IconIndex
		{
			get
			{
				return _iIconIndex;
			}
			set
			{
				_iIconIndex = value;
			}
		}

		public string IconLocation
		{
			get
			{
				return _strIconLoc;
			}
			set
			{
				_strIconLoc = value;
			}
		}

		#endregion

		#region Private Func

		private void loadLnk()
		{
			_isiShell = createShellItem(_strFilePath);
			_strTarget = getShortcutTarget(_isiShell);
			_strArgs = getShortcutArgs(_isiShell);

			int iconIndex;
			_strIconLoc = getIconLocation(_islwShell, out iconIndex);
			_iIconIndex = iconIndex;

			_strDes = getShortcutDescription(_islwShell);

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
				throw GetArgsFailed;
			}

			return args;
		}

		private string getShortcutDescription(IShellLinkW islw)
		{
			StringBuilder sb = new StringBuilder(APIs.INFOTIPSIZE);
			islw.GetDescription(sb, sb.Capacity);

			return sb.ToString();
		}

		private string getIconLocation(IShellLinkW islw, out int iconIndex)
		{
			StringBuilder sb = new StringBuilder(1024);
			int index;
			int hr = (int)islw.GetIconLocation(sb, sb.Capacity, out index);
			checkResult(hr, GetIconLocFailed);
			iconIndex = index;
			return sb.ToString();
		}

		private void setPropertyStoreValue(IPropertyStore ips, PropertyKey pk, PropVariant pv)
		{
			int hr;
			hr = (int)ips.SetValue(pk, pv);
			checkResult(hr, PropertySetFailed);

			hr = (int)ips.Commit();
			checkResult(hr, PropertyCommitFailed);
		}

		private IPropertyStore getPropertyStore(IShellItem2 isi)
		{
			IPropertyStore ips;
			int hr;
			hr = isi.GetPropertyStore(GETPROPERTYSTOREFLAGS.GPS_READWRITE, typeof(IPropertyStore).GUID, out ips);
			checkResult(hr, PropertyStoreFailed);
			return ips;
		}

		private void saveShortcut(string filename)
		{
			if (_isNew)
			{
				_ipfFile.Save(filename,true);
			}
		}

		private void checkResult(int hResult, Exception ex)
		{
			if (hResult != APIs.S_OK)
			{
				throw ex;
			}
		}

		#endregion

		#region Disposer

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool IsDisposing)
		{
			if (_disposed)
			{
				return;
			}

			Marshal.FinalReleaseComObject(_isiShell);
			Marshal.FinalReleaseComObject(_ipsStore);
			Marshal.FinalReleaseComObject(_islwShell);
			Marshal.FinalReleaseComObject(_ipfFile);

			_disposed = true;
		}

		~lnkHelper()
		{
			Dispose(false);
		}

		#endregion


	}
}
