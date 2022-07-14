using Protecc.Classes;
using Protecc.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Protecc.Controls
{
    public sealed partial class SettingsControl : UserControl
    {
        public SettingsClass Settings = new();

        public SettingsControl()
        {
            this.InitializeComponent();
            SetupSettings();
        }

        public async void SetupSettings()
        {
            if (!await KeyCredentialManager.IsSupportedAsync())
            {
                WindowsHelloText.Text = "*Windows Hello not setup";
                WindowsHelloSwitch.IsEnabled = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e) => ExportHelper.Export();

        private void OOBE_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(OOBEPage));
        }
        
        private async void GitHub_Click(object sender, RoutedEventArgs e) => await Windows.System.Launcher.LaunchUriAsync(new Uri("https://github.com/FireCubeStudios-Community/FlowBoard-FireCube-Edition"));

        private async void Discord_Click(object sender, RoutedEventArgs e) => await Windows.System.Launcher.LaunchUriAsync(new Uri("https://discord.gg/3WYcKat"));
    }
}
