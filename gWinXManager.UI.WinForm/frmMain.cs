using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using gWinXManager.Core;
using System.Drawing.Imaging;

namespace gWinXManager.UI.WinForm
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
			WinXHelper wx = new WinXHelper();
			wx.Load();

			Dictionary<string, List<ShortcutInfo>> entries = new Dictionary<string, List<ShortcutInfo>>();

			entries = wx.Shortcuts;
			int i = 0;
			foreach (KeyValuePair<string, List<ShortcutInfo>> k in entries)
			{
				lbShortcuts.Items.Add("Group : " + k.Key);
				k.Value.ForEach(delegate(ShortcutInfo item)
				{
					lbShortcuts.Items.Add(item.Filename);
					lbShortcuts.Items.Add(item.Filepath);
					lbShortcuts.Items.Add(item.GroupPath);

					//item.iIcon.Save(k.Key + " - " + item.Filename + ".png", ImageFormat.Png);

					PictureBox p = new PictureBox();
					p.BorderStyle = BorderStyle.FixedSingle;
					p.Image = item.iIcon;
					p.SizeMode = PictureBoxSizeMode.AutoSize;
					p.Left = i * 32;
					p.Top = 10;
					pnIcon.Controls.Add(p);
					i++;
				});
			}
			
		}
	}
}
