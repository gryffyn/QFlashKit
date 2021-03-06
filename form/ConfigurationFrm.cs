﻿using System;
using System.Drawing;
using System.Windows.Forms;
using QFlashKit.code.Utility;

namespace QFlashKit.form
{
    public class ConfigurationFrm : Form
    {
        private CheckBox chkMD5;

        public ConfigurationFrm()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            chkMD5 = new CheckBox();
            SuspendLayout();
            chkMD5.AutoSize = true;
            chkMD5.Location = new Point(53, 42);
            chkMD5.Name = "chkMD5";
            chkMD5.Size = new Size(156, 16);
            chkMD5.TabIndex = 0;
            chkMD5.Text = "check MD5 before flash";
            chkMD5.UseVisualStyleBackColor = true;
            chkMD5.CheckedChanged += chkMD5_CheckedChanged;
            AutoScaleDimensions = new SizeF(6f, 12f);
            ClientSize = new Size(548, 282);
            Controls.Add(chkMD5);
            Name = "ConfigurationFrm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Configuration";
            Load += ConfigurationFrm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void chkMD5_CheckedChanged(object sender, EventArgs e)
        {
            MiAppConfig.SetValue("checkMD5", chkMD5.Checked.ToString());
        }

        private void ConfigurationFrm_Load(object sender, EventArgs e)
        {
            chkMD5.Checked = MiAppConfig.GetAppConfig("checkMD5").ToLower() == "true";
        }
    }
}