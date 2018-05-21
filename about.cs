using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhotoSorter
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            linkLabelVK.Links.Add(6, 4, "https://vk.com/id244452742");
            //linkLabelVK.LinkData = "http://www.dotnetperls.com/";
            //linkLabelVK.Links.Add(link);
        }

        private void linkLabelVK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://vk.com/id244452742");
        }
    }
}
