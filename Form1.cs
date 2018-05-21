﻿using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PhotoSorter
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Класс для хранения статистики по работе второго потока.</summary>
        public class WorkState
        {
            public int filesPocessed { get; set; }
            public long totalSize { get; set; }
        }

        //private Thread addDataRunner;
        private BackgroundWorker bgw = new BackgroundWorker();
        string root;
        private int filesPocessed = 0;
        private long totalSize = 0;

        private Settings settings = new Settings();

        private void scanDrives()
        {
            comboBoxDriveList.Items.Clear();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Removable /*&& d.VolumeLabel == "MyVolumeLabel"*/)
                {
                    comboBoxDriveList.Items.Add(d.RootDirectory);
                    //FileInfo file = new FileInfo(all_path);
                    //file.CopyTo(d.Name + @"\tst\test\testing");
                }

            }
            if (comboBoxDriveList.Items.Count > 0)
                comboBoxDriveList.SelectedIndex = 0;
            if (comboBoxDriveList.Items.Count <= 1)
                comboBoxDriveList.Enabled = false;
        }

        public Form1()
        {
            InitializeComponent();
            settings.load();
            textBoxFolder.Text = settings.folderName;
            scanDrives();
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProgressChanged);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            bgw.WorkerReportsProgress = true;
            bgw.WorkerSupportsCancellation = true;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!bgw.IsBusy)
            {
                filesPocessed = 0;
                totalSize = 0;
                textBoxProcessed.Text = "";
                textBoxTotalSize.Text = "";

                root = comboBoxDriveList.SelectedItem.ToString();
                bgw.RunWorkerAsync();
                buttonStart.Text = "Стоп";
                //checkBox.Checked = false;
                //_tracksFound = 0;
                //dataGridView1.DataSource = dt;
                //dataGridView1.Sort(dataGridView1.Columns[_sortColunm], ListSortDirection.Ascending);
                //}
            }
            else
                bgw.CancelAsync();
            //ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
            //addDataRunner = new Thread(addDataThreadStart);
            //addDataRunner.Start();

            /*
            textBoxProcessed.Text = "0";
            string inFolder = @"g:\DCIM\112D7000\";
            string OufFolder = @"d:\фототест\";
            string[] files = Directory.GetFiles(inFolder);
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
                string newFolder = aaa[46].Substring(2, aaa[46].IndexOf('T') - 2);
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
            */
            //System.IO.Directory directory = new System.IO.Directory();
        }

        /// <summary>
        /// Обрабатывает треки в указанной папке.</summary>
        private void folderWalker(ref BackgroundWorker bgw, string path/*, bool searchSubFolder, List<GpsPoint> points*/)
        {
            if (!bgw.CancellationPending)
            {
                DirectoryInfo d = new DirectoryInfo(path);

                string[] extensions = new[] { ".nef", ".jpg" };
                string OufFolder = @"d:\фототест2\";

                FileInfo[] files = d.GetFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();
                foreach (var fileInfo in files)
                {
                    WorkState state = new WorkState();
                    string file = fileInfo.FullName;
                    int pos = file.LastIndexOf('\\');
                    string shortName = file.Substring(pos + 1, file.Length - pos - 1);
                    DateTime date = File.GetCreationTimeUtc(file);
                    string[] aaa = date.GetDateTimeFormats();

                    string year = date.Year.ToString().Substring(2, 2);
                    string month = date.Month.ToString();
                    string day = date.Day.ToString();

                    //string newFolder = year + month + day;
                    string newFolder = aaa[46].Substring(2, aaa[46].IndexOf('T') - 2);
                    newFolder = newFolder.Replace("-", String.Empty);

                    string newFullFolder = OufFolder + newFolder + "\\";
                    if (!Directory.Exists(newFullFolder))
                    {
                        Directory.CreateDirectory(newFullFolder);
                    }
                    FileInfo current = new FileInfo(file);
                    if (File.Exists(newFullFolder + shortName))
                    {
                        FileInfo old = new FileInfo(newFullFolder + shortName);
                        FileInfo newFile = new FileInfo(file);
                        if (newFile.Length > old.Length)
                        {
                            WorkState stateCopy = copyFile(file, newFullFolder + shortName);
                            if (stateCopy != null)
                            {
                                state.filesPocessed++;
                                state.totalSize = state.totalSize + stateCopy.totalSize;
                            }
                            //File.Copy(file, newFullFolder + shortName, true);
                            //totalSize = current.Length;
                            //state.totalSize = current.Length;
                        }
                    }
                    else
                    {
                        WorkState stateCopy = copyFile(file, newFullFolder + shortName);
                        state.filesPocessed++;
                        state.totalSize = state.totalSize + stateCopy.totalSize;
                    }

                    state.filesPocessed = filesPocessed++;
                    bgw.ReportProgress(0, state);  // Обновляем информацию о результатах работы.
                }
                // if (searchSubFolder)
                foreach (var folder in d.GetDirectories())
                    folderWalker(ref bgw, path + "\\" + folder.Name/*, searchSubFolder , points*/);
            }
        }

        private WorkState copyFile(string from, string to)
        {
            try
            {
                File.Copy(from, to);
                WorkState state = new WorkState();
                FileInfo current = new FileInfo(from);
                state.totalSize = current.Length;
                state.totalSize = current.Length;
                return state;
            }
            catch (Exception ex)
            {
                const string caption = "Ошибка";
                var result = MessageBox.Show(ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Задача вторичного потока.</summary>
        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //List<GpsPoint> list = new List<GpsPoint>();
                //if (settings.SearchByPos)
                //  list.Add(_internalDegres);
                //if (settings.SearchByWpt)
                //Drivers.ParseWpt(settings.WptFileName, ref list);
                folderWalker(ref bgw, root/*, settings.SearchSubFolder, list*/);
            }
            catch (Exception ex)
            {
                const string caption = "Ошибка";
                var result = MessageBox.Show(ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Событие изменения прогресс-бара.</summary>
        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WorkState state = (WorkState)e.UserState;
            if (state.filesPocessed > 0)
                textBoxProcessed.Text = state.filesPocessed.ToString();
            if (state.totalSize > 0)
            {
                totalSize = totalSize + state.totalSize;
                textBoxTotalSize.Text = SizeSuffix(totalSize, 1);
            }
        }

        /// <summary>
        /// Вторичный поток работу закончил.</summary>
        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //sWatch.Stop();
            buttonStart.Text = "Старт";
            //labelCurrentFolder.Text = "Поиск: завершен";
            //TimeSpan tSpan = sWatch.Elapsed;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            string newFolder = getFolder();
            if (newFolder.Length > 0)
            {
                textBoxFolder.Text = newFolder;
                settings.folderName = newFolder;
                settings.save();
            }
        }

        /// <summary>
        /// Отображает диалог указания папки.</summary>
        private string getFolder()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Поиск папки";
            dlg.SelectedPath = textBoxFolder.Text;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
                return dlg.SelectedPath;
            else
                return "";
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            using (AboutForm about = new AboutForm())
            {
                DialogResult resDlg = about.ShowDialog();
            }
        }
    }
}
