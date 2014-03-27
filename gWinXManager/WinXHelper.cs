using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace gWinXManager
{
	class WinXHelper
	{
		private string _strWinXPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Windows\\WinX";
		private const string _strExt = "*.lnk";	

		public struct GroupShortcut
		{
			public string Group;
			public List<ShortcutPath> Shortcut;
		}

		public struct ShortcutPath
		{
			public string Filename;
			public string Filepath;
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
						return new ShortcutPath()
						{
							Filename = Path.GetFileName(shortcutPath),
							Filepath = shortcutPath
						};
					}).ToList()
				};

				lgs.Add(gs);
			});

			return lgs;
		}
	}
}
