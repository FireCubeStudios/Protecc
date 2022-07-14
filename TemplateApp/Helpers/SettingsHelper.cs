using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Protecc.Helpers
{
    public class SettingsHelper
    {
        private static ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static void Setup()
        {
            localSettings.Values["WindowsHello"] = false;
            localSettings.Values["LaunchBlur"] = false;
            localSettings.Values["FocusBlur"] = true;
        }

        public static bool GetWindowsHello() => (bool)localSettings.Values["WindowsHello"];

        public static bool GetLaunchBlur() => (bool)localSettings.Values["LaunchBlur"];

        public static bool GetFocusBlur() => (bool)localSettings.Values["FocusBlur"];

        public static void SetWindowsHello(bool boolean) => localSettings.Values["WindowsHello"] = boolean;

        public static void SetLaunchBlur(bool boolean) => localSettings.Values["LaunchBlur"] = boolean;

        public static void SetFocusBlur(bool boolean) => localSettings.Values["FocusBlur"] = boolean;
    }
}
