using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Protecc.Classes
{
    public class SettingsClass
    {
        private ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public bool WindowsHello
        {
            get { return (bool)localSettings.Values["WindowsHello"]; }
            set { localSettings.Values["WindowsHello"] = value; }
        }
        public bool LaunchBlur
        {
            get { return (bool)localSettings.Values["LaunchBlur"]; }
            set { localSettings.Values["LaunchBlur"] = value; }
        }
        public bool FocusBlur
        {
            get { return (bool)localSettings.Values["FocusBlur"]; }
            set { localSettings.Values["FocusBlur"] = value; }
        }
        public int LaunchCount
        {
            get { return (int)localSettings.Values["LaunchCount"]; }
            set { localSettings.Values["LaunchCount"] = value; }
        }

        //  Used in first run
        public void Setup()
        {
            localSettings.Values["WindowsHello"] = localSettings.Values["LaunchBlur"] = false;
            localSettings.Values["FocusBlur"] = true;
            localSettings.Values["LaunchCount"] = 1;
        }

        public void Update()
        {
            localSettings.Values["LaunchCount"] = 1;
        }
    }
}
