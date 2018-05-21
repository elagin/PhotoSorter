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
