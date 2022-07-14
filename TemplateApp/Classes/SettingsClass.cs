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

        //  Used in first run
        public void Setup()
        {
            localSettings.Values["WindowsHello"] = localSettings.Values["LaunchBlur"] = false;
            localSettings.Values["FocusBlur"] = true;
        }
    }
}
