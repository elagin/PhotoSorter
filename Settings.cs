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
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoSorter
{
    class Settings
    {
        private readonly string APP_NAME = "photoSorter";
        private readonly string CACH_FOLDER_NAME = "folderName";

        public string folderName;

        public void save()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
            key.CreateSubKey(APP_NAME);
            key = key.OpenSubKey(APP_NAME, true);
            if (folderName != null)
                key.SetValue(CACH_FOLDER_NAME, folderName);
        }

        public void load()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software");
            key = key.OpenSubKey(APP_NAME);
            if (key != null)
            {
                folderName = key.GetValue(CACH_FOLDER_NAME, "").ToString();
            }
        }
    }
}
