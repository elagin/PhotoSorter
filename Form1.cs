using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PhotoSorter
{
    public partial class Form1 : Form
    {
        private int processedFiles = 0;
        private Thread addDataRunner;

        public Form1()
        {
            InitializeComponent();
        }

        private void AddDataThreadLoop()
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
            //addDataRunner = new Thread(addDataThreadStart);
            //addDataRunner.Start();

            textBoxProcessed.Text = "0";
            string inFolder = @"g:\DCIM\112D7000\";
            string OufFolder = @"d:\фототест\";
            string[] files = System.IO.Directory.GetFiles(inFolder);
            foreach (string file in files)
            {
                int pos = file.LastIndexOf('\\');
                string shortName = file.Substring(pos + 1, file.Length - pos - 1);
                DateTime date = File.GetCreationTimeUtc(file);
                string[] aaa = date.GetDateTimeFormats();
           
                string year = date.Year.ToString().Substring(2, 2);
                string month = date.Month.ToString();
                string day = date.Day.ToString();

                //string newFolder = year + month + day;
                string newFolder = aaa[46].Substring(2, aaa[46].IndexOf('T')-2);
                newFolder = newFolder.Replace("-", String.Empty);

                string newFullFolder = OufFolder + newFolder + "\\";
                if (!Directory.Exists(newFullFolder))
                {
                    Directory.CreateDirectory(newFullFolder);
                }
                if (!File.Exists(newFullFolder + shortName))
                    File.Copy(file, newFullFolder + shortName);
                textBoxProcessed.Text = (++processedFiles).ToString();
            }
            //System.IO.Directory directory = new System.IO.Directory();
        }
    }
}
