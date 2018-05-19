using System;
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
        }

        //private Thread addDataRunner;
        private BackgroundWorker bgw = new BackgroundWorker();
        string root;
        private int filesPocessed = 0;

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
                //if (SaveCtrls(true))
                //{
                //    dt.Clear();
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
                    if (File.Exists(newFullFolder + shortName))
                    {
                        FileInfo old = new FileInfo(newFullFolder + shortName);
                        FileInfo newFile = new FileInfo(file);
                        if (newFile.Length > old.Length)
                        {
                            File.Copy(file, newFullFolder + shortName, true);
                        }
                    }
                    else
                    {
                        File.Copy(file, newFullFolder + shortName);
                    }
                    WorkState state = new WorkState();
                    state.filesPocessed = filesPocessed++;
                    bgw.ReportProgress(0, state);  // Обновляем информацию о результатах работы.
                    /*
                    TrackStat stat = new TrackStat();

                    if (file.Extension == ".plt")
                        stat = Drivers.ParsePlt(settings.Distaice, file.FullName, points, ref settings, false);
                    else if (file.Extension == ".gpx")
                        stat = Drivers.ParseGpx(settings.Distaice, file.FullName, points);

                    if (settings.Distaice * 1000 > (int)stat.MinDist)
                    {
                        WorkState state = new WorkState();
                        object[] arr = new object[7];
                        arr[0] = false;
                        arr[1] = stat.FileName;
                        arr[2] = stat.MinDist / 1000;
                        arr[3] = stat.Length / 1000;
                        arr[4] = stat.Points;
                        arr[5] = stat.Points / (stat.Length / 1000);
                        arr[6] = stat.MaxSpeed;

                        state.arr = arr;
                        state.path = path;

                        bgw.ReportProgress(0, state);  // Обновляем информацию о результатах работы.

                    }
                    */
                }
                // if (searchSubFolder)
                foreach (var folder in d.GetDirectories())
                    folderWalker(ref bgw, path + "\\" + folder.Name/*, searchSubFolder , points*/);
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
            textBoxProcessed.Text = state.filesPocessed.ToString();
            //dt.Rows.Add(state.arr);
            //labelCurrentFolder.Text = "Поиск: " + state.path;
            //_tracksFound++;
            //labelFoundInfo.Text = String.Format("Найдено/выбрано: {0}/0", _tracksFound);
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
    }
}
