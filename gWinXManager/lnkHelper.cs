using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using gWinXManager.ShellProvider;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;
using System.Windows;

namespace gWinXManager
{
	class lnkHelper:IDisposable
	{
		#region Public 
		
		public static Exception PathNotFound = new Exception("Specific file does not exist.");
		public static Exception TargetNotFound = new Exception("Failed to retrieve target arguments.");
		public static Exception PropertyStoreFailed = new Exception("Failed to get property store.");
		public static Exception PropertySetFailed = new Exception("Failed to set property store value.");
		public static Exception PropertyCommitFailed = new Exception("Failed to write changes to .lnk.");

		#endregion

		#region Private
		
		private string _strFilePath;
		private IShellItem2 _isiShell;
		private IPropertyStore _ipsStore;

		private string _strTarget;
		private string _strArgs;

		private bool _disposed = false;

		#endregion

		#region Public Func
		
		public lnkHelper(string filepath)
		{
			_strFilePath = filepath;
		}

		public void Load()
		{
			_isiShell = createShellItem(_strFilePath);
			_strTarget = getShortcutTarget(_isiShell);
			_strArgs = getShortcutArgs(_isiShell);

		}
		
		public void CreatePropertyStore()
		{
			_ipsStore = getPropertyStore(_isiShell);
		}

		public void SetPropertyStoreValue(PropertyKey pk, PropVariant pv)
		{
			setPropertyStoreValue(_ipsStore, pk, pv);
		}		 

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
				_strTarget = value;
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
				_strArgs = value;
			}
		}

		#endregion

		#region Private Func

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
				throw TargetNotFound;
			}

			return args;
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

			_disposed = true;
		}

		~lnkHelper()
		{
			Dispose(false);
		}

		#endregion


	}
}
