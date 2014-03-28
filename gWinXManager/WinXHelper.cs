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
		private string _strWinXPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Windows\\WinX";
		private const string _strExt = "*.lnk";	

		public struct GroupShortcut
		{
			public string Group;
			public List<ShortcutInfo> Shortcut;
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
		}

		public void Load()
		{
			_lgsEntries = listGroups(_strWinXPath, _strExt);
		}

		//Help to rebuild into functions
		private List<GroupShortcut> listGroups(string folderPath, string ext)
		{
			List<GroupShortcut> lgs = new List<GroupShortcut>();
			Directory.GetDirectories(folderPath).ToList().ForEach(delegate(string groupPath)
			{
				GroupShortcut gs = new GroupShortcut()
				{
					Group = Path.GetFileName(groupPath),
					Shortcut = Directory.GetFiles(groupPath,ext).Select(delegate(string shortcutPath)
					{
						lnkHelper lh = new lnkHelper(shortcutPath);
						return new ShortcutInfo()
						{
							Filename = Path.GetFileName(shortcutPath),
							Filepath = shortcutPath,
							Icon = lh.ShortcutIcon
						};
					}).ToList()
				};

				lgs.Add(gs);
			});

			return lgs;
		}
	}
}
