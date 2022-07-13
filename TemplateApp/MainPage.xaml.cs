using CubeKit.UI.Services;
using Protecc.Helpers;
using Protecc.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Protecc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);
            CredentialService.RefreshListAsync();
        }

        private void EnterKey_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddAccountPage), "");
        }

        private void ScanQR_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(ScanQRPage), "");
        }

        //  For some reason when Selectionmode is None then the list item animations don't work.
        //  This is a cursed workaround
        private void AccountsView_SelectionChanged(object sender, SelectionChangedEventArgs e) => AccountsView.SelectedItem = null;

        private async void AddButton_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);
            ((Button)sender).IsTabStop = true; //Cursed workaround for focus on startup bug
        }
    }
}
