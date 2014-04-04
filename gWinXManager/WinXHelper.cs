using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace gWinXManager
{
	//TODO : Validate file name (do not contain special characters)
	class WinXHelper
	{
		private object _PropertyName;
		private Dictionary<string, List<ShortcutInfo>> _dEntries = new Dictionary<string, List<ShortcutInfo>>();
		private const string _strExt = "*.lnk";
		private string _strWinXPath = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Microsoft\Windows\WinX");
		

		public struct ShortcutInfo
		{
			public string GroupPath;
			public string Filename;
			public string Filepath;
			public ImageSource Icon;
		}

		public WinXHelper()
		{
			Load();
			//addShortcut("C:\\Users\\ZhongBo\\Desktop\\hashlnk.exe.lnk", "Group4");
			//addGroup("Group4");
		}

		public void Load()
		{
			this._dEntries = this.listEntries(this._strWinXPath, "*.lnk");
		}

		private void addGroup(string groupName)
		{
			List<ShortcutInfo> lsi = new List<ShortcutInfo>();
			if (_dEntries.TryGetValue(groupName, out lsi))
			{
				throw Exceptions.GroupExist;
			}
			else
			{
				string targetPath = Path.Combine(_strWinXPath, groupName);
				createDirectory(targetPath);
				
			}
		}

		private void addShortcut(string shortcutPath, string groupName)
		{
			List<ShortcutInfo> lsi = new List<ShortcutInfo>();
			if (_dEntries.TryGetValue(groupName,out lsi))
			{
				string groupPath = _strWinXPath + "\\" + groupName;
				string targetPath =  copyFile(shortcutPath, groupPath);
				Hashlnk hl = new Hashlnk(targetPath);
				hl.Create();

				Console.WriteLine(hl.Hash);

				_dEntries[groupName].Add(loadShortcut(targetPath,groupPath));
			}
			else
			{
				throw Exceptions.GroupNotVaild;
			}			
		}

		private string copyFile(string filePath, string targetPath)
		{
			if (!Directory.Exists(targetPath) || !File.Exists(filePath))
			{
				throw Exceptions.PathNotFound;
			}

			string fileName = Path.GetFileName(filePath);
			targetPath = Path.Combine(targetPath, fileName);

			File.Copy(filePath, targetPath, true);
			return targetPath;
		}

		private void createDirectory(string targetPath)
		{
			Directory.CreateDirectory(targetPath);
		}

		private ShortcutInfo loadShortcut(string shortcutPath, string groupPath)
		{
			lnkHelper lh = new lnkHelper(shortcutPath);
			ShortcutInfo si = new ShortcutInfo
			{
				GroupPath = groupPath,
				Filename = Path.GetFileName(shortcutPath),
				Filepath = shortcutPath,
				Icon = lh.ShortcutIcon
			};
			return si;
		}

		private Dictionary<string, List<ShortcutInfo>> listEntries(string folderPath, string ext)
		{
			Dictionary<string, List<ShortcutInfo>> entries = new Dictionary<string, List<ShortcutInfo>>();
			Directory.GetDirectories(folderPath).ToList<string>().ForEach(delegate(string groupPath)
			{
				List<ShortcutInfo> ls = new List<ShortcutInfo>();
				Directory.GetFiles(groupPath, ext).ToList<string>().ForEach(delegate(string shortcutPath)
				{
					ls.Add(loadShortcut(shortcutPath, groupPath));
				});
				entries.Add(Path.GetFileName(groupPath), ls);
			});
			return entries;
		}
	}
}
