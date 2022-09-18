using CubeKit.UI.Services;
using Protecc.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Protecc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WhatsNewPage : Page
    {
        public WhatsNewPage()
        {
            this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            Ring.Visibility = Visibility.Visible;
            Frame rootFrame = Window.Current.Content as Frame;
            if (new SettingsClass().WindowsHello)
                rootFrame.Navigate(typeof(WindowsHelloPage));
            else
                rootFrame.Navigate(typeof(MainPage));
        }
    }
}
