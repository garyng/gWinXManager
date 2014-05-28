using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace gWinXManager.Core
{
	public struct ShortcutInfo
	{
		public string GroupPath;
		public string Filename;
		public string Filepath;
		public ImageSource Icon;
		public Image iIcon;
	}

	//TODO : Validate file name (do not contain special characters)
	//TODO : Backup
	//TODO : Custom add shortcut
	public class WinXHelper
	{
		#region PrivateVar

		private object _PropertyName;
		private Dictionary<string, List<ShortcutInfo>> _dEntries = new Dictionary<string, List<ShortcutInfo>>();
		private const string _strExt = "*.lnk";
		private string _strWinXPath = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Microsoft\Windows\WinX");

		#endregion

		public WinXHelper()
		{
			Load();

			//addGroup("Group4");
			//addShortcut("C:\\Users\\ZhongBo\\Desktop\\hashlnk.exe.lnk", "Group4");
			//reloadExplorer();
			//deleteShortcut("4 - Control Panel.lnk", "Group4");
			//deleteGroup("Group4");
		}

		#region PublicFunc

		public void Load()
		{
			this._dEntries = this.listEntries(this._strWinXPath, "*.lnk");
		}

		public void AddGroup(string groupName)
		{
			addGroup(groupName);
		}

		public void DeleteGroup(string groupName)
		{
			deleteGroup(groupName);
		}

		public void AddShortcut(string shortcutPath, string groupName)
		{
			addShortcut(shortcutPath, groupName);
		}

		public void DeleteShortcut(string shortcutName, string groupName)
		{
			deleteShortcut(shortcutName, groupName);
		}
		
		public void Restart()
		{
			reloadExplorer();
		}

		#endregion

		#region PublicProp
		
		public Dictionary<string, List<ShortcutInfo>> Shortcuts
		{
			get
			{
				return _dEntries;
			}
		}

		#endregion

		#region PrivateFunc

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
				_dEntries.Add(groupName, new List<ShortcutInfo>());
			}
		}

		private void deleteGroup(string groupName)
		{
			string groupPath = getPathFromGroupName(groupName);
			if (Directory.Exists(groupPath))
			{
				deleteDirectory(groupPath);
			}
			else
			{
				throw Exceptions.PathNotFound;
			}
		}

		private void addShortcut(string shortcutPath, string groupName)
		{
			List<ShortcutInfo> lsi = new List<ShortcutInfo>();
			if (_dEntries.TryGetValue(groupName, out lsi))
			{
				string groupPath = getPathFromGroupName(groupName);
				string targetPath = copyFile(shortcutPath, groupPath);
				HashlnkHelper hl = new HashlnkHelper(targetPath);
				hl.Create();

				Console.WriteLine(hl.Hash);

				_dEntries[groupName].Add(loadShortcut(targetPath, groupPath));
			}
			else
			{
				throw Exceptions.GroupNotVaild;
			}
		}

		private void deleteShortcut(string shortcutName, string groupName)
		{
			string shortcutPath = getPathFromGroupName(groupName) + "\\" + shortcutName;

			if (!File.Exists(shortcutPath))
			{
				throw Exceptions.PathNotFound;
			}

			List<ShortcutInfo> lsi = new List<ShortcutInfo>();
			if (_dEntries.TryGetValue(groupName, out lsi))
			{
				_dEntries[groupName].RemoveAll(item => item.Filepath == shortcutPath);
				deleteFile(shortcutPath);
			}
			else
			{
				throw Exceptions.GroupNotVaild;
			}
		}

		private void reloadExplorer()
		{
			foreach (Process p in Process.GetProcesses())
			{
				try
				{
					if (p.MainModule.FileName.ToLower().EndsWith(":\\windows\\explorer.exe"))
					{
						p.Kill();
						break;
					}
				}
				catch { }
			}
			Process.Start("explorer.exe");
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

		private void deleteFile(string filePath)
		{
			File.Delete(filePath);
		}

		private void createDirectory(string targetPath)
		{
			Directory.CreateDirectory(targetPath);
		}

		private void deleteDirectory(string dirPath)
		{
			Directory.Delete(dirPath, true);
		}

		private string getPathFromGroupName(string groupName)
		{
			return _strWinXPath + "\\" + groupName;
		}

		private ShortcutInfo loadShortcut(string shortcutPath, string groupPath)
		{
			lnkHelper lh = new lnkHelper(shortcutPath);
			ShortcutInfo si = new ShortcutInfo
			{
				GroupPath = groupPath,
				Filename = Path.GetFileName(shortcutPath),
				Filepath = shortcutPath,
				Icon = lh.ShortcutIcon,
				iIcon = lh.ShortcutIconImage
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

		#endregion

	
	}
}
