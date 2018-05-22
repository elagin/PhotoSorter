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
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PhotoSorter
{
	public partial class FormAbout : Form
	{
		[DllImport("shell32.dll")]
		public static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation,
			string lpFile, string lpParameters, string lpDirectory, int nShowCmd); 
		public FormAbout()
		{
			InitializeComponent();

			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

			//this.Text = String.Format("Orbis Terrarum v.{0}", fvi.FileVersion);

			string about = "PhotoSorter - бесплатное и свободное приложение для копирования фотографий с карт памяти на компьютер с последующей сортировкой по датам." + Environment.NewLine + "Распространяется на основе лицензии GPL v2.0 по принципу 'как есть'." + Environment.NewLine + Environment.NewLine +
				"* Автор не несёт ответственности за любые действия пользователя использующего программу, вся ответственность за использование программой целиком и полностью ложиться на пользователя." + Environment.NewLine +
"* Автор не несёт ответственности за любые аппаратные и/или программные ошибки возникающие при работе программы." + Environment.NewLine +
"* Автор не несёт ответственности за не совпадения ожиданиям пользователя и функционалом программы.";
			richTextBox1.Text = about;
			richTextBox1.Select(0, 14);
			Font currentFont = richTextBox1.SelectionFont;
			Font bold = new Font(currentFont.Name, currentFont.Size + 2, FontStyle.Bold);
			richTextBox1.SelectionFont = bold;
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void linkEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ShellExecute(IntPtr.Zero, "open", "mailto:elagin.pasha@gmail.com?subject=photosorter%20feedback&body=Здравствуйте.", "", "", 4 /* sw_shownoactivate */);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://github.com/elagin/photosorter");
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://sourceforge.net/projects/photosorter/");
		}

		private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://www.gnu.org/licenses");
		}
	}
}
