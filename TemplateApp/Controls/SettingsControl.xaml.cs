using Protecc.Classes;
using Protecc.Helpers;
using System;
using Windows.Foundation;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        private async void Export_Click(object sender, RoutedEventArgs e) => await ExportHelper.Export();

        private void OOBE_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(OOBEPage));
        }
        
        private IAsyncOperation<bool> OpenLink(string linkUrl) => Windows.System.Launcher.LaunchUriAsync(new Uri(linkUrl));

        private async void GitHub_Click(object sender, RoutedEventArgs e) => await OpenLink("https://github.com/FireCubeStudios/Protecc");

        private async void Discord_Click(object sender, RoutedEventArgs e) => await OpenLink("https://discord.gg/3WYcKat");

        private async void Twitter_Click(object sender, RoutedEventArgs e) => await OpenLink("https://twitter.com/FireCubeStudios");
    }
}
