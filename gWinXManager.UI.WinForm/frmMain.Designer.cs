namespace gWinXManager.UI.WinForm
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lbShortcuts = new System.Windows.Forms.ListBox();
			this.pnIcon = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// lbShortcuts
			// 
			this.lbShortcuts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbShortcuts.FormattingEnabled = true;
			this.lbShortcuts.Location = new System.Drawing.Point(12, 12);
			this.lbShortcuts.Name = "lbShortcuts";
			this.lbShortcuts.ScrollAlwaysVisible = true;
			this.lbShortcuts.Size = new System.Drawing.Size(469, 147);
			this.lbShortcuts.TabIndex = 0;
			// 
			// pnIcon
			// 
			this.pnIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnIcon.AutoScroll = true;
			this.pnIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnIcon.Location = new System.Drawing.Point(13, 184);
			this.pnIcon.Name = "pnIcon";
			this.pnIcon.Size = new System.Drawing.Size(468, 69);
			this.pnIcon.TabIndex = 1;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(493, 416);
			this.Controls.Add(this.pnIcon);
			this.Controls.Add(this.lbShortcuts);
			this.Name = "frmMain";
			this.Text = "gWinXManager";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbShortcuts;
		private System.Windows.Forms.Panel pnIcon;
	}
}

