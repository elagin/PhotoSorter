/*
 * PhotoSorter. Copy photos from flash and sort.
 * Copyright © 2018 Pavel Elagin elagin.pasha@gmail.com

 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <www.gnu.org/licenses/>
 * 
 * Source code: https://github.com/elagin/PhotoSorter
 * 
 */
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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

            public WorkState()
            {
                filesPocessed = 0;
                totalSize = 0;
            }
        }

        private Settings settings = new Settings();
        private BackgroundWorker bgw = new BackgroundWorker();

        private const int WM_DEVICECHANGE = 0x0219;
        private const string CAPTION_ERROR = "Ошибка";
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        string root;
        private int filesPocessed = 0;
        private long totalSize = 0;

        private void scanDrives()
        {
            comboBoxDriveList.Items.Clear();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Removable)
                {
                    comboBoxDriveList.Items.Add(d.RootDirectory);
                }
            }
            if (comboBoxDriveList.Items.Count > 0)
            {
                comboBoxDriveList.SelectedIndex = 0;
                buttonStart.Enabled = true;
            }
            else
                buttonStart.Enabled = false;
            if (comboBoxDriveList.Items.Count <= 1)
                comboBoxDriveList.Enabled = false;
        }

        public Form1()
        {
            InitializeComponent();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text = "PhotoSorter " + fvi.FileVersion;

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
            }
            else
            {
                isPause = true;
                var res = MessageBox.Show("Прервать операцию?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                    bgw.CancelAsync();
                isPause = false;
            }
        }

        private bool isPause = false;

        /// <summary>
        /// Обрабатывает треки в указанной папке.</summary>
        private void folderWalker(ref BackgroundWorker bgw, string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            string[] extensions = new[] { ".ari", ".dpx", ".arw", ".srf", ".sr2", ".bay", ".crw", ".cr2", ".dng", ".dcr", ".kdc", ".erf", ".3fr", ".mef", ".mrw", ".nef", ".nrw", ".orf", ".ptx", ".pef", ".raf", ".raw", ".rwl", ".dng", ".raw", ".rw2", ".r3d", ".srw", ".x3f", ".jpg" };
            FileInfo[] files = info.GetFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();
            for(int i=0; i < files.Length; i++)
            {
                var fileInfo = files[i];
                try
                {
                    if (!bgw.CancellationPending)
                    {
                        while (isPause)
                            Thread.Sleep(500);
                        WorkState state = new WorkState();
                        string file = fileInfo.FullName;
                        int pos = file.LastIndexOf('\\');
                        string shortName = file.Substring(pos + 1, file.Length - pos - 1);

                        string[] timeFormat = File.GetCreationTimeUtc(file).GetDateTimeFormats();
                        string newFolder = timeFormat[46].Substring(2, timeFormat[46].IndexOf('T') - 2);
                        newFolder = newFolder.Replace("-", String.Empty);

                        string newFullFolder = settings.folderName + newFolder + "\\";
                        if (!Directory.Exists(newFullFolder))
                            Directory.CreateDirectory(newFullFolder);

                        long fileSize = 0;
                        FileInfo current = new FileInfo(file);
                        if (File.Exists(newFullFolder + shortName))
                        {
                            FileInfo old = new FileInfo(newFullFolder + shortName);
                            FileInfo newFile = new FileInfo(file);
                            if (newFile.Length > old.Length)
                            {
                                fileSize = copyFile(file, newFullFolder + shortName);
                                if (fileSize > 0)
                                {
                                    totalSize = totalSize + fileSize;
                                    state.totalSize = totalSize;
                                    state.filesPocessed = filesPocessed++;
                                }
                            }
                        }
                        else
                        {
                            fileSize = copyFile(file, newFullFolder + shortName);
                            if (fileSize > 0)
                            {
                                totalSize = totalSize + fileSize;
                                state.totalSize = totalSize;
                                state.filesPocessed = filesPocessed++;
                            }
                        }
                        bgw.ReportProgress(0, state);  // Обновляем информацию о результатах работы.
                    }
                    else
                    {
                        Console.Out.WriteLine("-= Work is canceled =-");
                    }
                }
                catch (Exception ex)
                {
                    var btn = MessageBox.Show(fileInfo.FullName + Environment.NewLine + ex.Message, CAPTION_ERROR, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                    switch (btn)
                    {
                        case DialogResult.Retry:
                            i--;
                            break;
                        case DialogResult.Abort:
                            bgw.CancelAsync();
                            break;
                    }
                }
            }
            foreach (var folder in info.GetDirectories())
                folderWalker(ref bgw, path + "\\" + folder.Name);
        }

        private long copyFile(string from, string to)
        {
            try
            {
                File.Copy(from, to);
                FileInfo current = new FileInfo(from);
                return current.Length;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CAPTION_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        /// <summary>
        /// Задача вторичного потока.</summary>
        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                folderWalker(ref bgw, root);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CAPTION_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Событие изменения прогресс-бара.</summary>
        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WorkState state = (WorkState)e.UserState;
            textBoxProcessed.Text = state.filesPocessed.ToString();
            textBoxTotalSize.Text = SizeSuffix(totalSize, 1);
        }

        /// <summary>
        /// Вторичный поток работу закончил.</summary>
        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //sWatch.Stop();
            buttonStart.Text = "Старт";
            MessageBox.Show("Работа завершена, хозяин.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            {
                if (!dlg.SelectedPath.EndsWith(@"\"))
                    return dlg.SelectedPath + @"\";
                return dlg.SelectedPath;
            }
            else
                return string.Empty;
        }

        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} байт", 0); }

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
            using (FormAbout about = new FormAbout())
            {
                about.ShowDialog();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
                scanDrives();
            base.WndProc(ref m); // Переопределение оконной процедуры
        }
    }
}
