using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace gWinXManager
{
	class WinXHelper
	{
		private object _PropertyName;
		private Dictionary<string, List<ShortcutInfo>> _dEntries = new Dictionary<string, List<ShortcutInfo>>();
		private const string _strExt = "*.lnk";
		private string _strWinXPath = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Microsoft\Windows\WinX");

		public struct GroupShortcut
		{
			public string Group;
			public List<WinXHelper.ShortcutInfo> Shortcut;
		}

		public struct ShortcutInfo
		{
			public string Filename;
			public string Filepath;
			public ImageSource Icon;
		}

		private List<GroupShortcut> _lgsEntries = new List<GroupShortcut>();

		public WinXHelper()
		{
			Load();
			copyFile("C:\\Users\\ZhongBo\\Desktop\\hashlnk.exe.lnk", "C:\\Users\\ZhongBo\\AppData\\Local\\Temp");
		}

		public void Load()
		{
			this._dEntries = this.listEntries(this._strWinXPath, "*.lnk");
		}

		private void addShortcut(string shortcutPath, string groupName)
		{
			
			
		}

		private void copyFile(string filePath, string targetPath)
		{
			if (!File.Exists(targetPath))
			{
				throw Exceptions.PathNotFound;
			}
		}

		private Dictionary<string, List<ShortcutInfo>> listEntries(string folderPath, string ext)
		{
			Dictionary<string, List<ShortcutInfo>> entries = new Dictionary<string, List<ShortcutInfo>>();
			Directory.GetDirectories(folderPath).ToList<string>().ForEach(delegate(string groupPath)
			{
				List<ShortcutInfo> ls = new List<ShortcutInfo>();
				Directory.GetFiles(groupPath, ext).ToList<string>().ForEach(delegate(string shortcutPath)
				{
					lnkHelper helper = new lnkHelper(shortcutPath);
					ShortcutInfo item = new ShortcutInfo
					{
						Filename = Path.GetFileName(shortcutPath),
						Filepath = shortcutPath,
						Icon = helper.ShortcutIcon
					};
					ls.Add(item);
				});
				entries.Add(Path.GetFileName(groupPath), ls);
			});
			return entries;
		}
	}
}
